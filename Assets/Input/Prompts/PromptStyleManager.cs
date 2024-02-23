﻿using System;
using SchizoQuest.Helpers;

namespace SchizoQuest.Input
{
    public sealed class PromptStyleManager : MonoSingleton<PromptStyleManager>
    {
        public enum Style
        {
            Jstatz,
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
