using System.Collections;
using UnityEngine;

namespace SchizoQuest.VFX.Transition
{
    public class NeuroEvilTransitionManager : MonoBehaviour
    {
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

        public Transform NeuroTransform { set; private get; }

        #region ids

        private static readonly int playerPos = Shader.PropertyToID("_TRAN_PlayerPos");
        private static readonly int phaseID = Shader.PropertyToID("_TRAN_Phase");
        private static readonly int midThicknessID = Shader.PropertyToID("_TRAN_MiddleThickness");
        private static readonly int gradCenterID = Shader.PropertyToID("_TRAN_GradCenter");
        private static readonly int centerColorID = Shader.PropertyToID("_TRAN_CenterColor");
        private static readonly int midColorID = Shader.PropertyToID("_TRAN_MiddleColor");
        private static readonly int outsideColorID = Shader.PropertyToID("_TRAN_OutsideColor");

        #endregion ids

        private void Awake()
        {
            Shader.SetGlobalFloat(phaseID, 0f);
        }

        private void SetProps(bool isEvil)
        {
            Shader.SetGlobalFloat(midThicknessID, middleStripSize);
            Shader.SetGlobalFloat(gradCenterID, centerGradientStrength);

            if (isEvil)
            {
                Shader.SetGlobalColor(centerColorID, centerColorEvil);
                Shader.SetGlobalColor(midColorID, middleColorEvil);
                Shader.SetGlobalColor(outsideColorID, outsideColorEvil);
            }
            else
            {
                Shader.SetGlobalColor(centerColorID, centerColorNeuro);
                Shader.SetGlobalColor(midColorID, middleColorNeuro);
                Shader.SetGlobalColor(outsideColorID, outsideColorNeuro);
            }
        }

        public void Play(bool isEvil, float duration = 1f)
        {
            StartCoroutine(PlayRoutine(duration, isEvil));
        }

        private IEnumerator PlayRoutine(float duration, bool isEvil)
        {
            for (float t = 0; t < duration; t += Time.unscaledDeltaTime)
            {
                _phase = t / duration;
                _phase = isEvil ? _phase : 1 - _phase;
                _phase = phaseCurve.Evaluate(_phase);
                mask.transform.localScale = _phase * maskMaxSize * Vector3.one;

                SetProps(isEvil);
                Shader.SetGlobalFloat(phaseID, _phase);
                yield return null;
            }
            Shader.SetGlobalFloat(phaseID, isEvil ? 1f : 0f);

            mask.transform.localScale = isEvil ? 1000 * maskMaxSize * Vector3.one : Vector3.zero;
        }

        private void Update()
        {
            Shader.SetGlobalVector(playerPos, Camera.main.WorldToScreenPoint(NeuroTransform.position));
        }

        private void OnDisable()
        {
            Shader.SetGlobalFloat(phaseID, 0f);
        }
    }
}
