using SchizoQuest.Audio;
using SchizoQuest.Characters;
using SchizoQuest.Helpers;
using SchizoQuest.Input;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SchizoQuest.Menu.PauseMenu
{
    public class PauseMenuBehaviour : MonoSingleton<PauseMenuBehaviour>
    {
        public Options options;
        public Image backgroundMaterial;
        public Selectable focusOnOpen;

        public Color color = Color.black;
        public float transitionSpeed = 5f;

        [Range(-1, 1f)]
        public float horizontalOffset = 0f;

        [Range(90, 180)]
        public float angle = 166f;

        private float _phase = 0f;
        public static bool IsOpen { get; private set; }
        internal bool canToggle;

        private InputActions _input;

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
            canToggle = true;
            _input = new InputActions();
            _input.Player.PauseMenu.started += _ => OnPauseMenu();
            _input.UI.Cancel.started += _ => OnCancel();
            SetParameters();
        }

        private bool _closing;
        public void OnPauseMenu()
        {
            // keyboard menu binds (Esc/Backspace) are assigned to both PauseMenu and Cancel
            // and we can't toggle from Cancel b/c it's double bound to SwapForm on gamepad
            if (_closing) return;
            ToggleOpen();
        }

        // this is only here to let gamepad exit with the B button
        public void OnCancel()
        {
            if (IsOpen) _closing = true;
            Close();
        }

        public void ButtonQuit()
        {
            Application.Quit();
        }

        public void ButtonToCheckpoint()
        {
            Player.ActivePlayer.respawn.Respawn();
        }

        public void ButtonToMenu()
        {
            Close();
            MainMenu.skipNextIntro = true;
            SceneManager.LoadScene(0);
        }

        public void Close(bool force = false)
        {
            if (IsOpen)
                ToggleOpen(force);
        }

        public void ToggleOpen(bool force = false)
        {
            if (!canToggle && !force) return;

            IsOpen = !IsOpen;
            OnIsOpenChanged();
        }

        private void OnIsOpenChanged()
        {
            options.gameObject.SetActive(IsOpen);
            Time.timeScale = IsOpen ? 0 : 1;
            // Player.ActivePlayer.ForceFacingFront = IsOpen;
            Player.ActivePlayer.ToggleMovement(!IsOpen);
            AudioSystem.PauseSfx(IsOpen);
            if (IsOpen) focusOnOpen.Select();
        }

        private void Update()
        {
            _phase = Mathf.Lerp(_phase, IsOpen ? 1 : 0, Time.unscaledDeltaTime * transitionSpeed);
            backgroundMaterial.material.SetFloat(phaseID, _phase);
            _closing = false;
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            _input.UI.Enable();
            _input.Player.Enable();
        }

        private void OnDisable()
        {
            Close(true);
            backgroundMaterial.material.SetFloat(phaseID, 0);
            _input.UI.Disable();
            _input.Player.Disable();
        }
    }
}
