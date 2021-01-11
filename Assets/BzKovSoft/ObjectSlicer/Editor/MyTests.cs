using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using System;

namespace BzKovSoft.ObjectSlicer.ObjectSlicer
{
	public class MyTests
	{
		[Test]
		public void ColliderTest()
		{
			var go = new GameObject();
			var cldr = go.AddComponent<MeshCollider>();
			go.AddComponent<Rigidbody>();

			var mesh = new Mesh();
			mesh.vertices = new []
			{
				new Vector3(-1f, -1f, 0f),
				new Vector3(-1f,  1f, 0f),
				new Vector3( 1f,  1f, 0f),
				new Vector3( 1f, -1f, 0.000001f),
			};

			mesh.triangles = new []
			{
				0, 1, 2,
				0, 2, 3,

				0, 2, 1,
				0, 3, 2,
			};

			cldr.sharedMesh = mesh;
			cldr.inflateMesh = true;
			var convexResult = new ConvexSetResult();
			convexResult.SetConvex(cldr);

		}
	}
}