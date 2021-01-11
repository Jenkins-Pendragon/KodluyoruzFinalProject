using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using BzKovSoft.ObjectSlicer.EventHandlers;

namespace BzKovSoft.ObjectSlicer
{
	/// <summary>
	/// Simple sliceable object. Simple mesh with MeshRenderer and MeshFilter
	/// </summary>
	[Obsolete("This component is obsolete, use BzSliceableObjectBase implementations")]
	[DisallowMultipleComponent]
	public class BzSliceableSimple : MonoBehaviour, IBzSliceable
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
			MeshFilter meshFilter = GetComponent<MeshFilter>();
			var mesh = meshFilter.sharedMesh;

			if (SectionViewMaterial == null)
				throw new InvalidOperationException("SectionViewMaterial == null");

			if (!CheckBounds(plane))        // AABB check for optimisation
				return false;

			var renderer = GetComponent<MeshRenderer>();
			IBzSliceAddapter adapter = new BzSliceMeshFilterAddapter(mesh.vertices, gameObject);
			BzMeshDataDissector meshDissector = new BzMeshDataDissector(mesh, plane, renderer.sharedMaterials, adapter, null);
			meshDissector.DefaultSliceMaterial = SectionViewMaterial;

			if (meshDissector.Slice() != SliceResult.Sliced)
				return false;

			// duplicate object
			var duplicate = Instantiate(this.gameObject, this.gameObject.transform.parent);
			var duplRenderer = duplicate.GetComponent<MeshRenderer>();

			meshDissector.RebuildNegMesh(renderer);
			meshDissector.RebuildPosMesh(duplRenderer);

			InvokeEvents(this.gameObject, duplicate);

			OutObjectsNegative = new GameObject[] { this.gameObject };
			OutObjectsPositive = new GameObject[] { duplicate };

			return true;
		}

		private void InvokeEvents(GameObject original, GameObject duplicate)
		{
			var events = original.GetComponents<IBzObjectSlicedEvent>();
			for (int i = 0; i < events.Length; i++)
				events[i].ObjectSliced(original, original, duplicate);
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
	}
}