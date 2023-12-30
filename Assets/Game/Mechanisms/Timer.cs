using UnityEngine;

namespace SchizoQuest.Game.Mechanisms
{
    public class Timer : Switch
    {
        public bool isTimerOn;

        public float intervalOn;
        public float intervalOff;
        public float offset;
        private float _nextPulseTime;

        private void Awake()
        {
            _nextPulseTime = Time.time + offset;
        }

        public override void Toggle()
        {
            isTimerOn = !isTimerOn;
            NextTimer();
        }

        private void NextTimer()
        {
            _nextPulseTime = Time.time + (isOn ? intervalOn : intervalOff);
        }

        public void Update()
        {
            if (!isTimerOn) return;
            if (Time.time < _nextPulseTime) return;

            NextTimer();
            base.Toggle();
        }
    }
}