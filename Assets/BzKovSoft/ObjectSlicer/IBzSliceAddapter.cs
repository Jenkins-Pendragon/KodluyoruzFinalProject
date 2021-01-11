using UnityEngine;

namespace BzKovSoft.ObjectSlicer
{
	public interface IBzSliceAddapter
	{
		Vector3 GetWorldPos(int index);
		Vector3 GetWorldPos(BzMeshData meshData, int index);
		bool Check(BzMeshData meshData);
		void RebuildMesh(Mesh mesh, Material[] materials, Renderer meshRenderer);
		Vector3 GetObjectCenterInWorldSpace();
	}
}