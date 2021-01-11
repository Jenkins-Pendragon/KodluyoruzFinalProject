using BzKovSoft.ObjectSlicer;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BzKovSoft.CharacterSlicer
{
	class BzSliceSkinnedMeshAddapter : IBzSliceAddapter
	{
		private Vector3 _position;
		private Vector3[] _vertices;

		readonly Matrix4x4[] _charToW;
		readonly BoneWeight[] _boneWeights;

		public BzSliceSkinnedMeshAddapter(SkinnedMeshRenderer renderer)
		{
			_position = renderer.gameObject.transform.position;
			var mesh = renderer.sharedMesh;
			_vertices = mesh.vertices;

			var bones = renderer.bones;
			var bindposes = mesh.bindposes;
			_charToW = new Matrix4x4[bones.Length];
			for (int i = 0; i < bones.Length; i++)
			{
				var tr = bones[i];
				if (tr == null)
					continue;

				_charToW[i] = tr.localToWorldMatrix * bindposes[i];
			}

			_boneWeights = mesh.boneWeights;
		}

		public Vector3 GetWorldPos(int index)
		{
			Vector3 position = _vertices[index];
			var boneWeight = _boneWeights[index];

			
			Vector3 newPosition = Vector3.zero;
			if (boneWeight.weight0 > 0f)
				newPosition += _charToW[boneWeight.boneIndex0].MultiplyPoint3x4(position) * boneWeight.weight0;
			if (boneWeight.weight1 > 0f)
				newPosition += _charToW[boneWeight.boneIndex1].MultiplyPoint3x4(position) * boneWeight.weight1;
			if (boneWeight.weight2 > 0f)
				newPosition += _charToW[boneWeight.boneIndex2].MultiplyPoint3x4(position) * boneWeight.weight2;
			if (boneWeight.weight3 > 0f)
				newPosition += _charToW[boneWeight.boneIndex3].MultiplyPoint3x4(position) * boneWeight.weight3;

			return newPosition;
		}

		public Vector3 GetWorldPos(BzMeshData meshData, int index)
		{
			Vector3 position = meshData.Vertices[index];
			var boneWeight = meshData.BoneWeights[index];

			Vector3 newPosition = Vector3.zero;
			if (boneWeight.weight0 > 0f)
				newPosition += _charToW[boneWeight.boneIndex0].MultiplyPoint3x4(position) * boneWeight.weight0;
			if (boneWeight.weight1 > 0f)
				newPosition += _charToW[boneWeight.boneIndex1].MultiplyPoint3x4(position) * boneWeight.weight1;
			if (boneWeight.weight2 > 0f)
				newPosition += _charToW[boneWeight.boneIndex2].MultiplyPoint3x4(position) * boneWeight.weight2;
			if (boneWeight.weight3 > 0f)
				newPosition += _charToW[boneWeight.boneIndex3].MultiplyPoint3x4(position) * boneWeight.weight3;

			return newPosition;
		}

		public bool Check(BzMeshData meshData)
		{
			return true;
		}

		public void RebuildMesh(Mesh mesh, Material[] materials, Renderer meshRenderer)
		{
			((SkinnedMeshRenderer)meshRenderer).sharedMesh = mesh;
			meshRenderer.sharedMaterials = materials;
		}

		public Vector3 GetObjectCenterInWorldSpace()
		{
			return _position;
		}
	}
}