using System.Collections;
using SchizoQuest.Input;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace SchizoQuest.Menu
{
    public class TitlescreenBehaviour : MonoBehaviour
    {
        public Image titleScreenImage;
        public float targetY;
        public AnimationCurve movementCurve;

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

            Color color = default;

            for (float t = 0; t < 3; t += Time.deltaTime)
            {
                // Fade in panel
                color = titleScreenImage.color;
                color.a = Mathf.Lerp(0, 1, t / 3);
                titleScreenImage.color = color;

                yield return null;
            }

            color.a = 1;

            titleScreenImage.color = color;

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
    }
}
