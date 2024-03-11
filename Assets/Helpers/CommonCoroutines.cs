using System;
using System.Collections;
using UnityEngine;

namespace SchizoQuest.Helpers
{
    public static class CommonCoroutines
    {
        public static IEnumerator DoOverTime(float duration, Action<float> action, bool unscaled = false)
        {
            for (float t = 0; t < duration; t += unscaled ? Time.unscaledDeltaTime : Time.deltaTime)
            {
                action(t);
                yield return null;
            }
        }
        public static IEnumerator DoOverRealTime(float duration, Action<float> action)
            => DoOverTime(duration, action, true);

        public static IEnumerator DoOverTime(float duration, Func<float, IEnumerator> coroAction, bool unscaled = false)
        {
            for (float t = 0; t < duration; t += unscaled ? Time.unscaledDeltaTime : Time.deltaTime)
            {
                float now = Time.time;
                yield return coroAction(t);
                // just in case the coro doesn't wait, we don't want to increment without time actually passing
                if (Mathf.Approximately(Time.time, now)) yield return null;
            }
        }

        public static IEnumerator DoOverRealTime(float duration, Func<float, IEnumerator> coroAction)
            => DoOverTime(duration, coroAction, true);
    }
}
