using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BzKovSoft.ObjectSlicer
{
	public class LinkedLoop<T>
	{
		public int size;
		public LoopNode<T> first;
		public LoopNode<T> last;

		public LinkedLoop()
		{
		}

		public LinkedLoop(List<T> list)
		{
			int count = list.Count;
			for (int i = 0; i < count; i++)
			{
				LoopNode<T> node = new LoopNode<T>(this, list[i]);
				AddLast(node);
			}
		}

		public void AddFirst(LoopNode<T> node)
		{
			size++;

			if (first == null)
			{
				first = node;
			}

			if (last == null)
			{
				last = node;
			}

			node.previous = last;
			node.next = first;

			first.previous = node;
			last.next = node;
			first = node;
		}

		public void AddLast(LoopNode<T> node)
		{
			size++;

			if (first == null)
			{
				first = node;
			}

			if (last == null)
			{
				last = node;
			}

			node.previous = last;
			node.next = first;

			first.previous = node;
			last.next = node;
			last = node;
		}

		public void AddFirst(T value)
		{
			var node = new LoopNode<T>(this, value);
			AddFirst(node);
		}

		public void AddLast(T value)
		{
			var node = new LoopNode<T>(this, value);
			AddLast(node);
		}

		public override string ToString()
		{
			return "Size: " + size.ToString();
		}

		/// <summary>
		/// Reverse items order in collection
		/// </summary>
		public void Reverse()
		{
			if (size <= 1)
				return;

			var current = first;
			do
			{
				var curNext = current.next;

				var tmp = current.next;
				current.next = current.previous;
				current.previous = tmp;

				current = curNext;
			}
			while (current != first);

			var tmpFrist = first;
			first = last;
			last = tmpFrist;
		}
	}

	public static class LinkedLoop
	{
		/// <summary>
		/// Concat items of two collections
		/// </summary>
		public static LinkedLoop<T> ConcatList<T>(LinkedLoop<T> listA, LinkedLoop<T> listB)
		{
			if (listA.size == 0)
				return listB;

			if (listB.size == 0)
				return listA;

			var result = new LinkedLoop<T>();
			result.size = listA.size + listB.size;

			result.first = listA.first;
			result.last = listB.last;

			result.first.previous = result.last;
			result.last.next = result.first;

			listA.last.next = listB.first;
			listB.first.previous = listA.last;

			// change node owners
			var current = result.first;
			do
			{
				current.list = result;
				current = current.next;
			}
			while (current != result.first);

			// empty list A and B
			listA.first = null;
			listA.last = null;
			listA.size = 0;

			listB.first = null;
			listB.last = null;
			listB.size = 0;

			return result;
		}
	}

	public class LoopNode<T>
	{
		internal LinkedLoop<T> list;
		public T value;
		public LoopNode<T> previous;
		public LoopNode<T> next;
		
		public LoopNode(LinkedLoop<T> list, T value)
		{
			this.list = list;
			this.value = value;
		}
		
		public void Remove()
		{
			--list.size;
			
			if (list.size == 0)
			{
				list.first = null;
				list.last = null;
				list = null;
				next = null;
				previous = null;
			}
			else
			{
				if (list.first == this)
					list.first = next;

				if (list.last == this)
					list.last = previous;

				next.previous = previous;
				previous.next = next;
			}
		}

		public override string ToString()
		{
			return "Node: " + value.ToString();
		}
	}
}
