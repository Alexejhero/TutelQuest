using System;
using System.Runtime.InteropServices;
using NUnit.Framework.Internal.Filters;
using TMPro;
using UnityEngine;

public class RAMTimer : MonoBehaviour
{
    public float timeToComplete16GB;
    public float timeAdditionPerGB;
    public TMP_Text text;
    public AnimationCurve fpsCurve;

    private float _startingMemory;
    private float _timeElapsed;
    private float _timeToComplete;

    private void Awake()
    {
        Application.targetFrameRate = 60;

        GetPhysicallyInstalledSystemMemory(out long memoryKb);
        _startingMemory = memoryKb / 1024f / 1024f;
        _timeToComplete = timeToComplete16GB + timeAdditionPerGB * (_startingMemory - 16);
    }

    private void Update()
    {
        // 0 ... timeToComplete16GB + timeAdditionPerGB * (memory - 16)
        // _startingMemory ... 0

        _timeElapsed += Time.deltaTime;

        float remainingMemory = Mathf.Lerp(0, _startingMemory, _timeElapsed / _timeToComplete);

        text.text = $"{remainingMemory:F1}GB/{_startingMemory:F1}GB";

        Application.targetFrameRate = Mathf.RoundToInt(fpsCurve.Evaluate(_timeElapsed / _timeToComplete));

        Debug.Log(Application.targetFrameRate);
    }

    private void OnDestroy()
    {
        Application.targetFrameRate = -1;
    }

    [DllImport("kernel32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool GetPhysicallyInstalledSystemMemory(out long totalMemoryInKilobytes);
}
