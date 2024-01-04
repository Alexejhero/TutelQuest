using System.Collections;
using UnityEngine;

namespace SchizoQuest
{
    public class EffectsManager : MonoBehaviour
    {
        public enum Effects
        {
            all = -1,
            death = 0,
            gameFinish = 1,
            gameStart = 2,
        }

        public static EffectsManager Instance;
        public AnimationCurve deathEffectCurve;

        public Color deathEffectFadeColor = Color.black;

        [Range(0f, 2f)]
        public float deathEffectDistplacement = 1f;

        [Space]
        public AnimationCurve gameFinishCurve;

        public Color gameFinishEffectColor = Color.white;

        [Space]
        public AnimationCurve gameStartCurve;

        public Color gameStartEffectColor = Color.white;
        public float gameStartEffectDuration = 5f;

        #region ids

        private static readonly int deathFade = Shader.PropertyToID("_DeathFade");
        private static readonly int deathFadeColor = Shader.PropertyToID("_DeathFadeColor");
        private static readonly int deathFadeOffset = Shader.PropertyToID("_DeathFadeOffset");
        private static readonly int gameFinish = Shader.PropertyToID("_GameFinish");
        private static readonly int gameFinishColor = Shader.PropertyToID("_GameFinishColor");

        #endregion ids

        private void OnValidate()
        {
            ResetValues(Effects.all);
        }

        private void Awake()
        {
            if (Instance == null) { Instance = this; } else { Destroy(this); }
            ResetValues(Effects.all);
        }

        private void Start()
        {
            PlayEffect(Effects.gameStart, gameStartEffectDuration);
        }

        private void ResetValues(Effects effect)
        {
            switch (effect)
            {
                case Effects.all:
                    Shader.SetGlobalColor(deathFadeColor, Color.white);
                    Shader.SetGlobalFloat(deathFade, 1f);
                    Shader.SetGlobalFloat(deathFadeOffset, 1f);
                    Shader.SetGlobalColor(gameFinishColor, Color.black);
                    Shader.SetGlobalFloat(gameFinish, 0f);
                    break;

                case Effects.death:
                    Shader.SetGlobalColor(deathFadeColor, Color.white);
                    Shader.SetGlobalFloat(deathFade, 1f);
                    Shader.SetGlobalFloat(deathFadeOffset, 1f);
                    break;

                case Effects.gameFinish:
                    Shader.SetGlobalColor(gameFinishColor, Color.black);
                    Shader.SetGlobalFloat(gameFinish, 0f);
                    break;
            }
        }

        public void PlayEffect(Effects effect, float duration = 1f)
        {
            switch (effect)
            {
                case Effects.death:
                    StartCoroutine(DoDeathEffect(duration));
                    break;

                case Effects.gameFinish:
                    StartCoroutine(DoGameFinishEffect(duration));
                    break;

                case Effects.gameStart:
                    StartCoroutine(DoGameStartEffect(duration));
                    break;
            }
        }

        private IEnumerator DoDeathEffect(float duration)
        {
            for (float t = 0f; t < duration; t += Time.deltaTime)
            {
                float deathEffectValue = 1 - (t / duration);
                Shader.SetGlobalColor(deathFadeColor, Color.Lerp(deathEffectFadeColor, Color.white, deathEffectCurve.Evaluate(deathEffectValue)));
                Shader.SetGlobalFloat(deathFade, deathEffectCurve.Evaluate(deathEffectValue));
                Shader.SetGlobalFloat(deathFadeOffset, deathEffectCurve.Evaluate(deathEffectValue) * deathEffectDistplacement + (1 - deathEffectDistplacement));
                yield return null;
            }
            ResetValues(Effects.death);
        }

        private IEnumerator DoGameFinishEffect(float duration)
        {
            for (float t = 0; t < duration; t += Time.deltaTime)
            {
                float gameFinishValue = 1 - (t / duration);
                Shader.SetGlobalColor(gameFinishColor, gameFinishEffectColor);
                Shader.SetGlobalFloat(gameFinish, 1 - gameFinishCurve.Evaluate(gameFinishValue));
                yield return null;
            }
            ResetValues(Effects.gameFinish);
        }

        private IEnumerator DoGameStartEffect(float duration)
        {
            for (float t = 0f; t < duration; t += Time.deltaTime)
            {
                float gameStartValue = 1 - (t / duration);
                Shader.SetGlobalColor(gameFinishColor, gameStartEffectColor);
                Shader.SetGlobalFloat(gameFinish, gameStartCurve.Evaluate(gameStartValue));
                yield return null;
            }
            ResetValues(Effects.gameFinish);
        }
    }
}
