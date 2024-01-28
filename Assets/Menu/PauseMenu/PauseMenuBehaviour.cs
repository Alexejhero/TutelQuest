using SchizoQuest.Game;
using SchizoQuest.Helpers;
using SchizoQuest.Menu;
using UnityEngine;
using UnityEngine.UI;

namespace SchizoQuest
{
    public class PauseMenuBehaviour : MonoSingleton<PauseMenuBehaviour>
    {
        public OptionsBehaviour options;
        public Image backgroundMaterial;

        public Color color = Color.black;
        public float transitionSpeed = 5f;

        [Range(-1, 1f)]
        public float horizontalOffset = 0f;

        [Range(1.5708f, Mathf.PI)]
        public float angle = 3f;

        private float _phase = 0f;
        public static bool IsOpen { private set; get; }
        private bool _isLocked = false;

        #region IDs

        private static readonly int colorID = Shader.PropertyToID("_PAUS_Color");
        private static readonly int horizOffset = Shader.PropertyToID("_PAUS_HorizOffset");
        private static readonly int phaseID = Shader.PropertyToID("_PAUS_Phase");
        private static readonly int angleID = Shader.PropertyToID("_PAUS_Angle");

        #endregion IDs

        private void SetParameters()
        {
            backgroundMaterial.material.SetColor(colorID, color);
            backgroundMaterial.material.SetFloat(horizOffset, horizontalOffset);
            backgroundMaterial.material.SetFloat(phaseID, _phase);
            backgroundMaterial.material.SetFloat(angleID, angle);
        }

        private void OnValidate()
        {
            SetParameters();
        }

        protected override void Awake()
        {
            base.Awake();
            SetParameters();
        }

        public void OnCancel()
        {
            ToggleOptions();
        }

        public void ForceCloseAndPreventOpening(bool preventOpening = true)
        {
            if (IsOpen)
            {
                ToggleOptions();
            }
            if (!_isLocked && preventOpening)
            {
                _isLocked = true;
            }
        }

        public void ToggleOptions()
        {
            if (_isLocked) return;

            IsOpen = !IsOpen;
            MonoSingleton<CameraController>.Instance.IsInPauseMenu = IsOpen;
            options.gameObject.SetActive(IsOpen);
            //Timescale.Instance.timescale = IsOpen? 0 : 1;
        }

        private void Update()
        {
            _phase = Mathf.Lerp(_phase, IsOpen ? 1 : 0, Time.deltaTime * transitionSpeed);
            backgroundMaterial.material.SetFloat(phaseID, _phase);
        }

        private void OnDisable()
        {
            backgroundMaterial.material.SetFloat(phaseID, 0);
        }
    }
}
