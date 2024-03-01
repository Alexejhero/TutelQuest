using System.Collections;
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
            for (float t = 0f; t < duration; t += Time.deltaTime)
            {
                float gameStartValue = t / duration;
                Shader.SetGlobalColor(gameFinishColorID, Color.white);
                Shader.SetGlobalFloat(gameFinishID, alphaCurve.Evaluate(gameStartValue));
                yield return null;
            }
        }
    }
}
