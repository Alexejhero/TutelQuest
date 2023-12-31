using System.Collections;
using System.Collections.Generic;
using SchizoQuest.Input;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace SchizoQuest.Menu
{
    public class StoryboardBehaviour : MonoBehaviour
    {
        public Transform storyboard;
        public List<Image> panels;
        public List<float> storyboardYPositions;
        public float transitionLength;
        public AnimationCurve movementCurve;
        public AnimationCurve fadeCurve;
        public TitlescreenBehaviour titleScreen;

        private float _timeout = 0;
        private int _currentPanel = -1;
        private bool _ready = true;

        private InputActions _input;

        private void Awake()
        {
            _input = new InputActions();

            // _input.UI.Submit.performed += TryAdvance;
            // _input.UI.Submit.Enable();
            //
            // _input.UI.Click.performed += TryAdvance;
            // _input.UI.Click.Enable();
            //
            // _input.UI.MainMenuAdvance.performed += TryAdvance;
            // _input.UI.MainMenuAdvance.Enable();
        }

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(3);

            for (int i = 0; i < panels.Count; i++)
            {
                yield return CoAdvance();
                yield return new WaitForSeconds(2);
            }

            yield return CoSwitchToTitleScreen();
        }

        private void Update()
        {
            _timeout -= Time.deltaTime;
        }

        private void TryAdvance(InputAction.CallbackContext ctx)
        {
            if (_ready && _timeout <= 0)
            {
                if (_currentPanel < panels.Count - 1)
                {
                    StartCoroutine(CoAdvance());
                }
                else
                {
                    StartCoroutine(CoSwitchToTitleScreen());
                }
            }
        }

        private IEnumerator CoAdvance()
        {
            _ready = false;

            _currentPanel++;

            Image panel = panels[_currentPanel];
            float oldYPos = storyboard.localPosition.y;

            Color color = default;
            Vector3 position = default;

            for (float t = 0; t < transitionLength; t += Time.deltaTime)
            {
                // Fade in panel
                color = panel.color;
                color.a = Mathf.Lerp(0, 1, fadeCurve.Evaluate(t / transitionLength));
                panel.color = color;

                // Move storyboard
                position = storyboard.localPosition;
                position.y = Mathf.Lerp(oldYPos, storyboardYPositions[_currentPanel], movementCurve.Evaluate(t / transitionLength));
                storyboard.localPosition = position;

                yield return null;
            }

            color.a = 1;
            position.y = storyboardYPositions[_currentPanel];

            panel.color = color;
            storyboard.localPosition = position;

            _timeout = 0.5f;
            _ready = true;
        }

        private IEnumerator CoSwitchToTitleScreen()
        {
            _ready = false;

            yield return new WaitForSeconds(0.5f);

            Image currentPanel = panels[_currentPanel];
            Image lastPanel = panels[_currentPanel - 1];

            for (float t = 0; t < 2; t += Time.deltaTime)
            {
                Color colorCurrent = currentPanel.color;
                Color colorLast = lastPanel.color;

                colorCurrent.a = Mathf.Lerp(1, 0, t / 2);
                colorLast.a = Mathf.Lerp(1, 0, t / 2);

                currentPanel.color = colorCurrent;
                lastPanel.color = colorLast;

                yield return null;
            }

            storyboard.gameObject.SetActive(false);
            titleScreen.gameObject.SetActive(true);
        }
    }
}
