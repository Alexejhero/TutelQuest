using System;
using System.Collections;
using TMPro;
using UnityEngine;

public sealed class MenuPlayer : MonoBehaviour
{
    public TMP_Text text;

    public IEnumerator Start()
    {
        while (text.text.StartsWith("\n") || text.text.StartsWith("+"))
        {
            text.text = text.text[(text.text.IndexOf("\n", StringComparison.Ordinal) + 1)..];
            yield return new WaitForSecondsRealtime(0.2f);
        }
    }
}
