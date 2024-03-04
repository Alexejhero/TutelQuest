using System.Collections;
using UnityEngine;

namespace SchizoQuest.VFX.Transition
{
    public sealed class EffectsManager : MonoBehaviour
    {
        public enum Effects
        {
            all = -1,
            death = 0,
            gameFinish = 1,
            gameStart = 2,
        }

        public static EffectsManager Instance;

        [Header("Death")]
        public AnimationCurve deathEffectCurve;

        public Color deathEffectFadeColor = Color.black;

        [Range(0f, 2f)]
        public float deathEffectDistplacement = 1f;

        [Space, Header("Game Finish")]
        public AnimationCurve gameFinishCurve;

        public Color gameFinishEffectColor = Color.white;

        [Space, Header("Game Start")]
        public AnimationCurve gameStartCurve;

        public Color gameStartEffectColor = Color.white;
        public float gameStartEffectDuration = 5f;

        [Space, Header("SKY")]
        public Texture2D sunTexture;
        public Vector2 sunPosition = new(0.13f, 0.46f);
        public float sunScale = 0.5f;
        public Color nightSkyColor;
        public Color daySkyColor;
        public Color daySkyColor2;
        public Color starColor;

        #region ids

        private static readonly int deathFadeID = Shader.PropertyToID("_DeathFade");
        private static readonly int deathFadeColorID = Shader.PropertyToID("_DeathFadeColor");
        private static readonly int deathFadeOffsetID = Shader.PropertyToID("_DeathFadeOffset");
        private static readonly int gameFinishID = Shader.PropertyToID("_GameFinish");
        private static readonly int gameFinishColorID = Shader.PropertyToID("_GameFinishColor");
        private static readonly int skySunTexID = Shader.PropertyToID("_SKY_SunTex");
        private static readonly int skySunPosID = Shader.PropertyToID("_SKY_SunPos");
        private static readonly int skySunScaleID = Shader.PropertyToID("_SKY_SunScale");
        private static readonly int skyNightSkyColorID = Shader.PropertyToID("_SKY_NightSkyColor");
        private static readonly int skyDaySkyColorID = Shader.PropertyToID("_SKY_DaySkyColor");
        private static readonly int skyDaySkyColor2ID = Shader.PropertyToID("_SKY_DaySkyColor2");
        private static readonly int skyStarColorID = Shader.PropertyToID("_SKY_StarColor");

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

        private void OnDestroy()
        {
            ResetValues(Effects.all);
        }

        private void ResetValues(Effects effect)
        {
            switch (effect)
            {
                case Effects.all:
                    Shader.SetGlobalColor(deathFadeColorID, Color.white);
                    Shader.SetGlobalFloat(deathFadeID, 1f);
                    Shader.SetGlobalFloat(deathFadeOffsetID, 1f);
                    Shader.SetGlobalColor(gameFinishColorID, Color.black);
                    Shader.SetGlobalFloat(gameFinishID, 0f);
                    Shader.SetGlobalTexture(skySunTexID, sunTexture);
                    Shader.SetGlobalVector(skySunPosID, sunPosition);
                    Shader.SetGlobalFloat(skySunScaleID, sunScale);
                    Shader.SetGlobalColor(skyNightSkyColorID, nightSkyColor);
                    Shader.SetGlobalColor(skyDaySkyColorID, daySkyColor);
                    Shader.SetGlobalColor(skyDaySkyColor2ID, daySkyColor2);
                    Shader.SetGlobalColor(skyStarColorID, starColor);
                    break;

                case Effects.death:
                    Shader.SetGlobalColor(deathFadeColorID, Color.white);
                    Shader.SetGlobalFloat(deathFadeID, 1f);
                    Shader.SetGlobalFloat(deathFadeOffsetID, 1f);
                    break;

                case Effects.gameFinish:
                    Shader.SetGlobalColor(gameFinishColorID, Color.black);
                    Shader.SetGlobalFloat(gameFinishID, 0f);
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
                Shader.SetGlobalColor(deathFadeColorID, Color.Lerp(deathEffectFadeColor, Color.white, deathEffectCurve.Evaluate(deathEffectValue)));
                Shader.SetGlobalFloat(deathFadeID, deathEffectCurve.Evaluate(deathEffectValue));
                Shader.SetGlobalFloat(deathFadeOffsetID, deathEffectCurve.Evaluate(deathEffectValue) * deathEffectDistplacement + (1 - deathEffectDistplacement));
                yield return null;
            }
            ResetValues(Effects.death);
        }

        private IEnumerator DoGameFinishEffect(float duration)
        {
            for (float t = 0; t < duration; t += Time.deltaTime)
            {
                float gameFinishValue = 1 - (t / duration);
                Shader.SetGlobalColor(gameFinishColorID, gameFinishEffectColor);
                Shader.SetGlobalFloat(gameFinishID, 1 - gameFinishCurve.Evaluate(gameFinishValue));
                yield return null;
            }
            ResetValues(Effects.gameFinish);
        }

        private IEnumerator DoGameStartEffect(float duration)
        {
            for (float t = 0f; t < duration; t += Time.deltaTime)
            {
                float gameStartValue = 1 - (t / duration);
                Shader.SetGlobalColor(gameFinishColorID, gameStartEffectColor);
                Shader.SetGlobalFloat(gameFinishID, gameStartCurve.Evaluate(gameStartValue));
                yield return null;
            }
            ResetValues(Effects.gameFinish);
        }
    }
}
