using SchizoQuest.Characters;
using SchizoQuest.Game;
using SchizoQuest.Helpers;
using SchizoQuest.Menu;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SchizoQuest
{
    public class PauseMenuBehaviour : MonoSingleton<PauseMenuBehaviour>
    {
        public Options options;
        public Image backgroundMaterial;

        public Color color = Color.black;
        public float transitionSpeed = 5f;

        [Range(-1, 1f)]
        public float horizontalOffset = 0f;

        [Range(90, 180)]
        public float angle = 166f;

        private float _phase = 0f;
        public static bool IsOpen { get; private set; }
        internal bool canToggle = true;

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

        private void Start()
        {
            SetParameters();
        }

        public void OnCancel()
        {
            ToggleOpen();
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
            Timescale.Instance.timescale = IsOpen ? 0 : 1;
            // forces idle anim (facing the camera)
            Player.ActivePlayer.Winning = IsOpen;
        }

        private void Update()
        {
            _phase = Mathf.Lerp(_phase, IsOpen ? 1 : 0, Time.unscaledDeltaTime * transitionSpeed);
            backgroundMaterial.material.SetFloat(phaseID, _phase);
        }

        private void OnDisable()
        {
            Close(true);
            backgroundMaterial.material.SetFloat(phaseID, 0);
        }
    }
}
