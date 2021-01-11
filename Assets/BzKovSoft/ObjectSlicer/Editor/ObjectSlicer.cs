using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using System;
using BzKovSoft.ObjectSlicerSamples;
using System.Linq;

namespace BzKovSoft.ObjectSlicer.Editor
{
	public class ObjectSlicer
	{
		[Test]
		public void SliceTest()
		{
			// arrange
			var go = GameObject.CreatePrimitive(PrimitiveType.Cube);

			var slicer = go.AddComponent<ObjectSlicerMock>();
			slicer.Asynchronously = false;
			slicer.DefaultSliceMaterial = new Material(Shader.Find("Standard"));

			BzSliceTryResult result = null;
			Action<BzSliceTryResult> action = (x) => { result = x; };

			// act
			slicer.Slice(new Plane(Vector3.up, Vector3.zero), 0, action);

			// assert
			if (!result.sliced)
				throw new InvalidOperationException("Not sliced");
			
			var mesh1 = result.outObjectNeg.GetComponent<MeshFilter>().sharedMesh;
			var mesh2 = result.outObjectPos.GetComponent<MeshFilter>().sharedMesh;
			Assert.AreEqual(4 * 6, mesh1.vertexCount);
			Assert.AreEqual(4 * 6, mesh2.vertexCount);
			Assert.AreEqual(6 * 2 * 3, mesh1.triangles.Length);
			Assert.AreEqual(6 * 2 * 3, mesh2.triangles.Length);

			var up = mesh1.vertices.Where(v => v.y == 0.5f).ToArray();
			var down = mesh1.vertices.Where(v => v.y == 0f).ToArray();
			Assert.AreEqual(12, up.Length);
			Assert.AreEqual(12, down.Length);

			var vertices = mesh1.vertices;
			var normals = mesh1.normals;

			var vv = vertices.Where((n, i) => normals[i] == Vector3.up).ToArray();

			Assert.AreEqual(4, vv.Length);
		}
	}
}