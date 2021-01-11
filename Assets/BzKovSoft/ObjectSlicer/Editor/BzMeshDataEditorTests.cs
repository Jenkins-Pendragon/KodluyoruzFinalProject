using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using System.Linq;

namespace BzKovSoft.ObjectSlicer.ObjectSlicer
{
	public class BzMeshDataEditorTests
	{
		[Test]
		public void GetEdgeLoopsByIndex()
		{
			//Arrange
			BzMeshDataEditor editor = new BzMeshDataEditor(null, new Plane(), null);
			editor.CapEdges.Add(new IndexVector(1, 5));
			editor.CapEdges.Add(new IndexVector(9, 4));
			editor.CapEdges.Add(new IndexVector(7, 8));
			editor.CapEdges.Add(new IndexVector(2, 4));
			editor.CapEdges.Add(new IndexVector(5, 3));
			editor.CapEdges.Add(new IndexVector(9, 10));
			editor.CapEdges.Add(new IndexVector(6, 3));
			editor.CapEdges.Add(new IndexVector(6, 7));

			//Act
			var loops = editor.GetEdgeLoopsByIndex();

			//Assert
			Assert.AreEqual(2, loops.Count);

			var loop1 = loops.Single(loop => loop.size == 6);
			if (loop1.first.value == 8)
				loop1.Reverse();

			var loop2 = loops.Single(loop => loop.size == 4);
			if (loop2.first.value == 10)
				loop2.Reverse();

			Assert.IsTrue(Enumerable.SequenceEqual(new[] { 1, 5, 3, 6, 7, 8 }, LoopToArray(loop1)));
			Assert.IsTrue(Enumerable.SequenceEqual(new[] { 2, 4, 9, 10 }, LoopToArray(loop2)));
		}

		[Test]
		public void JoinBySameValue()
		{
			//Arrange
			var vertices = new Vector3[]
			{
			new Vector3(0, 0, 0),
			new Vector3(1, 0, 0),
			new Vector3(1, 0, 0),
			new Vector3(3, 0, 0),

			new Vector3(4, 0, 0),
			new Vector3(5, 0, 0),
			new Vector3(5, 0, 0),
			new Vector3(7, 0, 0),
			};

			var mesh = new Mesh();
			mesh.vertices = vertices;

			var meshData = new BzMeshData(mesh, null);
			var adapter = new BzMockAddapter(vertices);
			BzMeshDataEditor editor = new BzMeshDataEditor(meshData, new Plane(), adapter);

			editor.CapEdges.Add(new IndexVector(2, 3));
			editor.CapEdges.Add(new IndexVector(0, 1));

			editor.CapEdges.Add(new IndexVector(4, 5));
			editor.CapEdges.Add(new IndexVector(6, 5));
			editor.CapEdges.Add(new IndexVector(6, 7));

			//Act
			var loops = editor.GetEdgeLoopsByIndex();
			Assert.AreEqual(3, loops.Count);
			editor.EdgeLoops_JoinBySameValue(loops);

			//Assert
			Assert.AreEqual(2, loops.Count);

			var loop1 = loops.Single(loop => loop.first.value == 0 | loop.last.value == 0);
			if (loop1.first.value != 0)
				loop1.Reverse();

			var loop2 = loops.Single(loop => loop.first.value == 4 | loop.last.value == 4);
			if (loop2.first.value != 4)
				loop2.Reverse();

			Assert.IsTrue(Enumerable.SequenceEqual(new[] { 0, 1, 3 }, LoopToArray(loop1)));
			Assert.IsTrue(Enumerable.SequenceEqual(new[] { 4, 5, 6, 7 }, LoopToArray(loop2)));
		}

		[Test]
		public void GetEdgeLoops()
		{
			//Arrange
			var vertices = new Vector3[]
			{
			new Vector3(0, 0, 0),
			new Vector3(1, 0, 0),
			new Vector3(1, 0, 0),
			new Vector3(3, 0, 0),

			new Vector3(4, 0, 0),
			new Vector3(5, 0, 0),
			new Vector3(5, 0, 0),
			new Vector3(7, 0, 0),
			};

			var mesh = new Mesh();
			mesh.vertices = vertices;

			var meshData = new BzMeshData(mesh, null);
			var adapter = new BzMockAddapter(vertices);
			BzMeshDataEditor editor = new BzMeshDataEditor(meshData, new Plane(), adapter);

			editor.CapEdges.Add(new IndexVector(2, 3));
			editor.CapEdges.Add(new IndexVector(0, 1));

			editor.CapEdges.Add(new IndexVector(4, 5));
			editor.CapEdges.Add(new IndexVector(6, 5));
			editor.CapEdges.Add(new IndexVector(6, 7));

			//Act
			var loops = editor.GetEdgeLoops();

			//Assert
			Assert.AreEqual(2, loops.Count);

			var loop1 = loops.Single(loop => loop.first.value == 0 | loop.last.value == 0);
			if (loop1.first.value != 0)
				loop1.Reverse();

			var loop2 = loops.Single(loop => loop.first.value == 4 | loop.last.value == 4);
			if (loop2.first.value != 4)
				loop2.Reverse();

			Assert.IsTrue(Enumerable.SequenceEqual(new[] { 0, 1, 3 }, LoopToArray(loop1)));
			Assert.IsTrue(Enumerable.SequenceEqual(new[] { 4, 5, 7 }, LoopToArray(loop2)));
		}

		T[] LoopToArray<T>(LinkedLoop<T> loop)
		{
			T[] t = new T[loop.size];

			var current = loop.first;
			for (int i = 0; i < loop.size; i++)
			{
				t[i] = current.value;
				current = current.next;
			}

			return t;
		}
	}
}