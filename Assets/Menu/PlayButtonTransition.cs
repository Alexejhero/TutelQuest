using System.Collections;
using SchizoQuest.Helpers;
using UnityEngine;

namespace SchizoQuest.Menu
{
    public sealed class PlayButtonTransition : MonoBehaviour
    {
        public AnimationCurve alphaCurve;
        public float duration = 1f;

        private static readonly int gameFinishID = Shader.PropertyToID("_GameFinish");
        private static readonly int gameFinishColorID = Shader.PropertyToID("_GameFinishColor");

        public IEnumerator DoGameStartEffect()
        {
            yield return CommonCoroutines.DoOverTime(duration, t =>
            {
                float gameStartValue = t / duration;
                Shader.SetGlobalColor(gameFinishColorID, Color.white);
                Shader.SetGlobalFloat(gameFinishID, alphaCurve.Evaluate(gameStartValue));
            });
        }
    }
}
