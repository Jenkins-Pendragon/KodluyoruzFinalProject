using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BzKovSoft.ObjectSlicer
{
	/// <summary>
	/// One triange from a mesh
	/// </summary>
	struct BzTriangle
	{
		public readonly int i1;
		public readonly int i2;
		public readonly int i3;

		/// <summary>
		/// Create triangle from 3 indexes of a mesh
		/// </summary>
		public BzTriangle(int i1, int i2, int i3)
		{
			this.i1 = i1;
			this.i2 = i2;
			this.i3 = i3;
		}

		/// <summary>
		/// Cut this trianle by plane (plane from MeshPeparator)
		/// </summary>
		/// <param name="tr">Out triangle 1 (always exists)</param>
		/// <param name="trNegExtra">Out triangle 2 (can be null)</param>
		public void DivideByPlane(BzMeshDataEditor meshDataEditorNeg, BzMeshDataEditor meshDataEditorPos,
			List<BzTriangle> trianglesNegSliced, List<BzTriangle> trianglesPosSliced,
			bool _side1, bool _side2, bool _side3)
		{

			if (!_side1 & _side2 & _side3)
			{
				CalculateOneTr(meshDataEditorNeg, i1, i2, i3, trianglesNegSliced);
				CalculateTwoTr(meshDataEditorPos, i1, i2, i3, trianglesPosSliced);
			}
			else if (_side1 & !_side2 & _side3)
			{
				CalculateOneTr(meshDataEditorNeg, i2, i3, i1, trianglesNegSliced);
				CalculateTwoTr(meshDataEditorPos, i2, i3, i1, trianglesPosSliced);
			}
			else if (_side1 & _side2 & !_side3)
			{
				CalculateOneTr(meshDataEditorNeg, i3, i1, i2, trianglesNegSliced);
				CalculateTwoTr(meshDataEditorPos, i3, i1, i2, trianglesPosSliced);
			}
			

			else if (_side1 & !_side2 & !_side3)
			{
				CalculateTwoTr(meshDataEditorNeg, i1, i2, i3, trianglesNegSliced);
				CalculateOneTr(meshDataEditorPos, i1, i2, i3, trianglesPosSliced);
			}
			else if (!_side1 & _side2 & !_side3)
			{
				CalculateTwoTr(meshDataEditorNeg, i2, i3, i1, trianglesNegSliced);
				CalculateOneTr(meshDataEditorPos, i2, i3, i1, trianglesPosSliced);
			}
			else if (!_side1 & !_side2 & _side3)
			{
				CalculateTwoTr(meshDataEditorNeg, i3, i1, i2, trianglesNegSliced);
				CalculateOneTr(meshDataEditorPos, i3, i1, i2, trianglesPosSliced);
			}
			else
				throw new InvalidOperationException();
		}

		/// <summary>
		/// Calculate when two point right. The result is one triangle
		/// </summary>
		static void CalculateOneTr(BzMeshDataEditor meshDataEditor, int neg, int rightA, int rightB, List<BzTriangle> trianglesSliced)
		{
			//            pos
			//    rA\^^^^^^^^^^^^^/rB
			//       \           /
			//        \         /
			//      ---------------
			//    new1  \     /  new2
			//           \   /
			//            \ /
			//            neg

			int new1 = meshDataEditor.GetIndexFor(neg, rightA);
			int new2 = meshDataEditor.GetIndexFor(neg, rightB);

			var tr = new BzTriangle(neg, new1, new2);
			trianglesSliced.Add(tr);

			// save eghe to chash
			meshDataEditor.CapEdges.Add(new IndexVector(new1, new2));
		}

		/// <summary>
		/// Calculate when one point right. The result is two triangles
		/// </summary>
		static void CalculateTwoTr(BzMeshDataEditor meshDataEditor, int pos, int negA, int negB, List<BzTriangle> trianglesSliced)
		{
			//            pos
			//            / \
			//           /   \
			//    new1  /     \  new2
			//      ----.----------
			//        /   .     \
			//       /       .   \
			//    iB/___________._\iA
			//           neg

			int new1 = meshDataEditor.GetIndexFor(negB, pos);
			int new2 = meshDataEditor.GetIndexFor(negA, pos);

			var tr1 = new BzTriangle(negB, new1, negA);
			var tr2 = new BzTriangle(negA, new1, new2);
			trianglesSliced.Add(tr1);
			trianglesSliced.Add(tr2);

			// save eghe to chash
			meshDataEditor.CapEdges.Add(new IndexVector(new1, new2));
		}
	}
}
