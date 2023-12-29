using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace SchizoQuest.Menu
{
    public class MenuBorderPlayer : MonoBehaviour
    {
        public TMP_Text borderText;
        public MenuPlayer menuPlayer;

        private IEnumerator Start()
        {
            while (borderText.text.StartsWith("\n") || borderText.text.StartsWith("+"))
            {
                borderText.text = borderText.text[(borderText.text.IndexOf("\n", StringComparison.Ordinal) + 1)..];
                yield return new WaitForSecondsRealtime(0.25f);
            }

            menuPlayer.enabled = true;
        }
    }
}
