using UnityEngine;

namespace SchizoQuest.Input.Prompts
{
    public sealed class PromptStyleSwitcher : MonoBehaviour
    {
        public GameObject kenneyFull;
        public GameObject kenneyFullColor;
        public GameObject kenneyOutline;
        public GameObject kenneyOutlineColor;

        private void Awake()
        {
            PromptStyleManager.Instance.OnStyleChange += OnStyleChange;
            OnStyleChange(PromptStyleManager.Instance.style);
        }

        private void OnStyleChange(PromptStyleManager.Style style)
        {
            kenneyFull.SetActive(false);
            kenneyFullColor.SetActive(false);
            kenneyOutline.SetActive(false);
            kenneyOutlineColor.SetActive(false);

            switch (style)
            {
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
