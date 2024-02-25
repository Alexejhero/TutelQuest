using SchizoQuest.Characters;
using SchizoQuest.Characters.Vedal;
using UnityEngine;

namespace SchizoQuest.Game
{
    public sealed class TutelDashHint : MonoBehaviour
    {
        private TutelForm _tutelForm;

        public float enableDistance = 17f;
        public LightUpBlock[] hints;
        public AnimationCurve curve;
        public float hintDuration = 0.8f;

        private void SetHint(int index, bool on)
        {
            float blend = on ? 1f : 0f;
            if (hints[index].TargetBlend != blend) hints[index].SetBlend(blend, hintDuration, curve);
        }

        private void Update()
        {
            if (!_tutelForm && CharacterSwitcher.Instance.CurrentPlayer.GetComponent<TutelForm>() is { } tutelForm) _tutelForm = tutelForm;
        }

        private void LateUpdate()
        {
            if (Vector2.Distance(_tutelForm.transform.position, transform.position) > enableDistance)
            {
                bool passed = _tutelForm.transform.position.x > transform.position.x;

                SetHint(0, passed);
                SetHint(1, passed);
                SetHint(2, passed);
                return;
            }

            SetHint(0, _tutelForm.IsDashing || !_tutelForm.IsAlt);
            SetHint(1, _tutelForm.IsDashing || (!_tutelForm.IsAlt && Mathf.Abs(_tutelForm.rb.velocity.x) / _tutelForm.controller.stats.maxHorizontalSpeed >= 0.8f));
            SetHint(2, _tutelForm.IsDashing);
        }
    }
}
