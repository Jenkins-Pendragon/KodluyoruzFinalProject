using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

namespace BzKovSoft.ObjectSlicer
{
	class BzMeshDataEditor
	{
		public List<IndexVector> CapEdges = new List<IndexVector>();
		readonly IBzSliceAddapter _adapter;

		readonly Plane _planeLocal;
		readonly Dictionary<IndexVector, int> _chashedOuterLines;
		readonly BzMeshData _meshData;


		public BzMeshDataEditor(BzMeshData meshData, Plane planeLocal, IBzSliceAddapter adapter)
		{
			_meshData = meshData;
			_adapter = adapter;
			_planeLocal = planeLocal;
			_chashedOuterLines = new Dictionary<IndexVector, int>();
		}

		/// <summary>
		/// Generate new index in triangle list that resides in plane
		/// </summary>
		public int GetIndexFor(int from, int to)
		{
			var oldDir = new IndexVector(from, to);
			int result;
			if (_chashedOuterLines.TryGetValue(oldDir, out result))
				return result;

			result = _meshData.Vertices.Count;
			_chashedOuterLines.Add(oldDir, result);

			float ratioIn = CalcGetRatio(from, to);


			{ // Vertex
				Vector3 vFrom = _meshData.Vertices[from];
				Vector3 vTo = _meshData.Vertices[to];
				Vector3 vNew = Vector3.Lerp(vFrom, vTo, ratioIn);
				_meshData.Vertices.Add(vNew);
			}

			if (_meshData.NormalsExists)
			{
				Vector3 nFrom = _meshData.Normals[from];
				Vector3 nTo = _meshData.Normals[to];
				Vector3 nNew = Vector3.Slerp(nFrom, nTo, ratioIn);
				_meshData.Normals.Add(nNew);
			}

			if (_meshData.ColorsExists)
			{
				Color colorFrom = _meshData.Colors[from];
				Color colorTo = _meshData.Colors[to];
				Color colorNew = Color.Lerp(colorFrom, colorTo, ratioIn);
				_meshData.Colors.Add(colorNew);
			}
			if (_meshData.Colors32Exists)
			{
				Color32 colorFrom = _meshData.Colors32[from];
				Color32 colorTo = _meshData.Colors32[to];
				Color32 colorNew = Color32.Lerp(colorFrom, colorTo, ratioIn);
				_meshData.Colors32.Add(colorNew);
			}

			if (_meshData.UVExists)
			{
				AddValue(_meshData.UV, from, to, ratioIn);
			}
			if (_meshData.UV2Exists)
			{
				AddValue(_meshData.UV2, from, to, ratioIn);
			}
			if (_meshData.UV3Exists)
			{
				AddValue(_meshData.UV3, from, to, ratioIn);
			}
			if (_meshData.UV4Exists)
			{
				AddValue(_meshData.UV4, from, to, ratioIn);
			}

			if (_meshData.TangentsExists)
			{
				Vector4 vFrom = _meshData.Tangents[from];
				Vector4 vTo = _meshData.Tangents[to];
				Vector4 vNew = Vector4.Lerp(vFrom, vTo, ratioIn);
				_meshData.Tangents.Add(vNew);
			}

			if (_meshData.BoneWeightsExists)
			{
				var w1 = _meshData.BoneWeights[from];
				var w2 = _meshData.BoneWeights[to];

				var ws = new Dictionary<int, float>();
				float ratioOut = 1 - ratioIn;

				if (w1.weight0 != 0) ws.Add(w1.boneIndex0, w1.weight0 * ratioIn);
				if (w1.weight1 != 0) ws.Add(w1.boneIndex1, w1.weight1 * ratioIn);
				if (w1.weight2 != 0) ws.Add(w1.boneIndex2, w1.weight2 * ratioIn);
				if (w1.weight3 != 0) ws.Add(w1.boneIndex3, w1.weight3 * ratioIn);

				if (w2.weight0 != 0)
				{
					float fA;
					ws.TryGetValue(w2.boneIndex0, out fA);
					ws[w2.boneIndex0] = fA + w2.weight0 * ratioOut;
				}
				if (w2.weight1 != 0)
				{
					float fA;
					ws.TryGetValue(w2.boneIndex1, out fA);
					ws[w2.boneIndex1] = fA + w2.weight1 * ratioOut;
				}
				if (w2.weight2 != 0)
				{
					float fA;
					ws.TryGetValue(w2.boneIndex2, out fA);
					ws[w2.boneIndex2] = fA + w2.weight2 * ratioOut;
				}
				if (w2.weight3 != 0)
				{
					float fA;
					ws.TryGetValue(w2.boneIndex3, out fA);
					ws[w2.boneIndex3] = fA + w2.weight3 * ratioOut;
				}

				var newBoneWeight = new BoneWeight();
				var wsArr = ws
					.Where(v => v.Value != 0)
					.Take(4)
					.OrderByDescending(v => v.Value)
					.ToArray();
				KeyValuePair<int, float>[] wsArr4 = new KeyValuePair<int, float>[4];
				Array.Copy(wsArr, wsArr4, wsArr.Length);
				
				float weightSum = 0;
				weightSum += wsArr4[0].Value;
				weightSum += wsArr4[1].Value;
				weightSum += wsArr4[2].Value;
				weightSum += wsArr4[3].Value;

				float weightRatio = 1 / weightSum;

				if (wsArr.Length > 0)
				{
					newBoneWeight.boneIndex0 = wsArr[0].Key;
					newBoneWeight.weight0 = wsArr[0].Value * weightRatio;
				}
				if (wsArr.Length > 1)
				{
					newBoneWeight.boneIndex1 = wsArr[1].Key;
					newBoneWeight.weight1 = wsArr[1].Value * weightRatio;
				}
				if (wsArr.Length > 2)
				{
					newBoneWeight.boneIndex2 = wsArr[2].Key;
					newBoneWeight.weight2 = wsArr[2].Value * weightRatio;
				}
				if (wsArr.Length > 3)
				{
					newBoneWeight.boneIndex3 = wsArr[3].Key;
					newBoneWeight.weight3 = wsArr[3].Value * weightRatio;
				}

				_meshData.BoneWeights.Add(newBoneWeight);
			}

			return result;
		}

