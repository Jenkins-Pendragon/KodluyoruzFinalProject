using System;
using UnityEngine;

namespace BzKovSoft.ObjectSlicer
{
	class ConvexSetResult
	{
		public bool Success { get; private set; }

		public void SetConvex(MeshCollider collider)
		{
			Success = true;

			Application.logMessageReceivedThreaded += MessageHandler;
			collider.convex = true;
			Application.logMessageReceivedThreaded -= MessageHandler;

			if (!Success)
				Debug.Log("Collider error was proporly handled!");
		}

		void MessageHandler(string condition, string stackTrace, LogType type)
		{
			Success = false;
		}
	}
}
