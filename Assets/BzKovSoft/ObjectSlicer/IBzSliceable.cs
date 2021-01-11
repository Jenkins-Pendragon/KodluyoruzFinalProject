using System;
using UnityEngine;

namespace BzKovSoft.ObjectSlicer
{
	/// <summary>
	/// Sliceable object
	/// </summary>
	public interface IBzSliceable
	{
		/// <summary>
		/// Slice the object this script is attached to
		/// </summary>
		/// <param name="plane">Plane by which you are going to slice</param>
		bool Slice(Plane plane);

		/// <summary>
		/// Get objects from positive side of a slice plane
		/// </summary>
		GameObject[] OutObjectsPositive { get; }
		/// <summary>
		/// Get objects from negative side of a slice plane
		/// </summary>
		GameObject[] OutObjectsNegative { get; }
	}
}