		private static void AddValue(List<Vector2> vectors, int from, int to, float ratioL)
		{
			Vector2 nFrom = vectors[from];
			Vector2 nTo = vectors[to];
			Vector2 nNew = Vector2.Lerp(nFrom, nTo, ratioL);
			vectors.Add(nNew);
		}

		public List<PolyMeshData> CapSlice(Material capMaterial)
		{
			// get all section's vertices and create a meshes from them.
			// Then combine these meshes to single 'sectionMesh' (with one submesh),
			// add this mesh to 'mesh' and add new section material

			var edgeLoops = GetEdgeLoops();
			var capMeshes = new List<PolyMeshData>(edgeLoops.Count);
			foreach (var edgeLoop in edgeLoops)
			{
				var poly = new BzPoly(_meshData, edgeLoop, _planeLocal.normal, _adapter);
				if (!poly.Created)
					continue;

				capMeshes.Add(poly.GetMeshData());
			}

			if (capMeshes.Count != 0)
			{
				AddMeshes(capMeshes);

				if (_meshData.MaterialsExists)
				{
					Material[] materials = _meshData.Materials;
					var newMaterials = new Material[materials.Length + 1];
					for (int i = 0; i < materials.Length; ++i)
						newMaterials[i] = materials[i];

					newMaterials[materials.Length] = capMaterial;
					_meshData.Materials = newMaterials;
				}
			}

			return capMeshes;
		}

