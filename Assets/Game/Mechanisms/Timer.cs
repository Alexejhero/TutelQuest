using UnityEngine;

namespace SchizoQuest.Game.Mechanisms
{
    public class Timer : Switch
    {
        public float intervalOn;
        public float intervalOff;
        public float offset;
        private float _timer;

        private void OnEnable()
        {
            Init();
        }

        public override void Toggle()
        {
            enabled = !enabled;
        }

        private void Init()
        {
            _timer = offset;
            NextTimer();
        }

        private void NextTimer()
        {
            // adding makes negative offsets work
            // it also slightly enhances precision
            _timer += isOn ? intervalOn : intervalOff;
        }

        public void Update()
        {
            _timer -= Time.deltaTime;
            if (_timer <= 0)
            {
                NextTimer();
                base.Toggle();
            }
        }
    }
}