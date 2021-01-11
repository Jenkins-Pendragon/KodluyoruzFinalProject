using UnityEngine;
using UnityEditor;
using NUnit.Framework;

namespace BzKovSoft.ObjectSlicer.ObjectSlicer
{
	public class LinkedLoopTests
	{
		[Test]
		public void AddItem()
		{
			//Arrange
			LinkedLoop<int> list = new LinkedLoop<int>();

			//Act
			list.AddLast(1);
			list.AddLast(2);
			list.AddLast(3);

			//Assert
			Assert.AreEqual(3, list.size);

			var item1 = list.first;
			var item2 = item1.next;
			var item3 = item2.next;

			Assert.AreEqual(1, item1.value);
			Assert.AreEqual(2, item2.value);
			Assert.AreEqual(3, item3.value);

			Assert.AreEqual(item1.next, item2);
			Assert.AreEqual(item1.previous, item3);

			Assert.AreEqual(item2.next, item3);
			Assert.AreEqual(item2.previous, item1);

			Assert.AreEqual(item3.next, item1);
			Assert.AreEqual(item3.previous, item2);
		}

		[Test]
		public void RemoveAndZeroingSize()
		{
			//Arrange
			LinkedLoop<int> list = new LinkedLoop<int>();

			//Act
			list.AddLast(1);

			var item1 = list.first;
			item1.Remove();

			//Assert
			Assert.AreEqual(0, list.size);
			Assert.IsNull(list.first);
			Assert.IsNull(list.last);

			Assert.IsNull(item1.next);
			Assert.IsNull(item1.previous);
		}

		[Test]
		public void RemoveInMiddle()
		{
			//Arrange
			LinkedLoop<int> list = new LinkedLoop<int>();

			//Act
			list.AddLast(1);
			list.AddLast(2);
			list.AddLast(3);
			list.AddLast(4);

			var item1 = list.first;
			var item2 = item1.next;
			var item3 = item2.next;
			var item4 = item3.next;
			item2.Remove();

			//Assert
			Assert.AreEqual(3, list.size);

			Assert.AreEqual(1, item1.value);
			Assert.AreEqual(3, item3.value);
			Assert.AreEqual(4, item4.value);

			Assert.AreEqual(item1.next, item3);
			Assert.AreEqual(item1.previous, item4);

			Assert.AreEqual(item3.next, item4);
			Assert.AreEqual(item3.previous, item1);

			Assert.AreEqual(item4.next, item1);
			Assert.AreEqual(item4.previous, item3);
		}

		[Test]
		public void RemoveFirst()
		{
			//Arrange
			LinkedLoop<int> list = new LinkedLoop<int>();

			//Act
			list.AddLast(1);
			list.AddLast(2);
			list.AddLast(3);
			list.AddLast(4);

			var item1 = list.first;
			var item2 = item1.next;
			var item3 = item2.next;
			var item4 = item3.next;
			item1.Remove();

			//Assert
			Assert.AreEqual(3, list.size);

			Assert.AreEqual(2, item2.value);
			Assert.AreEqual(3, item3.value);
			Assert.AreEqual(4, item4.value);

			Assert.AreEqual(item2.next, item3);
			Assert.AreEqual(item2.previous, item4);

			Assert.AreEqual(item3.next, item4);
			Assert.AreEqual(item3.previous, item2);

			Assert.AreEqual(item4.next, item2);
			Assert.AreEqual(item4.previous, item3);
		}

		[Test]
		public void RemoveLast()
		{
			//Arrange
			LinkedLoop<int> list = new LinkedLoop<int>();

			//Act
			list.AddLast(1);
			list.AddLast(2);
			list.AddLast(3);
			list.AddLast(4);

			var item1 = list.first;
			var item2 = item1.next;
			var item3 = item2.next;
			var item4 = item3.next;
			item4.Remove();

			//Assert
			Assert.AreEqual(3, list.size);

			Assert.AreEqual(1, item1.value);
			Assert.AreEqual(2, item2.value);
			Assert.AreEqual(3, item3.value);

			Assert.AreEqual(item1.next, item2);
			Assert.AreEqual(item1.previous, item3);

			Assert.AreEqual(item2.next, item3);
			Assert.AreEqual(item2.previous, item1);

			Assert.AreEqual(item3.next, item1);
			Assert.AreEqual(item3.previous, item2);
		}

		[Test]
		public void RemoveConcat()
		{
			//Arrange
			LinkedLoop<int> list1 = new LinkedLoop<int>();
			LinkedLoop<int> list2 = new LinkedLoop<int>();

			//Act
			list1.AddLast(1);
			list1.AddLast(2);
			list2.AddLast(3);
			list2.AddLast(4);

			LinkedLoop<int> listS = LinkedLoop.ConcatList(list1, list2);

			//Assert
			var item1 = listS.first;
			var item2 = item1.next;
			var item3 = item2.next;
			var item4 = item3.next;

			Assert.AreEqual(4, listS.size);

			Assert.AreEqual(1, item1.value);
			Assert.AreEqual(2, item2.value);
			Assert.AreEqual(3, item3.value);
			Assert.AreEqual(4, item4.value);

			Assert.AreEqual(item1.next, item2);
			Assert.AreEqual(item1.previous, item4);

			Assert.AreEqual(item2.next, item3);
			Assert.AreEqual(item2.previous, item1);

			Assert.AreEqual(item3.next, item4);
			Assert.AreEqual(item3.previous, item2);

			Assert.AreEqual(item4.next, item1);
			Assert.AreEqual(item4.previous, item3);
		}

		[Test]
		public void RemoveReverse4()
		{
			//Arrange
			LinkedLoop<int> list = new LinkedLoop<int>();

			//Act
			list.AddLast(1);
			list.AddLast(2);
			list.AddLast(3);
			list.AddLast(4);
			list.Reverse();

			//Assert
			var item1 = list.first;
			var item2 = item1.next;
			var item3 = item2.next;
			var item4 = item3.next;

			Assert.AreEqual(4, list.size);

			Assert.AreEqual(4, item1.value);
			Assert.AreEqual(3, item2.value);
			Assert.AreEqual(2, item3.value);
			Assert.AreEqual(1, item4.value);

			Assert.AreEqual(item1.next, item2);
			Assert.AreEqual(item1.previous, item4);

			Assert.AreEqual(item2.next, item3);
			Assert.AreEqual(item2.previous, item1);

			Assert.AreEqual(item3.next, item4);
			Assert.AreEqual(item3.previous, item2);

			Assert.AreEqual(item4.next, item1);
			Assert.AreEqual(item4.previous, item3);
		}

		[Test]
		public void RemoveReverse3()
		{
			//Arrange
			LinkedLoop<int> list = new LinkedLoop<int>();

			//Act
			list.AddLast(1);
			list.AddLast(2);
			list.AddLast(3);
			list.Reverse();

			//Assert
			var item1 = list.first;
			var item2 = item1.next;
			var item3 = item2.next;

			Assert.AreEqual(3, list.size);

			Assert.AreEqual(3, item1.value);
			Assert.AreEqual(2, item2.value);
			Assert.AreEqual(1, item3.value);

			Assert.AreEqual(item1.next, item2);
			Assert.AreEqual(item1.previous, item3);

			Assert.AreEqual(item2.next, item3);
			Assert.AreEqual(item2.previous, item1);

			Assert.AreEqual(item3.next, item1);
			Assert.AreEqual(item3.previous, item2);
		}
	}
}