		/// <summary>
		/// Optimize mesh: deleting all vertices that present in mesh but not present in mesh's triangle list
		/// </summary>
		public void DeleteUnusedVertices()
		{
			DeleteEmptySubmeshes();

			bool[] used = new bool[_meshData.Vertices.Count];

			int newVertexCount = 0;
			for (var i = 0; i < _meshData.SubMeshes.Length; ++i)
			{
				var subMesh = _meshData.SubMeshes[i];
				for (var j = 0; j < subMesh.Length; ++j)
				{
					int vIndex = subMesh[j];
					if (!used[vIndex])
						++newVertexCount;
					used[vIndex] = true;
				}
			}

			var oldToNewShift = new int[_meshData.Vertices.Count];

			newVertexCount = 0;
			for (int i = 0; i < used.Length; ++i)
			{
				if (!used[i])
					continue;

				_meshData.Vertices[newVertexCount] = _meshData.Vertices[i];
				oldToNewShift[i] = newVertexCount;

				++newVertexCount;
			}

			if (_meshData.NormalsExists)
				DeleteUnusedItem(used, _meshData.Normals);

			if (_meshData.ColorsExists)
				DeleteUnusedItem(used, _meshData.Colors);
			if (_meshData.Colors32Exists)
				DeleteUnusedItem(used, _meshData.Colors32);

			if (_meshData.UVExists)
				DeleteUnusedItem(used, _meshData.UV);
			if (_meshData.UV2Exists)
				DeleteUnusedItem(used, _meshData.UV2);
			if (_meshData.UV3Exists)
				DeleteUnusedItem(used, _meshData.UV3);
			if (_meshData.UV4Exists)
				DeleteUnusedItem(used, _meshData.UV4);

			if (_meshData.TangentsExists)
				DeleteUnusedItem(used, _meshData.Tangents);

			if (_meshData.BoneWeightsExists)
				DeleteUnusedItem(used, _meshData.BoneWeights);

			// shift sub meshes
			for (int smi = 0; smi < _meshData.SubMeshes.Length; ++smi)
			{
				var subMesh = _meshData.SubMeshes[smi];
				for (int vertIndex = 0; vertIndex < subMesh.Length; ++vertIndex)
					subMesh[vertIndex] = oldToNewShift[subMesh[vertIndex]];
			}

			// shift crossSegments
			for (int i = 0; i < CapEdges.Count; i++)
			{
				var segment = CapEdges[i];

				CapEdges[i] = new IndexVector(oldToNewShift[segment.From], oldToNewShift[segment.To]);
			}

			// truncate
			_meshData.Vertices.RemoveRange(newVertexCount, _meshData.Vertices.Count - newVertexCount);
			if (_meshData.NormalsExists)
				_meshData.Normals.RemoveRange(newVertexCount, _meshData.Normals.Count - newVertexCount);

			if (_meshData.ColorsExists)
				_meshData.Colors.RemoveRange(newVertexCount, _meshData.Colors.Count - newVertexCount);
			if (_meshData.Colors32Exists)
				_meshData.Colors32.RemoveRange(newVertexCount, _meshData.Colors32.Count - newVertexCount);

			if (_meshData.UVExists)
				_meshData.UV.RemoveRange(newVertexCount, _meshData.UV.Count - newVertexCount);
			if (_meshData.UV2Exists)
				_meshData.UV2.RemoveRange(newVertexCount, _meshData.UV2.Count - newVertexCount);
			if (_meshData.UV3Exists)
				_meshData.UV3.RemoveRange(newVertexCount, _meshData.UV3.Count - newVertexCount);
			if (_meshData.UV4Exists)
				_meshData.UV4.RemoveRange(newVertexCount, _meshData.UV4.Count - newVertexCount);

			if (_meshData.TangentsExists)
				_meshData.Tangents.RemoveRange(newVertexCount, _meshData.Tangents.Count - newVertexCount);

			if (_meshData.BoneWeightsExists)
				_meshData.BoneWeights.RemoveRange(newVertexCount, _meshData.BoneWeights.Count - newVertexCount);
		}

		private static void DeleteUnusedItem<T>(bool[] used, List<T> elements)
		{
			int newIndex = 0;
			for (int i = 0; i < used.Length; ++i)
			{
				if (!used[i])
					continue;

				elements[newIndex] = elements[i];

				++newIndex;
			}
		}

		static bool TryAttachEdgeToList(IndexVector segment, LinkedLoop<int> toList)
		{
			var first = toList.first.value;
			if (first == segment.From)
			{
				toList.AddFirst(segment.To);
				return true;
			}
			if (first == segment.To)
			{
				toList.AddFirst(segment.From);
				return true;
			}

			var last = toList.last.value;
			if (last == segment.From)
			{
				toList.AddLast(segment.To);
				return true;
			}
			if (last == segment.To)
			{
				toList.AddLast(segment.From);
				return true;
			}

			return false;
		}
		
		/// <summary>
		/// Get vertices that resides on section edge, combined to sequences
		/// </summary>
		internal List<LinkedLoop<int>> GetEdgeLoops()
		{
			LinkedList<LinkedLoop<int>> edgeLoops = GetEdgeLoopsByIndex();

			EdgeLoops_JoinBySameValue(edgeLoops);

			// prepair for result
			var result = new List<LinkedLoop<int>>(edgeLoops.Count);
			LinkedListNode<LinkedLoop<int>> parrentListsNode = edgeLoops.First;


			while (parrentListsNode != null)
			{
				var indices = parrentListsNode.Value;

				// delete same positions
				Vector3 prevValue = _meshData.Vertices[parrentListsNode.Value.last.value];
				var indexNode = indices.first;
				int counter = indices.size;
				while (counter != 0)
				{
					--counter;
					var current = indexNode;
					indexNode = indexNode.next;

					var v = _meshData.Vertices[current.value];
					if (prevValue == v)
						current.Remove();

					prevValue = v;
				}

				if (indices.size > 2)
					result.Add(indices);

				parrentListsNode = parrentListsNode.Next;
			}

			return result;
		}

