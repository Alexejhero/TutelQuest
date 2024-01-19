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
        public AnimationCurve skipHoldTransparencyCurve;
        public TMP_Text skipHoldText;
        public TitlescreenBehaviour titleScreen;

        private int _currentPanel = -1;

        private InputActions _input;
        private bool _gone;

        private void Awake()
        {
            _input = new InputActions();

            _input.UI.AnyKey.Enable();
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

        private float _skipHoldTime;

        private void Update()
        {
            if (_gone) return;

            if (_input.UI.AnyKey.IsPressed()) _skipHoldTime += Time.deltaTime;
            else _skipHoldTime = 0;

            float a = skipHoldTransparencyCurve.Evaluate(_skipHoldTime);
            skipHoldText.color = new Color(1, 1, 1, a);

            if (a >= 1)
            {
                _gone = true;
                skipHoldText.gameObject.SetActive(false);
                StartCoroutine(CoSwitchToTitleScreen());
            }
        }

        private IEnumerator CoAdvance()
        {
            _currentPanel++;

            Image panel = panels[_currentPanel];
            float oldYPos = storyboard.localPosition.y;

            Color color = default;
            Vector3 position = default;

            for (float t = 0; t < transitionLength; t += Time.deltaTime)
            {
                if (!_gone)
                {
                    // Fade in panel
                    color = panel.color;
                    color.a = Mathf.Lerp(0, 1, fadeCurve.Evaluate(t / transitionLength));
                    panel.color = color;
                }

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
        }

        private IEnumerator CoSwitchToTitleScreen()
        {
            yield return new WaitForSeconds(0.5f);

            Image currentPanel = panels[_currentPanel];
            Image lastPanel = panels[Mathf.Max(0, _currentPanel - 1)];

            float currentStartingColor = currentPanel.color.a;
            float lastStartingColor = lastPanel.color.a;

            for (float t = 0; t < 2; t += Time.deltaTime)
            {
                Color colorCurrent = currentPanel.color;
                Color colorLast = lastPanel.color;

                colorCurrent.a = Mathf.Lerp(currentStartingColor, 0, t / 2);
                colorLast.a = Mathf.Lerp(lastStartingColor, 0, t / 2);

                currentPanel.color = colorCurrent;
                lastPanel.color = colorLast;

                yield return null;
            }

            storyboard.gameObject.SetActive(false);
            titleScreen.gameObject.SetActive(true);
        }
    }
}
