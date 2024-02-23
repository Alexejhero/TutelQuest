using UnityEngine;

namespace SchizoQuest.Input
{
    public sealed class PromptStyleSwitcher : MonoBehaviour
    {
        public GameObject jstatz;
        public GameObject kenneyFull;
        public GameObject kenneyFullColor;
        public GameObject kenneyOutline;
        public GameObject kenneyOutlineColor;

        private void OnEnable()
        {
            PromptStyleManager.Instance.OnStyleChange += OnStyleChange;
            OnStyleChange(PromptStyleManager.Instance.style);
        }

        private void OnDisable()
        {
            PromptStyleManager.Instance.OnStyleChange -= OnStyleChange;
        }

        private void OnStyleChange(PromptStyleManager.Style style)
        {
            jstatz.SetActive(false);
            kenneyFull.SetActive(false);
            kenneyFullColor.SetActive(false);
            kenneyOutline.SetActive(false);
            kenneyOutlineColor.SetActive(false);

            switch (style)
            {
                case PromptStyleManager.Style.Jstatz:
                    jstatz.SetActive(true);
                    break;
                case PromptStyleManager.Style.KenneyFull:
                    kenneyFull.SetActive(true);
                    break;
                case PromptStyleManager.Style.KenneyFullColor:
                    kenneyFullColor.SetActive(true);
                    break;
                case PromptStyleManager.Style.KenneyOutline:
                    kenneyOutline.SetActive(true);
                    break;
                case PromptStyleManager.Style.KenneyOutlineColor:
                    kenneyOutlineColor.SetActive(true);
                    break;
            }
        }
    }
}