		internal void EdgeLoops_JoinBySameValue(LinkedList<LinkedLoop<int>> edgeLoops)
		{
			// join by the most close points
			var parrentListsNode = edgeLoops.First;
			while (parrentListsNode != null)
			{
				var childList = parrentListsNode.Value;
				bool delete = false;

				var listFirstValue = _meshData.Vertices[childList.first.value];
				var listLastValue = _meshData.Vertices[childList.last.value];

				var rest = parrentListsNode.Next;
				while (rest != null)
				{
					var restFirstValue = _meshData.Vertices[rest.Value.first.value];
					var restLastValue = _meshData.Vertices[rest.Value.last.value];

					if (listLastValue == restFirstValue)
					{
						rest.Value.first.Remove();
						var newList = LinkedLoop.ConcatList(childList, rest.Value);
						edgeLoops.AddAfter(parrentListsNode, newList);
						delete = true;
						break;
					}
					if (listFirstValue == restLastValue)
					{
						childList.first.Remove();
						var newList = LinkedLoop.ConcatList(rest.Value, childList);
						edgeLoops.AddAfter(parrentListsNode, newList);
						delete = true;
						break;
					}
					if (listFirstValue == restFirstValue)
					{
						rest.Value.Reverse();
						childList.first.Remove();
						var newList = LinkedLoop.ConcatList(rest.Value, childList);
						edgeLoops.AddAfter(parrentListsNode, newList);
						delete = true;
						break;
					}
					if (listLastValue == restLastValue)
					{
						rest.Value.last.Remove();
						rest.Value.Reverse();
						var newList = LinkedLoop.ConcatList(childList, rest.Value);
						edgeLoops.AddAfter(parrentListsNode, newList);
						delete = true;
						break;
					}

					rest = rest.Next;
				}

				var tmp = parrentListsNode;
				parrentListsNode = parrentListsNode.Next;

				if (delete)
				{
					edgeLoops.Remove(tmp);
					edgeLoops.Remove(rest);
				}
			}
		}

		public LinkedList<LinkedLoop<int>> GetEdgeLoopsByIndex()
		{
			var lists = new LinkedList<LinkedLoop<int>>();

			// join by common indexes
			var capEdgesLoop = new LinkedLoop<IndexVector>(CapEdges);
			while (capEdgesLoop.size != 0)
			{
				var newList = new LinkedLoop<int>();
				lists.AddLast(newList);

				IndexVector edge = capEdgesLoop.first.value;
				capEdgesLoop.first.Remove();

				newList.AddLast(edge.From);
				newList.AddLast(edge.To);

				var current = capEdgesLoop.first;

				int counter = capEdgesLoop.size;
				while (counter != 0)
				{
					--counter;
					var next = current.next;

					if (TryAttachEdgeToList(current.value, newList))
					{
						current.Remove();
						counter = capEdgesLoop.size;
					}

					current = next;
				}
			}

			return lists;
		}

		/// <summary>
		/// Add mesh to another mesh as submesh
		/// </summary>
		/// <param name="to"></param>
		/// <param name="add"></param>
		void AddMeshes(List<PolyMeshData> meshes)
		{
			List<int[]> triangleList = new List<int[]>(meshes.Count);

			for (int m = 0; m < meshes.Count; m++)
			{
				int triangleValueShift = _meshData.Vertices.Count;

				var add = meshes[m];

				var addVertices = add.vertices;
				var addNormals = add.normals;
				var addBoneWeights = add.boneWeights;
				var addUVs = add.uv;

				for (int i = 0; i < addVertices.Length; i++)
				{
					var vertex = addVertices[i];
					var normal = addNormals[i];
					var uv = addUVs[i];
					BoneWeight? boneWeight = null;
					if (addBoneWeights.Length > 0)
						boneWeight = addBoneWeights[i];

					// todo: maybe we need to pass tangent
					AddVertex(vertex, normal, null, null, uv, null, null, null, null, boneWeight);
				}

				var addTriangles = add.triangles;
				for (int i = 0; i < addTriangles.Length; ++i)
					addTriangles[i] += triangleValueShift;

				triangleList.Add(addTriangles);
			}


			int totalTriangleSize = 0;
			for (int i = 0; i < triangleList.Count; i++)
				totalTriangleSize += triangleList[i].Length;

			int[] totalTriangles = new int[totalTriangleSize];

			int shiftPos = 0;
			for (int i = 0; i < triangleList.Count; i++)
			{
				var triangles = triangleList[i];

				Array.Copy(triangles, 0, totalTriangles, shiftPos, triangles.Length);
				shiftPos += triangles.Length;
			}
			
			var newSubMeshes = new int[_meshData.SubMeshes.Length + 1][];
			Array.Copy(_meshData.SubMeshes, newSubMeshes, _meshData.SubMeshes.Length);
			newSubMeshes[newSubMeshes.Length - 1] = totalTriangles;
			_meshData.SubMeshes = newSubMeshes;
		}

