using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using BzKovSoft.ObjectSlicer.MeshGenerator;
using BzKovSoft.ObjectSlicer.EventHandlers;
using System.Linq;

namespace BzKovSoft.ObjectSlicer
{
	/// <summary>
	/// Simple sliceable object. Simple mesh with MeshRenderer and MeshFilter
	/// </summary>
	[Obsolete("This component is obsolete, use BzSliceableObjectBase implementations")]
	[DisallowMultipleComponent]
	public class BzSliceableCollider : MonoBehaviour, IBzSliceable
	{
		/// <summary>
		/// Material that will be applied after slicing
		/// </summary>
#pragma warning disable 0649
		[SerializeField]
		private Material SectionViewMaterial;
#pragma warning restore 0649
		public GameObject[] OutObjectsPositive { get; private set; }
		public GameObject[] OutObjectsNegative { get; private set; }

		public bool Slice(Plane plane)
		{
			if (SectionViewMaterial == null)
				throw new InvalidOperationException("SectionViewMaterial == null");

			if (!CheckBounds(plane))        // AABB check for optimisation
				return false;

			var colliders = SliceColliders(plane);

			if (colliders == null)
				return false;

			var renderer = GetComponent<MeshRenderer>();
			MeshFilter meshFilter = GetComponent<MeshFilter>();
			var mesh = meshFilter.sharedMesh;

			IBzSliceAddapter adapter = new BzSliceMeshFilterAddapter(mesh.vertices, gameObject);
			BzMeshDataDissector meshDissector = new BzMeshDataDissector(mesh, plane, renderer.sharedMaterials, adapter, null);
			meshDissector.DefaultSliceMaterial = SectionViewMaterial;

			if (meshDissector.Slice() != SliceResult.Sliced)
				return false;

			// duplicate object
			var duplicate = Instantiate(this.gameObject, this.gameObject.transform.parent);
			
			ReplaceColliders(colliders, duplicate);

			var duplRenderer = duplicate.GetComponent<MeshRenderer>();

			meshDissector.RebuildNegMesh(renderer);
			meshDissector.RebuildPosMesh(duplRenderer);

			InvokeEvents(this.gameObject, duplicate);

			OutObjectsNegative = new GameObject[] { this.gameObject };
			OutObjectsPositive = new GameObject[] { duplicate };

			return true;
		}

		private void ReplaceColliders(ColliderSliceResult[] colliders, GameObject duplicate)
		{
			for (int i = 0; i < colliders.Length; i++)
			{
				var collider = colliders[i];

				var componentNeg = collider.OriginalCollider;
				var componentPos = BzSlicerHelper.GetSameComponentForDuplicate(componentNeg, this.gameObject, duplicate);
				var goNeg = collider.OriginalCollider.gameObject;
				var goPos = componentPos.gameObject;

				if (collider.SliceResult == SliceResult.Sliced)
				{
					AddCollider(collider.MeshDissector.SliceResultNeg.GenerateMesh(), collider.OriginalCollider.material, goNeg);
					AddCollider(collider.MeshDissector.SliceResultPos.GenerateMesh(), collider.OriginalCollider.material, goPos);
					Destroy(componentNeg);
					Destroy(componentPos);
					continue;
				}

				if (collider.SliceResult == SliceResult.Neg)
				{
					Destroy(componentPos);
					continue;
				}

				if (collider.SliceResult == SliceResult.Pos)
				{
					Destroy(componentNeg);
					continue;
				}

				throw new InvalidOperationException();
			}
		}

		private static void AddCollider(Mesh mesh, PhysicMaterial material, GameObject go)
		{
			var collider = go.AddComponent<MeshCollider>();
			collider.sharedMaterial = material;
			collider.sharedMesh = mesh;
			collider.convex = true;
		}

		private void InvokeEvents(GameObject original, GameObject extra)
		{
			var events = original.GetComponents<IBzObjectSlicedEvent>();
			for (int i = 0; i < events.Length; i++)
				events[i].ObjectSliced(original, original, extra);
		}

