using UnityEngine;
using System.Collections.Generic;
using System;

namespace BzKovSoft.ObjectSlicer
{
	/// <summary>
	/// Polygon creator
	/// </summary>
	class BzPoly
	{
		readonly LinkedLoop<int> _edgeLoop;
		Vector2[] _polyVertices2D;
		readonly BzMeshData _meshData;
		private List<int> _newTriangles;
		/// <summary>
		/// False if it is impossible to create a polygon
		/// </summary>
		public bool Created { get; private set; }

		/// <param name="vertices">Chain of vertices for polygon</param>
		/// <param name="normal">Normal the polygon is facing to</param>
		public BzPoly(BzMeshData meshData, LinkedLoop<int> edgeLoop, Vector3 normal, IBzSliceAddapter adapter)
		{
			_meshData = meshData;
			_edgeLoop = edgeLoop;

			if (edgeLoop.size < 3)
				return;

			_polyVertices2D = ConvertV3ToV2(adapter, normal);


			var newTriangles1 = MakeMesh(false);
			var newTriangles2 = MakeMesh(true);

			// get triangle list with more vertices
			if (newTriangles1.Count > newTriangles2.Count)
				_newTriangles = newTriangles1;
			else
				_newTriangles = newTriangles2;

			if (_newTriangles.Count != 0)
			{
				Created = true;
			}
		}

		/// <summary>
		/// Try to make mesh
		/// </summary>
		/// <param name="right">Clockwise if True</param>
		/// <returns>True if polygon was created</returns>
		private List<int> MakeMesh(bool right)
		{
			var newTriangles = new List<int>(_polyVertices2D.Length - 2);
			if (_polyVertices2D.Length < 3)
				return newTriangles;

			var linkList = new LinkedLoop<int>();
			for (int i = 0; i < _polyVertices2D.Length; ++i)
				linkList.AddLast(i);


			var node = linkList.first;
			int counter = 0;
			while (linkList.size > 2 & counter <= linkList.size)
			{
				var node1 = node;
				var node2 = node1.next;
				var node3 = node2.next;
				var node4 = node3.next;

				var i1 = node1.value;
				var i2 = node2.value;
				var i3 = node3.value;
				var i4 = node4.value;

				Vector2 v1 = _polyVertices2D[i1];
				Vector2 v2 = _polyVertices2D[i2];
				Vector2 v3 = _polyVertices2D[i3];
				Vector2 v4 = _polyVertices2D[i4];

				if ((v2 - v1).normalized == (v3 - v1).normalized)
				{
					node2.Remove();
					continue;
				}

				if ((v3 - v2).normalized == (v4 - v2).normalized)
				{
					node3.Remove();
					continue;
				}

				if (node1 == linkList.first)
				{
					var nodePrev = node1.previous;    // needed only for start

					var iPrev = nodePrev.value;
					Vector2 vPrev = _polyVertices2D[iPrev];
					if ((v2 - v1).normalized == (v1 - vPrev).normalized)
					{
						node1.Remove();
						node = node2;
						continue;
					}
				}

				++counter;

				bool allowed = IsAllowedToCreateTriangle(linkList, i1, i2, i3, right);

				if (allowed)
				{
					CreateTriangle(newTriangles, i1, i2, i3, right);
					node2.Remove();
					node = node3;
					counter = 0;
				}
				else
					node = node2;
			}

			return newTriangles;
		}

		/// <summary>
		/// Transfom vertices from modal space to plane space and convert them to Vector2[]
		/// </summary>
		private Vector2[] ConvertV3ToV2(IBzSliceAddapter adapter, Vector3 normal)
		{
			var rotation = Quaternion.FromToRotation(normal, Vector3.forward);
			var v2s = new Vector2[_edgeLoop.size];


			var edge = _edgeLoop.first;
			for (int i = 0; i < _edgeLoop.size; i++)
			{
				Vector3 v3 = rotation * adapter.GetWorldPos(_meshData, edge.value);
				v2s[i] = v3;

				edge = edge.next;
			}

			return v2s;
		}

