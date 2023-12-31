using UnityEngine;
using SchizoQuest.Characters;

namespace SchizoQuest.Transition_effects
{
    public class NeuroEvilTransitionManager : MonoBehaviour
    {
        private enum EvilNeuro
        {
            neuro = 0,
            evil = 1,
        }

        public SpriteMask mask;
        public float maskMaxSize = 100f;

        [Range(0f, 1f)]
        public float phase;

        private float _phase;
        public AnimationCurve phaseCurve;

        [Range(0f, 1f)]
        public float middleStripSize = 0.5f;

        [Range(0f, 1f)]
        public float centerGradientStrength = 0f;

        public Color centerColorNeuro = Color.blue;
        public Color middleColorNeuro = Color.cyan;
        public Color outsideColorNeuro = Color.black;

        public Color centerColorEvil = Color.green;
        public Color middleColorEvil = Color.red;
        public Color outsideColorEvil = Color.black;

        public bool isEvil = false;

        private float _playEndTime;
        private float _playStartTime;
        private float _duration = 0.1f;

        #region ids

        private static readonly int playerPos = Shader.PropertyToID("_TRAN_PlayerPos");
        private static readonly int phaseID = Shader.PropertyToID("_TRAN_Phase");
        private static readonly int midThicknessID = Shader.PropertyToID("_TRAN_MiddleThickness");
        private static readonly int gradCenterID = Shader.PropertyToID("_TRAN_GradCenter");
        private static readonly int centerColorID = Shader.PropertyToID("_TRAN_CenterColor");
        private static readonly int midColorID = Shader.PropertyToID("_TRAN_MiddleColor");
        private static readonly int outsideColorID = Shader.PropertyToID("_TRAN_OutsideColor");

        #endregion ids

        private void SetProps(EvilNeuro en)
        {
            Shader.SetGlobalFloat(midThicknessID, middleStripSize);
            Shader.SetGlobalFloat(gradCenterID, centerGradientStrength);

            switch (en)
            {
                case EvilNeuro.evil:
                    Shader.SetGlobalColor(centerColorID, centerColorEvil);
                    Shader.SetGlobalColor(midColorID, middleColorEvil);
                    Shader.SetGlobalColor(outsideColorID, outsideColorEvil);
                    return;

                case EvilNeuro.neuro:
                    Shader.SetGlobalColor(centerColorID, centerColorNeuro);
                    Shader.SetGlobalColor(midColorID, middleColorNeuro);
                    Shader.SetGlobalColor(outsideColorID, outsideColorNeuro);
                    return;
            }
        }

        public void Play(float duration = 1f)
        {
            _playStartTime = Time.time;
            _duration = Mathf.Max(float.Epsilon, duration);
            _playEndTime = _playStartTime + _duration;
        }

        private void Update()
        {
            if (Time.time < _playEndTime)
            {
                phase = (Time.time - _playStartTime) / _duration;
                phase = isEvil ? phase : 1 - phase;
                _phase = phaseCurve.Evaluate(phase + float.Epsilon);
                mask.transform.localScale = Mathf.Abs(Mathf.Clamp01(_phase)) * maskMaxSize * Vector3.one;

                SetProps(isEvil ? EvilNeuro.evil : EvilNeuro.neuro);
                Shader.SetGlobalVector(playerPos, Camera.main.WorldToScreenPoint(Player.ActivePlayer.transform.position));
                Shader.SetGlobalFloat(phaseID, _phase);
            }
            else
            {
                if (isEvil) mask.transform.localScale = 1000 * maskMaxSize * Vector3.one;
            }
        }
    }
}
