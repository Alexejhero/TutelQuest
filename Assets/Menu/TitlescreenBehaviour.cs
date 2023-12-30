using System.Collections;
using SchizoQuest.Input;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SchizoQuest.Menu
{
    public class TitlescreenBehaviour : MonoBehaviour
    {
        public Image titleScreenImage;
        public TMP_Text titleText;
        public float targetY;
        public AnimationCurve movementCurve;
        public AnimationCurve fadeInCurve;

        private bool _imageShown;
        private float _timeout;
        private bool _ready = true;
        private InputActions _input;
        private bool _isMainMenuDisplayed;

        private void Awake()
        {
            _input = new InputActions();

            _input.UI.Submit.performed += TryDisplayMainMenu;
            _input.UI.Submit.Enable();

            _input.UI.Click.performed += TryDisplayMainMenu;
            _input.UI.Click.Enable();

            _input.UI.MainMenuAdvance.performed += TryDisplayMainMenu;
            _input.UI.MainMenuAdvance.Enable();
        }

        private void Update()
        {
            _timeout -= Time.deltaTime;
        }

        private void TryDisplayMainMenu(InputAction.CallbackContext ctx)
        {
            if (_ready && _timeout <= 0)
            {
                if (!_imageShown)
                {
                    _imageShown = true;
                    StartCoroutine(CoFadeIn());
                }
                else if (!_isMainMenuDisplayed)
                {
                    _isMainMenuDisplayed = true;
                    StartCoroutine(CoDisplayMainMenu());
                }
            }
        }

        private IEnumerator CoFadeIn()
        {
            _ready = false;

            Color imageColor = default;
            Color textColor = default;

            for (float t = 0; t < 3; t += Time.deltaTime)
            {
                // Fade in panel
                imageColor = titleScreenImage.color;
                imageColor.a = Mathf.Lerp(0, 1, fadeInCurve.Evaluate(t / 3));
                titleScreenImage.color = imageColor;

                textColor = titleText.color;
                textColor.a = Mathf.Lerp(0, 1, fadeInCurve.Evaluate(t / 3) / fadeInCurve.Evaluate(3));
                titleText.color = textColor;

                yield return null;
            }

            imageColor.a = fadeInCurve.Evaluate(3);
            textColor.a = 1;

            titleScreenImage.color = imageColor;
            titleText.color = textColor;

            _timeout = 0.5f;
            _ready = true;
        }

        private IEnumerator CoDisplayMainMenu()
        {
            _ready = false;

            float oldYPos = titleScreenImage.transform.localPosition.y;

            Vector3 position = default;

            for (float t = 0; t < 1; t += Time.deltaTime)
            {
                // Move storyboard
                position = titleScreenImage.transform.localPosition;
                position.y = Mathf.Lerp(oldYPos, targetY, movementCurve.Evaluate(t));
                titleScreenImage.transform.localPosition = position;

                yield return null;
            }

            position.y = targetY;

            titleScreenImage.transform.localPosition = position;

            _timeout = 0.5f;
            _ready = true;
        }

        public void PlayPressed()
        {
            // SceneManager.LoadScene("Main");
        }

        public void SettingsPressed()
        {
            SceneManager.LoadScene("Settings", LoadSceneMode.Additive);
        }

        public void QuitPressed()
        {
            Application.Quit();
        }
    }
}
