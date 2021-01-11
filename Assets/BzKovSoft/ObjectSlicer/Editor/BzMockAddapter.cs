using System;
using UnityEngine;

namespace BzKovSoft.ObjectSlicer.ObjectSlicer
{
	class BzMockAddapter : IBzSliceAddapter
	{
		private Vector3[] _vertices;

		public BzMockAddapter(Vector3[] vertices)
		{
			_vertices = vertices;
		}

		public Vector3 GetWorldPos(int index)
		{
			Vector3 position = _vertices[index];

			return position;
		}

		public Vector3 GetWorldPos(BzMeshData meshData, int index)
		{
			return meshData.Vertices[index];
		}

		public bool Check(BzMeshData meshData)
		{
			int trCount = 0;
			for (int i = 0; i < meshData.SubMeshes.Length; i++)
			{
				trCount += meshData.SubMeshes[i].Length;
			}

			if (trCount < 3)
				throw new Exception("FFFFF3");

			if (trCount % 3 != 0)
				throw new Exception("FFFFF4");

			return true;
		}

		public void RebuildMesh(Mesh mesh, Material[] materials, Renderer meshRenderer)
		{
			throw new NotSupportedException();
		}

		public Vector3 GetObjectCenterInWorldSpace()
		{
			throw new NotImplementedException();
		}
	}
}