using System.Collections;
using SchizoQuest.Helpers;
using SchizoQuest.Input;
using TMPro;
using UnityEngine;

namespace SchizoQuest.Menu
{
    public class WaitForInput : MenuStage
    {
        private InputActions _input;
        public TMP_Text beginText;

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
            yield return CommonCoroutines.DoOverTime(1,
                t => beginText.color = Color.Lerp(Color.white, Color.clear, t));

            Done();
        }
    }
}
