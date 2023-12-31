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

        private float _deathEffectStart;
        private float _deathEffectDuration = 0.5f;

        [Space]
        public AnimationCurve gameFinishCurve;

        public Color gameFinishEffectColor = Color.white;
        private float _gameFinishEffectStart;
        private float _gameFinishEffectDuration = 5f;

        [Space]
        public AnimationCurve gameStartCurve;

        public Color gameStartEffectColor = Color.white;
        private float _gameStartEffectStart;
        public float gameStartEffectDuration = 5f;
        private float _gameStartEffectDuration;

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
            _deathEffectStart = Time.time - _deathEffectDuration;
            _gameFinishEffectStart = Time.time - _gameFinishEffectDuration;
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
            duration = Mathf.Max(0.005f, duration);
            switch (effect)
            {
                case Effects.death:
                    _deathEffectDuration = duration;
                    _deathEffectStart = Time.time;
                    break;

                case Effects.gameFinish:
                    _gameFinishEffectDuration = duration;
                    _gameFinishEffectStart = Time.time;
                    break;

                case Effects.gameStart:
                    _gameStartEffectDuration = duration;
                    _gameStartEffectStart = Time.time;
                    break;
            }
        }

        private void DoDeathEffect()
        {
            if (Time.time < _deathEffectStart + _deathEffectDuration)
            {
                float deathEffectValue = ((_deathEffectStart + _deathEffectDuration) - Time.time) / _deathEffectDuration;
                Shader.SetGlobalColor(deathFadeColor, Color.Lerp(deathEffectFadeColor, Color.white, deathEffectCurve.Evaluate(deathEffectValue)));
                Shader.SetGlobalFloat(deathFade, deathEffectCurve.Evaluate(deathEffectValue));
                Shader.SetGlobalFloat(deathFadeOffset, deathEffectCurve.Evaluate(deathEffectValue) * deathEffectDistplacement + (1 - deathEffectDistplacement));
            }
            else { ResetValues(Effects.death); }
        }

        private void DoGameFinishEffect()
        {
            if (Time.time < _gameFinishEffectStart + _gameFinishEffectDuration)
            {
                float gameFinishValue = ((_gameFinishEffectStart + _gameFinishEffectDuration) - Time.time) / _gameFinishEffectDuration;
                Shader.SetGlobalColor(gameFinishColor, gameFinishEffectColor);
                Shader.SetGlobalFloat(gameFinish, 1 - gameFinishCurve.Evaluate(gameFinishValue));
            }
            else { ResetValues(Effects.gameFinish); }
        }

        private void DoGameStartEffect()
        {
            if (Time.time < _gameStartEffectStart + _gameStartEffectDuration)
            {
                float gameStartValue = ((_gameStartEffectStart + _gameStartEffectDuration) - Time.time) / _gameStartEffectDuration;
                Shader.SetGlobalColor(gameFinishColor, gameStartEffectColor);
                Shader.SetGlobalFloat(gameFinish, gameStartCurve.Evaluate(gameStartValue));
            }
            else { DoGameFinishEffect(); }
        }

        private void Update()
        {
            DoGameStartEffect();
            DoDeathEffect();
        }
    }
}
