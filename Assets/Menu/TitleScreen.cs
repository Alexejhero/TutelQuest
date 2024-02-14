using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SchizoQuest.Menu
{
    public class TitleScreen : MenuStage
    {
        public Transform rotationTransform;
        public Graphic titleScreenImage;
        public TMP_Text titleText;
        public float targetY;
        public AnimationCurve movementCurve;
        public AnimationCurve fadeInCurve;

        public GameObject optionsObject;
        public GameObject creditsObject;
        public Selectable defaultSelectedUIElement;

        private bool _ready;

        private void Start()
        {
            StartCoroutine(CoFadeIn());
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

            _ready = true;

            defaultSelectedUIElement.Select();
        }

        public void PlayPressed()
        {
            SceneManager.LoadScene("finished-map");
        }

        public void SettingsPressed()
        {
            if (!_ready) return;
            _ready = false;
            creditsObject.SetActive(false);
            optionsObject.SetActive(true);
            EventSystem.current.SetSelectedGameObject(optionsObject);
            StartCoroutine(FlipRoutine(0, 89.9f));
        }

        public void CreditsPressed()
        {
            if (!_ready) return;
            _ready = false;
            optionsObject.SetActive(false);
            creditsObject.SetActive(true);
            EventSystem.current.SetSelectedGameObject(creditsObject);
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
