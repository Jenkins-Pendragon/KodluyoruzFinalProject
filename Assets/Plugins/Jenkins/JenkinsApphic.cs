using UnityEngine;
using System;
using DG.Tweening;
using Jenkins.Apphic;
using System.Collections;
using System.Collections.Generic;

namespace Jenkins
{
    namespace Apphic
    {

        /// <summary>
        /// Developing by Jenkins'Onur'Pendragon. 
        /// Apphic Games Limited Hypercasual Template.
        /// </summary>
        public static partial class JenkinsApphic
        {
            /// <summary>
            /// Transform.Rotation değerleri ile oyna.
            /// Zaman vermezseniz anında değiştirir. void OnComplete Yollayabilirsiniz.
            /// </summary>
            public static void SetAngle(this Transform transform, float x, float y, float z, float time = 0)
            {
                DOTween.To(() => transform.eulerAngles, (a) => transform.eulerAngles = a, new Vector3(x, y, z), time);
            }
            /// <summary>
            /// Transform.Rotation değerleri ile oyna.
            /// Zaman vermezseniz anında değiştirir. void OnComplete Yollayabilirsiniz.
            /// </summary>
            public static void SetAngle(this Transform transform, float x, float y, float z, Action Method, float time = 0)
            {
                DOTween.To(() => transform.eulerAngles, (a) => transform.eulerAngles = a, new Vector3(x, y, z), time).OnComplete(() => { Method(); });
            }
        }
        public static partial class JenkinsApphic //You can only call these methods by class name.
        {
            /// <summary>
            /// Listeyi kar.
            /// </summary>
            public static void DOShuffle<T>(this IList<T> list)
            {
                System.Random rng = new System.Random();
                int n = list.Count;
                while (n > 1)
                {
                    n--;
                    int k = rng.Next(n + 1);
                    T value = list[k];
                    list[k] = list[n];
                    list[n] = value;
                }
            }
            /// <summary>
            /// Zamanla hız değerini sıfırla.
            /// </summary>
            /// <remarks>
            /// Null değerleri vermeden overload kullanabilirsiniz.
            /// </remarks>
            public static void DOSpeedDown(this float Speed, float Time, Action OnComplete = null, Ease EaseMode = Ease.Unset)
            {
                DOTween.To(() => Speed, (a) => Speed = a, 0, Time).SetEase(EaseMode).OnComplete(() => { OnComplete(); });
            }
            /// <summary>
            /// Zamanla hız değerini yükselt.
            /// </summary>
            /// <remarks>
            /// Null değerleri vermeden overload kullanabilirsiniz.
            /// </remarks>
            public static void DOSpeedUp(this float speed, float time, Action myAction = null, Ease myEase = Ease.Unset)
            {
                DOTween.To(() => speed, (a) => speed = a, 0, time).SetEase(myEase).OnComplete(() => { myAction(); });

            }
        }
    }
}



public class Test : MonoBehaviour
{
    public void Awake()
    {
        transform.SetAngle(45, 45, 45, 3);
    }
}
