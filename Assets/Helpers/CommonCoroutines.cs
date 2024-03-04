using System;
using System.Collections;
using UnityEngine;

namespace SchizoQuest.Helpers
{
    public static class CommonCoroutines
    {
        public static IEnumerator DoOverTime(float duration, Action<float> action)
        {
            for (float t = 0; t < duration; t += Time.deltaTime)
            {
                action(t);
                yield return null;
            }
        }
    }
}