		private void AddVertex(Vector3 vertex, Vector3? normal, Color? color, Color32? color32, Vector2? uv, Vector2? uv2, Vector2? uv3, Vector2? uv4, Vector4? tangent, BoneWeight? boneWeight)
		{
			_meshData.Vertices.Add(vertex);
			if (_meshData.NormalsExists)
				_meshData.Normals.Add(normal.GetValueOrDefault());

			if (_meshData.ColorsExists)
				_meshData.Colors.Add(color.GetValueOrDefault());
			if (_meshData.Colors32Exists)
				_meshData.Colors32.Add(color32.GetValueOrDefault());

			if (_meshData.UVExists)
				_meshData.UV.Add(uv.GetValueOrDefault());
			if (_meshData.UV2Exists)
				_meshData.UV2.Add(uv2.GetValueOrDefault());
			if (_meshData.UV3Exists)
				_meshData.UV3.Add(uv3.GetValueOrDefault());
			if (_meshData.UV4Exists)
				_meshData.UV4.Add(uv4.GetValueOrDefault());

			if (_meshData.TangentsExists)
				_meshData.Tangents.Add(tangent.GetValueOrDefault());

			if (_meshData.BoneWeightsExists)
				_meshData.BoneWeights.Add(boneWeight.GetValueOrDefault());
		}

		private void DeleteEmptySubmeshes()
		{
			List<int[]> tmpSubMeshes = new List<int[]>(_meshData.SubMeshes.Length);
			for (int i = 0; i < _meshData.SubMeshes.Length; i++)
			{
				var subMesh = _meshData.SubMeshes[i];
				if (subMesh.Length != 0)
				{
					tmpSubMeshes.Add(subMesh);
				}
			}

			if (tmpSubMeshes.Count == _meshData.SubMeshes.Length)
				return;

			if (_meshData.Materials != null)
			{
				List<Material> tmpMaterials = new List<Material>(_meshData.Materials.Length);
				for (int i = 0; i < _meshData.SubMeshes.Length; i++)
				{
					var subMesh = _meshData.SubMeshes[i];
					if (subMesh.Length != 0)
					{
						tmpMaterials.Add(_meshData.Materials[i]);
					}
				}

				_meshData.Materials = tmpMaterials.ToArray();
			}

			_meshData.SubMeshes = tmpSubMeshes.ToArray();
		}

		/// <summary>
		/// Get ratio from 'v1' to plane
		/// </summary>
		float CalcGetRatio(int i1, int i2)
		{
			Vector3 v1 = _adapter.GetWorldPos(i1);
			Vector3 v2 = _adapter.GetWorldPos(i2);

			var vOldOrigin = v1;
			var vOldDir = v2 - vOldOrigin;

			var ray = new Ray(vOldOrigin, vOldDir);

			float enter;
			_planeLocal.Raycast(ray, out enter);

#if DEBUG
			if (vOldDir.magnitude == 0f)
				Debug.LogError("CalcGetRatio: vOldDir.magnitude == 0f");
#endif

			var r = enter / vOldDir.magnitude;
			return r;
		}
	}

	/// <summary>
	/// Vector from 'From' to 'To'
	/// </summary>
	struct IndexVector : IEquatable<IndexVector>
	{
		public IndexVector(int from, int to)
		{
			From = from;
			To = to;
		}
		public readonly int From;
		public readonly int To;

		public bool Equals(IndexVector obj)
		{
			return From == obj.From & To == obj.To;
		}

		public override string ToString()
		{
			return "(" + From.ToString() + " -> " + To.ToString() + ")";
		}
	}
}
