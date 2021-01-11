using System;
using UnityEngine;

namespace BzKovSoft.ObjectSlicer
{
	class BzSliceMeshFilterAddapter : IBzSliceAddapter
	{
		Matrix4x4 _ltw;
		Vector3[] _vertices;

		public BzSliceMeshFilterAddapter(Vector3[] vertices, GameObject gameObject)
		{
			_vertices = vertices;
			_ltw = gameObject.transform.localToWorldMatrix;
		}

		public Vector3 GetWorldPos(int index)
		{
			Vector3 position = _vertices[index];
			return _ltw.MultiplyPoint3x4(position);
		}

		public Vector3 GetWorldPos(BzMeshData meshData, int index)
		{
			return _ltw.MultiplyPoint3x4(meshData.Vertices[index]);
		}

		public bool Check(BzMeshData meshData)
		{
			return true;
		}

		public void RebuildMesh(Mesh mesh, Material[] materials, Renderer meshRenderer)
		{
			var meshFilter = meshRenderer.gameObject.GetComponent<MeshFilter>();
			meshFilter.mesh = mesh;
			meshRenderer.sharedMaterials = materials;
		}

		public Vector3 GetObjectCenterInWorldSpace()
		{
			return _ltw.MultiplyPoint3x4(Vector3.zero);
		}
	}
}