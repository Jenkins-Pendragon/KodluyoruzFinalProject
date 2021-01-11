using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using System.Linq;

namespace BzKovSoft.CharacterSlicer.Editor
{
	public class ChCM_RemoveComponentsTests
	{
		//[Test]
		//public void DeleteColliders()
		//{
		//	// arrange
		//	var go = new GameObject("go");

		//	var go1 = new GameObject("go1");
		//	var c1 = go1.AddComponent<BoxCollider>();
		//	go1.transform.parent = go.transform;

		//	var go2 = new GameObject("go2");
		//	var c2 = go2.AddComponent<BoxCollider>();
		//	go2.transform.parent = go.transform;

		//	var rr1 = new CharacterComponentManager.ColliderSliceResult();
		//	rr1.OriginalCollider = c1;
		//	rr1.SliceResult = ObjectSlicer.SliceResult.Pos;

		//	var rr2 = new CharacterComponentManager.ColliderSliceResult();
		//	rr2.OriginalCollider = c2;
		//	rr2.SliceResult = ObjectSlicer.SliceResult.Neg;

		//	// act
		//	GameObject go_Clone = UnityEngine.Object.Instantiate(go);
		//	CharacterComponentManager.RemoveColliders(new CharacterComponentManager.ColliderSliceResult[] { rr1, rr2 }, go, go_Clone);

		//	// assert
		//	Assert.IsTrue(c1 == null);
		//	Assert.IsTrue(c2 != null);
		//}
		


        [Test]
        public void DDDDDD()
        {
			// arrange
			//BzSliceSkinnedMeshAddapter o = new BzSliceSkinnedMeshAddapter();

		}

        static void AAA(Transform tr, Plane plane, bool prevSide)
        {
            bool currentSide = plane.GetSide(tr.position);

            if (prevSide != currentSide)
            {
                Transform trPrev = tr.parent;

                // todo: cross: trPrev -> tr
            }

            for (int i = 0; i < tr.childCount; i++)
            {
                Transform child = tr.GetChild(i);

                AAA(child, plane, currentSide);
            }
        }
    }
}