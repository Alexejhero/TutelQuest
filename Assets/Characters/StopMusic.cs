using FMODUnity;
using SchizoQuest.Game.Mechanisms;
using UnityEngine;

namespace SchizoQuest.Characters
{
    public class StopMusic : Trigger<EvilForm>
    {
        public StudioEventEmitter trackToStop;
        private float _fade = 100f;
        private bool _fading;
        protected override void OnEnter(EvilForm target)
        {
            _fading = true;
        }

        protected override void OnExit(EvilForm target)
        {
        }

        public void Update()
        {
            if (!_fading) return;
            _fade -= Time.deltaTime * 100;
            trackToStop.SetParameter("Fade", _fade);
            if (_fade <= 0) Destroy(this);
        }
    }
}