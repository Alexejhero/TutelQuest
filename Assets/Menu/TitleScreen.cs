using System.Collections;
using SchizoQuest.End;
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
        public PlayButtonTransition playButtonTransition;

        public GameObject optionsObject;
        public GameObject creditsObject;
        public Selectable focusOnOpen;
        public Selectable optionsButton;
        public Selectable creditsButton;

        private bool _ready;
        private Selectable _selectOnReturn;
        private GameObject _currentPage;

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

            focusOnOpen.Select();
            _selectOnReturn = focusOnOpen;
        }

        public void PlayPressed()
        {
            _ready = false;
            StartCoroutine(CoPlayPressed());
            return;

            IEnumerator CoPlayPressed()
            {
                MainMenu.Instance.music.SetParameter("Release", playButtonTransition.duration);
                MainMenu.Instance.music.Stop(); // will fade out over ^this many seconds
                yield return playButtonTransition.DoGameStartEffect();
                EndManager.GotRum = false;
                SceneManager.LoadScene("finished-map");
            }
        }

        public void SettingsPressed()
        {
            if (!_ready) return;
            _selectOnReturn = optionsButton;
            StartCoroutine(Open(optionsObject));
        }

        public void CreditsPressed()
        {
            if (!_ready) return;
            _selectOnReturn = creditsButton;
            StartCoroutine(Open(creditsObject));
        }

        public void BackPressed()
        {
            if (!_ready) return;
            StartCoroutine(Close());
        }

        private IEnumerator Open(GameObject page)
        {
            EventSystem.current.SetSelectedGameObject(null);
            if (_currentPage && _currentPage != page)
            {
                yield return Close();
            }
            _ready = false;
            page.SetActive(true);
            if (!_currentPage) // won't flip if we're already on the desired page
            {
                yield return FlipRoutine(0, 89.9f);
            }
            _currentPage = page;
            page.GetComponentInChildren<Selectable>().Select();
            _ready = true;
        }

        private IEnumerator Close()
        {
            _ready = false;
            if (_currentPage)
            {
                yield return FlipRoutine(89.9f, 0);
                _currentPage.SetActive(false);
                _currentPage = null;
            }
            _selectOnReturn.Select();
            _selectOnReturn = focusOnOpen;
            _ready = true;
        }

        public void OnCancel()
        {
            BackPressed();
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