		/// <param name="right">Clockwise if True</param>
		private static void CreateTriangle(List<int> triangles, int i1, int i2, int i3, bool right)
		{
			triangles.Add(i1);

			if (right)
			{
				triangles.Add(i2);
				triangles.Add(i3);
			}
			else
			{
				triangles.Add(i3);
				triangles.Add(i2);
			}
		}

		/// <summary>
		/// Check if triangle in right sequence and other points does not in it
		/// </summary>
		/// <param name="right">Clockwise if True</param>
		private bool IsAllowedToCreateTriangle(LinkedLoop<int> linkList, int i1, int i2, int i3, bool right)
		{
			Vector2 v1 = _polyVertices2D[i1];
			Vector2 v2 = _polyVertices2D[i2];
			Vector2 v3 = _polyVertices2D[i3];

			var node = linkList.first;
			int counter = linkList.size;
			while (counter != 0)
			{
				--counter;

				int i = node.value;
				node = node.next;

				if (i == i1 | i == i2 | i == i3)
					continue;

				var p = _polyVertices2D[i];
				bool b1 = PointInTriangle(ref p, ref v1, ref v2, ref v3);
				if (b1)
					return false;
			}

			Vector3 vA = v1 - v2;
			Vector3 vB = v3 - v2;
			Vector3 vC = Vector3.Cross(vA, vB);

			if (right)
				return vC.z < 0;
			else
				return vC.z > 0;
		}

		/// <summary>
		/// It is 0 on the line, and +1 on one side, -1 on the other side.
		/// </summary>
		static float SideOfLine(ref Vector2 p, ref Vector2 a, ref Vector2 b)
		{
			return (b.x - a.x) * (p.y - a.y) - (b.y - a.y) * (p.x - a.x);
		}

		/// <summary>
		/// True if point resides inside a triangle
		/// </summary>
		static bool PointInTriangle(ref Vector2 pt, ref Vector2 v1, ref Vector2 v2, ref Vector2 v3)
		{
			float s1 = SideOfLine(ref pt, ref v1, ref v2);
			float s2 = SideOfLine(ref pt, ref v2, ref v3);
			float s3 = SideOfLine(ref pt, ref v3, ref v1);

			return
				(s1 > 0 & s2 > 0 & s3 > 0) |
				(s1 < 0 & s2 < 0 & s3 < 0);
		}

