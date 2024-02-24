using System;
using SchizoQuest.Helpers;
using UnityEngine;

namespace SchizoQuest.Input.Prompts
{
    [DefaultExecutionOrder(-1000)]
    public sealed class PromptStyleManager : MonoSingleton<PromptStyleManager>
    {
        public enum Style
        {
            KenneyFull,
            KenneyFullColor,
            KenneyOutline,
            KenneyOutlineColor,
        }

        public Style style;

#if UNITY_EDITOR
        private Style _oldStyle = (Style)(-1);

        private void Update()
        {
            if (style != _oldStyle)
            {
                _oldStyle = style;
                OnStyleChange?.Invoke(style);
            }
        }
#endif

        public Action<Style> OnStyleChange;
    }
}
