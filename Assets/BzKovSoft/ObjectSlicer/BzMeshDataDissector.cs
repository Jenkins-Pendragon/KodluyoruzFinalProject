using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BzKovSoft.ObjectSlicer
{
	public class BzMeshDataDissector
	{
		const float MinWidth = 0.001f;
		readonly IBzSliceAddapter _adapter;
		Plane _plane;
		Material _defaultSliceMaterial;
		bool _sliced = false;

		BzMeshData _meshDataNeg;
		BzMeshData _meshDataPos;
		int[][] _subMeshes;

		public Material DefaultSliceMaterial
		{
			get { return _defaultSliceMaterial; }
			set { _defaultSliceMaterial = value; }
		}
		public BzMeshData SliceResultNeg { get { return _meshDataNeg; } }
		public BzMeshData SliceResultPos { get { return _meshDataPos; } }
		public BzSliceConfiguration Configuration { get; private set; }
		public List<PolyMeshData> CapsNeg { get; private set; }
		public List<PolyMeshData> CapsPos { get; private set; }

		public BzMeshDataDissector(Mesh mesh, Plane plane, Material[] materials, IBzSliceAddapter adapter, BzSliceConfiguration configuration)
		{
			_adapter = adapter;
			_plane = plane;
			Configuration = configuration;

			if (Configuration != null && Configuration.SliceMaterial == null)
			{
				Configuration.SliceMaterial = null;
			}

			_meshDataNeg = new BzMeshData(mesh, materials);
			_meshDataPos = new BzMeshData(mesh, materials);

			_subMeshes = new int[mesh.subMeshCount][];
			for (int subMeshIndex = 0; subMeshIndex < mesh.subMeshCount; ++subMeshIndex)
			{
				_subMeshes[subMeshIndex] = mesh.GetTriangles(subMeshIndex);
			}
		}

		public SliceResult Slice()
		{
			if (_sliced)
				throw new InvalidOperationException("Object already sliced");

			_sliced = true;

			if (Configuration == null)
				return SliceMesh(_defaultSliceMaterial);

			switch (Configuration.SliceType)
			{
				case SliceType.Slice:
					return SliceMesh(Configuration.SliceMaterial ?? _defaultSliceMaterial);

				case SliceType.KeepOne:
					return _plane.GetSide(_adapter.GetObjectCenterInWorldSpace()) ?
						SliceResult.Pos : SliceResult.Neg;

				case SliceType.Duplicate:
					return SliceResult.Duplicate;

				default: throw new NotSupportedException();
			}
		}

		private SliceResult SliceMesh(Material sectionViewMaterial)
		{
			var planeInverted = new Plane(-_plane.normal, -_plane.distance);

			BzMeshDataEditor meshEditorNeg = new BzMeshDataEditor(_meshDataNeg, _plane, _adapter);
			BzMeshDataEditor meshEditorPos = new BzMeshDataEditor(_meshDataPos, planeInverted, _adapter);

			for (int subMeshIndex = 0; subMeshIndex < _subMeshes.Length; ++subMeshIndex)
			{
				int[] newTriangles = _subMeshes[subMeshIndex];

				int trCount = newTriangles.Length / 3;
				var trianglesNeg = new List<BzTriangle>(trCount);
				var trianglesPos = new List<BzTriangle>(trCount);
				var trianglesNegSliced = new List<BzTriangle>(trCount / 10);
				var trianglesPosSliced = new List<BzTriangle>(trCount / 10);

				for (int i = 0; i < trCount; ++i)
				{
					int trIndex = i * 3;
					var bzTriangle = new BzTriangle(
						newTriangles[trIndex + 0],
						newTriangles[trIndex + 1],
						newTriangles[trIndex + 2]);

					Vector3 v1 = _adapter.GetWorldPos(bzTriangle.i1);
					Vector3 v2 = _adapter.GetWorldPos(bzTriangle.i2);
					Vector3 v3 = _adapter.GetWorldPos(bzTriangle.i3);
					bool side1 = _plane.GetSide(v1);
					bool side2 = _plane.GetSide(v2);
					bool side3 = _plane.GetSide(v3);
					bool PosSide = side1 | side2 | side3;
					bool NegSide = !side1 | !side2 | !side3;

					if (NegSide & PosSide)
					{
						bzTriangle.DivideByPlane(
							meshEditorNeg, meshEditorPos,
							trianglesNegSliced, trianglesPosSliced,
							side1, side2, side3);
					}
					else if (NegSide)
					{
						trianglesNeg.Add(bzTriangle);
					}
					else if (PosSide)
					{
						trianglesPos.Add(bzTriangle);
					}
					else
						throw new InvalidOperationException();
				}

				OptimizeEdgeTriangles(meshEditorNeg, _meshDataNeg, trianglesNegSliced);
				OptimizeEdgeTriangles(meshEditorPos, _meshDataPos, trianglesPosSliced);
				_meshDataNeg.SubMeshes[subMeshIndex] = MakeTriangleToList(trianglesNeg, trianglesNegSliced);
				_meshDataPos.SubMeshes[subMeshIndex] = MakeTriangleToList(trianglesPos, trianglesPosSliced);
			}

			CapsNeg = meshEditorNeg.CapSlice(sectionViewMaterial);
			CapsPos = meshEditorPos.CapSlice(sectionViewMaterial);

			meshEditorNeg.DeleteUnusedVertices();
			meshEditorPos.DeleteUnusedVertices();

			if (!CheckNewMesh(_meshDataNeg))
			{
				return SliceResult.Pos;
			}
			if (!CheckNewMesh(_meshDataPos))
			{
				return SliceResult.Neg;
			}

			return SliceResult.Sliced;
		}

		private static void OptimizeEdgeTriangles(BzMeshDataEditor meshEditor, BzMeshData meshData, List<BzTriangle> bzTriangles)
		{
			var edgeLoops = meshEditor.GetEdgeLoopsByIndex();

			bool[] trToDelete = new bool[bzTriangles.Count];

			var edgeLoopsNode = edgeLoops.First;
			while (edgeLoopsNode != null)
			{
				var edgeLoop = edgeLoopsNode.Value;
				edgeLoopsNode = edgeLoopsNode.Next;

				var edge = edgeLoop.first;
				int counter = edgeLoop.size;
				while (counter > 0 & edgeLoop.size >= 3)
				{
					--counter;

					var edgeItem1 = edge;
					var edgeItem2 = edgeItem1.next;
					var edgeItem3 = edgeItem2.next;

					int i1 = edgeItem1.value;
					int i2 = edgeItem2.value;
					int i3 = edgeItem3.value;

					var v1 = meshData.Vertices[i1];
					var v2 = meshData.Vertices[i2];
					var v3 = meshData.Vertices[i3];

					if (v1 == v2)
					{
						EmptyRedundantIndex(i2, i3, bzTriangles, trToDelete);
						edgeItem2.Remove();
						continue;
					}

					var dir1 = (v2 - v1).normalized;
					var dir2 = (v3 - v2).normalized;

					if (dir1 == dir2)
					{
						EmptyRedundantIndex(i2, i3, bzTriangles, trToDelete);
						edgeItem2.Remove();
					}
					else
						edge = edge.next;
				}
			}

			// remove empty
			int count = 0;
			for (int i = 0; i < bzTriangles.Count; i++)
			{
				var value = bzTriangles[i];
				bzTriangles[count] = value;

				if (!trToDelete[i])
					++count;
			}

			bzTriangles.RemoveRange(count, bzTriangles.Count - count);
		}

		public void RebuildNegMesh(Renderer meshRenderer)
		{
			var mesh = _meshDataNeg.GenerateMesh();
			_adapter.RebuildMesh(mesh, _meshDataNeg.Materials, meshRenderer);
		}

		public void RebuildPosMesh(Renderer meshRenderer)
		{
			var mesh = _meshDataPos.GenerateMesh();
			_adapter.RebuildMesh(mesh, _meshDataPos.Materials, meshRenderer);
		}

		private static void EmptyRedundantIndex(int indexMiddle, int indexNext, List<BzTriangle> bzTriangles, bool[] trToDelete)
		{
			// make redundants empty
			for (int i = 0; i < bzTriangles.Count; i++)
			{
				var tr = bzTriangles[i];
				if (trToDelete[i])
					continue;

				if (tr.i1 == indexMiddle | tr.i2 == indexMiddle | tr.i3 == indexMiddle)
				{
					if (tr.i1 == indexNext | tr.i2 == indexNext | tr.i3 == indexNext)
					{
						trToDelete[i] = true;
					}
					else
					{
						if (tr.i1 == indexMiddle)
						{
							bzTriangles[i] = new BzTriangle(indexNext, tr.i2, tr.i3);
						}
						else if (tr.i2 == indexMiddle)
						{
							bzTriangles[i] = new BzTriangle(tr.i1, indexNext, tr.i3);
						}
						else if (tr.i3 == indexMiddle)
						{
							bzTriangles[i] = new BzTriangle(tr.i1, tr.i2, indexNext);
						}
					}
				}
			}
		}

		private bool CheckNewMesh(BzMeshData meshData)
		{
			if (meshData.SubMeshes.All(s => s.Length == 0))
				return false;

			if (!CheckMinWidth(meshData))
				return false;

			return _adapter.Check(meshData);
		}

		private bool CheckMinWidth(BzMeshData meshData)
		{
			if (meshData.Vertices.Count < 3)
				return false;

			for (int i = 0; i < meshData.Vertices.Count; i++)
			{
				var pos = _adapter.GetWorldPos(meshData, i);
				float dist = _plane.GetDistanceToPoint(pos);
				if (Math.Abs(dist) > MinWidth)
					return true;
			}

			return false;
		}

		/// <summary>
		/// Get mesh triangle list from BzTriangle list
		/// </summary>
		static int[] MakeTriangleToList(List<BzTriangle> bzTriangles, List<BzTriangle> bzTrianglesExtra)
		{
			int[] triangles = new int[(bzTriangles.Count + bzTrianglesExtra.Count) * 3];
			for (int i = 0; i < bzTriangles.Count; ++i)
			{
				var tr = bzTriangles[i];
				int shift = i * 3;
				triangles[shift + 0] = tr.i1;
				triangles[shift + 1] = tr.i2;
				triangles[shift + 2] = tr.i3;
			}

			for (int i = 0; i < bzTrianglesExtra.Count; ++i)
			{
				var tr = bzTrianglesExtra[i];
				int shift = (bzTriangles.Count + i) * 3;
				triangles[shift + 0] = tr.i1;
				triangles[shift + 1] = tr.i2;
				triangles[shift + 2] = tr.i3;
			}

			return triangles;
		}
	}
}