		/// <summary>
		/// Generete and return mesh of polygon
		/// </summary>
		public PolyMeshData GetMeshData()
		{
			if (!Created)
				throw new InvalidOperationException("You cannot get mesh if Created == False");

			Vector2[] polyVertices2d;
			int[] edgeValues;
			int[] triangles;
			OptimizeData(out polyVertices2d, out edgeValues, out triangles);

			PolyMeshData meshData = new PolyMeshData();

			// triangles
			meshData.triangles = triangles;

			// vertices
			meshData.vertices = new Vector3[edgeValues.Length];
			for (int i = 0; i < edgeValues.Length; i++)
			{
				var index = edgeValues[i];
				Vector3 v = _meshData.Vertices[index];
				meshData.vertices[i] = v;
			}

			// normals
			meshData.normals = new Vector3[edgeValues.Length];
			for (int i = 0; i < meshData.triangles.Length; i += 3)
			{
				int i1 = meshData.triangles[i + 0];
				int i2 = meshData.triangles[i + 1];
				int i3 = meshData.triangles[i + 2];

				var v1 = meshData.vertices[i1];
				var v2 = meshData.vertices[i2];
				var v3 = meshData.vertices[i3];

				var dir1 = v2 - v1;
				var dir2 = v3 - v1;
				var normal = Vector3.Cross(dir1.normalized, dir2.normalized);

				meshData.normals[i1] += normal;
				meshData.normals[i2] += normal;
				meshData.normals[i3] += normal;
			}

			// normalize normals
			for (int i = 0; i < meshData.normals.Length; i++)
			{
				var n = Normalize(meshData.normals[i]);
#if DEBUG
				// TODO: move it to tests
				if (n.sqrMagnitude == 0f)
					throw new InvalidOperationException("meshData.normals[i].sqrMagnitude == 0f");
#endif
				meshData.normals[i] = n;
			}

			// uv
			float wMax, wMin, hMax, hMin;
			var first2D = polyVertices2d[0];
			wMax = wMin = first2D.x;
			hMax = hMin = first2D.y;
			for (int i = 1; i < polyVertices2d.Length; ++i)
			{
				var v = polyVertices2d[i];
				if (v.x > wMax) wMax = v.x;
				if (v.x < wMin) wMin = v.x;
				if (v.y > hMax) hMax = v.y;
				if (v.y < hMin) hMin = v.y;
			}
			float sizeX = wMax - wMin;
			float sizeY = hMax - hMin;
			float scale = Mathf.Max(sizeX, sizeY);

			Vector2 vMin = new Vector2(
				wMin + (sizeX - scale) / 2,
				hMin + (sizeY - scale) / 2);

			meshData.uv = new Vector2[polyVertices2d.Length];
			for (int i = 0; i < polyVertices2d.Length; ++i)
			{
				var v = polyVertices2d[i] - vMin;

				meshData.uv[i] = new Vector2(v.x / scale, v.y / scale);
			}

			// bone weights
			if (_meshData.BoneWeightsExists)
			{
				meshData.boneWeights = new BoneWeight[edgeValues.Length];
				for (int i = 0; i < edgeValues.Length; i++)
				{
					int index = edgeValues[i];
					BoneWeight bw = _meshData.BoneWeights[index];
					meshData.boneWeights[i] = bw;
				}
			}
			else
				meshData.boneWeights = new BoneWeight[0];

			return meshData;
		}

		private Vector3 Normalize(Vector3 v)
		{
			// I do not know why, but v.Normalized do not work with very small values

			float magnitudeSqr = v.x * v.x + v.y * v.y + v.z * v.z;
			float magnitude = Mathf.Sqrt(magnitudeSqr);
			float f = 1f / magnitude;
			return new Vector3(v.x * f, v.y * f, v.z * f);
		}

		private void OptimizeData(out Vector2[] polyVertices2D, out int[] edgeValues, out int[] triangles)
		{
			bool[] inUseValues = new bool[_edgeLoop.size];

			for (int i = 0; i < _newTriangles.Count; ++i)
			{
				int trIndex = _newTriangles[i];
				inUseValues[trIndex] = true;
			}

			// count how much values in use
			int newLength = 0;
			for (int i = 0; i < inUseValues.Length; i++)
			{
				var inUse = inUseValues[i];
				if (inUse)
					++newLength;
			}

			var vertexIndexShift = new int[_edgeLoop.size];

			int index = 0;
			polyVertices2D = new Vector2[newLength];
			edgeValues = new int[newLength];
			var edge = _edgeLoop.first;
			for (int i = 0; i < _edgeLoop.size; i++)
			{
				var edgeValue = edge.value;
				edge = edge.next;
				var inUse = inUseValues[i];

				if (!inUse)
				{
#if DEBUG
					// TODO: move it to tests
					vertexIndexShift[i] = -1;
#endif
					continue;
				}

				vertexIndexShift[i] = index;
				polyVertices2D[index] = _polyVertices2D[i];
				edgeValues[index] = edgeValue;
				++index;
			}

			triangles = new int[_newTriangles.Count];
			for (int i = 0; i < triangles.Length; i++)
			{
				var trIndex = _newTriangles[i];
				trIndex = vertexIndexShift[trIndex];
				triangles[i] = trIndex;
			}
		}
	}

	public class PolyMeshData
	{
		public Vector3[] vertices;
		public Vector3[] normals;
		public Vector2[] uv;
		public int[] triangles;
		public BoneWeight[] boneWeights;
	}
}