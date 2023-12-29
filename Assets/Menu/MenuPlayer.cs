using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SchizoQuest.Menu
{
    public sealed class MenuPlayer : MonoBehaviour
    {
        public TMP_Text optionsText;

        private int _optionsIndex = -1;
        private string _originalOptionsText;
        private bool _busy;

        private void OnEnable()
        {
            StartCoroutine(Coroutine());
            return;

            IEnumerator Coroutine()
            {
                _originalOptionsText = optionsText.text;

                for (int i = 0; i < 8; i++)
                {
                    optionsText.text = optionsText.text[(optionsText.text.IndexOf("\n", StringComparison.Ordinal) + 1)..];
                    yield return new WaitForSecondsRealtime(0.25f);
                }

                optionsText.text = "\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n]   PLAY   [\nOPTIONS\nSHUTDOWN";
                yield return new WaitForSecondsRealtime(0.25f);

                optionsText.text = "\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\nPLAY\nOPTIONS\nSHUTDOWN";
                yield return new WaitForSecondsRealtime(0.25f);

                optionsText.text = "\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n]   PLAY   [\nOPTIONS\nSHUTDOWN";
                yield return new WaitForSecondsRealtime(0.25f);

                optionsText.text = "\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\nPLAY\nOPTIONS\nSHUTDOWN";
                yield return new WaitForSecondsRealtime(0.25f);

                optionsText.text = "\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n]   PLAY   [\nOPTIONS\nSHUTDOWN";
                yield return new WaitForSecondsRealtime(0.25f);

                _optionsIndex = 0;
            }
        }

        private void OnDisable()
        {
            optionsText.text = _originalOptionsText;
            _optionsIndex = -1;
        }

        private void Update()
        {
            if (_optionsIndex == -1 || _busy) return;

            if (UnityEngine.Input.GetKeyDown(KeyCode.W) || UnityEngine.Input.GetKeyDown(KeyCode.UpArrow)) _optionsIndex--;
            else if (UnityEngine.Input.GetKeyDown(KeyCode.S) || UnityEngine.Input.GetKeyDown(KeyCode.DownArrow)) _optionsIndex++;

            if (_optionsIndex < 0) _optionsIndex = 2;
            else if (_optionsIndex > 2) _optionsIndex = 0;

            optionsText.text = _optionsIndex switch
            {
                0 => "\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n]   PLAY   [\nOPTIONS\nSHUTDOWN",
                1 => "\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\nPLAY\n] \u2004OPTIONS\u2004 [\nSHUTDOWN",
                2 => "\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\nPLAY\nOPTIONS\n] SHUTDOWN [",
                _ => throw new ArgumentOutOfRangeException()
            };

            if (UnityEngine.Input.GetKeyDown(KeyCode.Space) || UnityEngine.Input.GetKeyDown(KeyCode.Return) || UnityEngine.Input.GetKeyDown(KeyCode.KeypadEnter) || UnityEngine.Input.GetKeyDown(KeyCode.E))
            {
                switch (_optionsIndex)
                {
                    case 0:
                        SceneManager.LoadScene(sceneBuildIndex: 1);
                        break;
                    case 1:
                        _busy = true;
                        StartCoroutine(CoSwitchToOptions());
                        break;
                    case 2:
                        Application.Quit();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private IEnumerator CoSwitchToOptions()
        {
            for (int i = 0; i < 23; i++)
            {
                optionsText.text = optionsText.text[(optionsText.text.IndexOf("\n", StringComparison.Ordinal) + 1)..];
                yield return new WaitForSecondsRealtime(0.15f);
            }

            _busy = false;
            gameObject.SetActive(false);
        }
    }
}
