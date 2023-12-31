using System;
using System.Collections;
using SchizoQuest.Input;
using TMPro;
using UnityEngine;

namespace SchizoQuest.Menu
{
    public class BeginBehaviour : MonoBehaviour
    {
        private InputActions _input;
        public TMP_Text beginText;
        public StoryboardBehaviour next;

        private void Awake()
        {
            _input = new InputActions();

            _input.UI.AnyKey.performed += _ => StartCoroutine(CoBegin());
        }

        private void OnEnable()
        {
            _input.UI.AnyKey.Enable();
        }

        private void OnDisable()
        {
            _input.UI.AnyKey.Disable();
        }

        private IEnumerator CoBegin()
        {
            for (float t = 0; t < 1; t += Time.deltaTime)
            {
                beginText.color = Color.Lerp(Color.white, Color.clear, t);

                yield return null;
            }

            next.gameObject.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
