using System;
using System.Collections;
using System.Runtime.CompilerServices;
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
        public Transform rotationTransform;
        public Image titleScreenImage;
        public TMP_Text titleText;
        public float targetY;
        public AnimationCurve movementCurve;
        public AnimationCurve fadeInCurve;

        public GameObject optionsObject;
        public GameObject creditsObject;

        private float _timeout;
        private bool _ready = false;
        private InputActions _input;
        private bool _isMainMenuDisplayed;

        private void Awake()
        {
            _input = new InputActions();

            /*_input.UI.Submit.performed += TryDisplayMainMenu;
            _input.UI.Submit.Enable();

            _input.UI.Click.performed += TryDisplayMainMenu;
            _input.UI.Click.Enable();

            _input.UI.MainMenuAdvance.performed += TryDisplayMainMenu;
            _input.UI.MainMenuAdvance.Enable();*/
        }

        private void Start()
        {
            StartCoroutine(CoFadeIn());
        }

        private void Update()
        {
            _timeout -= Time.deltaTime;
        }

        private void TryDisplayMainMenu(InputAction.CallbackContext ctx)
        {
            if (_ready && _timeout <= 0)
            {
                if (!_isMainMenuDisplayed)
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

            yield return CoDisplayMainMenu();
        }

        private IEnumerator CoDisplayMainMenu()
        {
            _ready = false;

            float oldYPos = rotationTransform.localPosition.y;

            Vector3 position = default;

            for (float t = 0; t < 1; t += Time.deltaTime)
            {
                // Move storyboard
                position = rotationTransform.localPosition;
                position.y = Mathf.Lerp(oldYPos, targetY, movementCurve.Evaluate(t));
                rotationTransform.localPosition = position;

                yield return null;
            }

            position.y = targetY;

            rotationTransform.localPosition = position;

            _timeout = 0.5f;
            _ready = true;
        }

        public void PlayPressed()
        {
            // SceneManager.LoadScene("Main");
        }

        public void SettingsPressed()
        {
            if (!_ready) return;
            _ready = false;
            creditsObject.SetActive(false);
            optionsObject.SetActive(true);
            StartCoroutine(FlipRoutine(0, 89.9f));
        }

        public void CreditsPressed()
        {
            if (!_ready) return;
            _ready = false;
            optionsObject.SetActive(false);
            creditsObject.SetActive(true);
            StartCoroutine(FlipRoutine(0, 89.9f));
        }

        public void BackPressed()
        {
            if (_ready) return;
            _ready = true;
            StartCoroutine(FlipRoutine(89.9f, 0));
        }

        public void QuitPressed()
        {
            Application.Quit();
        }

        private IEnumerator FlipRoutine(float from, float to)
        {
            Vector3 angles = default;

            for (float t = 0; t < 1; t += Time.deltaTime)
            {
                angles = rotationTransform.transform.eulerAngles;
                angles.y = Mathf.Lerp(from, to, t);
                rotationTransform.transform.eulerAngles = angles;

                yield return null;
            }

            angles.y = to;
            rotationTransform.transform.eulerAngles = angles;
        }
    }
}
