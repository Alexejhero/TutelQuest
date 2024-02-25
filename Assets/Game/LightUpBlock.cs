using UnityEngine;

namespace SchizoQuest.Game
{
    public sealed class LightUpBlock : MonoBehaviour
    {
        public SpriteRenderer spriteRenderer;
        [SerializeField] private string blendPropertyName = "_GlowTexBlend";

        public float TargetBlend => _endingBlend;

        private float _startingBlend;
        private float _endingBlend;
        private float _currentTime;
        private float _transitionTime;
        private AnimationCurve _curve;

        public void SetBlend(float blend, float time, AnimationCurve curve)
        {
            _startingBlend = spriteRenderer.material.GetFloat(blendPropertyName);
            _endingBlend = blend;
            _curve = curve;

            _currentTime = 0;
            _transitionTime = _startingBlend != _endingBlend ? time : 0;
        }

        private void Update()
        {
            if (_currentTime >= _transitionTime) return;
            _currentTime += Time.deltaTime;

            float value = Mathf.Lerp(_startingBlend, _endingBlend, _currentTime / _transitionTime);

            spriteRenderer.material.SetFloat(blendPropertyName, /*_curve.Evaluate(*/value/*)*/);
        }
    }
}