		ColliderSliceResult[] SliceColliders(Plane plane)
		{
			var colliders = GetComponentsInChildren<Collider>();
			ColliderSliceResult[] results = new ColliderSliceResult[colliders.Length];
			bool ColliderExistsNeg = false;
			bool ColliderExistsPos = false;

			for (int i = 0; i < colliders.Length; i++)
			{
				var collider = colliders[i];

				var colliderB = collider as BoxCollider;
				var colliderS = collider as SphereCollider;
				var colliderC = collider as CapsuleCollider;
				var colliderM = collider as MeshCollider;

				ColliderSliceResult result;
				if (colliderB != null)
				{
					var mesh = Cube.Create(colliderB.size, colliderB.center);
					result = TrySliceCollider(colliderB.center, colliderB, mesh, plane);
				}
				else if (colliderS != null)
				{
					var mesh = IcoSphere.Create(colliderS.radius, colliderS.center);
					result = TrySliceCollider(colliderS.center, colliderS, mesh, plane);
				}
				else if (colliderC != null)
				{
					var mesh = Capsule.Create(colliderC.radius, colliderC.height, colliderC.direction, colliderC.center);
					result = TrySliceCollider(colliderC.center, colliderC, mesh, plane);
				}
				else if (colliderM != null)
				{
					Mesh mesh = Instantiate(colliderM.sharedMesh);
					result = TrySliceCollider(Vector3.zero, colliderM, mesh, plane);
				}
				else
					throw new NotSupportedException("Not supported collider type '" + collider.GetType().Name + "'");

				ColliderExistsNeg |= result.SliceResult == SliceResult.Sliced | result.SliceResult == SliceResult.Neg;
				ColliderExistsPos |= result.SliceResult == SliceResult.Sliced | result.SliceResult == SliceResult.Pos;
				results[i] = result;
			}

			bool sliced = ColliderExistsNeg & ColliderExistsPos;
			return sliced ? results : null;
		}

		private ColliderSliceResult TrySliceCollider(Vector3 locPos, Collider collider, Mesh mesh, Plane plane)
		{
			IBzSliceAddapter adapter = new BzSliceColliderAddapter(mesh.vertices, collider.gameObject);
			BzMeshDataDissector meshDissector = new BzMeshDataDissector(mesh, plane, null, adapter, null);

			ColliderSliceResult result = new ColliderSliceResult();
			result.OriginalCollider = collider;
			result.MeshDissector = meshDissector;
			result.SliceResult = meshDissector.Slice();

			return result;
		}

		/// <summary>
		/// Check if plane intersects AABB
		/// </summary>
		/// <param name="plane"></param>
		/// <returns></returns>
		private bool CheckBounds(Plane plane)
		{
			//    a1              b1
			//     /^^^^^^^^^^^^^/|
			//    /  |          / |
			// c1/           d1/  |
			//   --------------   |
			//   |   |        |   |
			//   |            |   |
			//   |   |        |   |
			//   |            |   |
			//   | a2 -  -  - | - /b2
			//   |  /         |  /
			//   |            | /
			// c2|/___________|/d2

			Mesh mesh = GetComponent<MeshFilter>().sharedMesh;
			var b1 = transform.TransformPoint(mesh.bounds.max);
			var c2 = transform.TransformPoint(mesh.bounds.min);
			var a2 = new Vector3(c2.x, c2.y, b1.z);
			var b2 = new Vector3(b1.x, c2.y, b1.z);
			var d2 = new Vector3(b1.x, c2.y, c2.z);
			var a1 = new Vector3(c2.x, b1.y, b1.z);
			var c1 = new Vector3(c2.x, b1.y, c2.z);
			var d1 = new Vector3(b1.x, b1.y, c2.z);

			bool p1 = plane.GetSide(a1);
			bool p2 = plane.GetSide(b1);
			bool p3 = plane.GetSide(c1);
			bool p4 = plane.GetSide(d1);
			bool p5 = plane.GetSide(a2);
			bool p6 = plane.GetSide(b2);
			bool p7 = plane.GetSide(c2);
			bool p8 = plane.GetSide(d2);

			bool havePositive = p1 | p2 | p3 | p4 | p5 | p6 | p7 | p8;
			bool haveNegative = !p1 | !p2 | !p3 | !p4 | !p5 | !p6 | !p7 | !p8;
			return havePositive & haveNegative;
		}

		struct ColliderSliceResult
		{
			public Collider OriginalCollider;
			public BzMeshDataDissector MeshDissector;
			public SliceResult SliceResult;
		}
	}
}