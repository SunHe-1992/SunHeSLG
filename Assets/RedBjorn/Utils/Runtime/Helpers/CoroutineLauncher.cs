using System;
using System.Collections;
using UnityEngine;

namespace RedBjorn.Utils
{
    /// <summary>
    /// Helper gameObject to launch coroutines from non-Monobehaviour classes
    /// </summary>
    public class CoroutineLauncher : MonoBehaviour
    {
        static CoroutineLauncher CachedInstance;
        static CoroutineLauncher Instance
        {
            get
            {
                if (CachedInstance == null)
                {
                    var go = new GameObject("CoroutineLauncher");
                    DontDestroyOnLoad(go);
                    CachedInstance = go.AddComponent<CoroutineLauncher>();
                }
                return CachedInstance;
            }
        }

        public static Coroutine Launch(IEnumerator ienum, Action onCompleted = null)
        {
            return Instance.Play(ienum, onCompleted);
        }

        public static void Finish(Coroutine coroutine)
        {
            Instance.Stop(coroutine);
        }

        Coroutine Play(IEnumerator ienum, Action onCompleted = null)
        {
            return StartCoroutine(WithOnCompleted(ienum, onCompleted));
        }

        void Stop(Coroutine coroutine)
        {
            StopCoroutine(coroutine);
        }

        IEnumerator WithOnCompleted(IEnumerator ienum, Action onCompleted)
        {
            while (true)
            {
                object current;
                if (!ienum.MoveNext())
                {
                    break;
                }
                current = ienum.Current;
                yield return current;
            }
            onCompleted.SafeInvoke();
        }
    }
}