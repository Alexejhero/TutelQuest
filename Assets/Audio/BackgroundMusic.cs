using FMODUnity;
using SchizoQuest.Helpers;
using UnityEngine;

namespace SchizoQuest.Audio
{
    public sealed class BackgroundMusic : MonoSingleton<BackgroundMusic>
    {
        [SerializeField]
        private StudioEventEmitter track;
        private StudioEventEmitter _nextTrack;
        public float fadeTime = 1f;

        protected override void Awake()
        {
            base.Awake();
            if (track)
            {
                track.Play();
                track.SetParameter("Fade", 1);
            }
        }

        public StudioEventEmitter GetTrack() => track;

        public void SetTrack(StudioEventEmitter newTrack)
        {
            // stop existing fade (if any)
            ResetFade();
            if (_nextTrack)
            {
                _nextTrack.SetParameter("Fade", 0);
                _nextTrack.Stop();
            }
            if (newTrack) newTrack.Play();
            
            _nextTrack = newTrack;
            _fading = true;
        }

        private void Update()
        {
            UpdateFade();
        }

        private bool _fading;
        private float _fade;
        private void UpdateFade()
        {
            if (!_fading) return;
            _fade += Time.deltaTime / fadeTime;
            if (track)
                track.SetParameter("Fade", 1 - _fade);
            if (_nextTrack)
                _nextTrack.SetParameter("Fade", _fade);
            if (_fade >= 1)
            {
                if (track) track.Stop();
                track = _nextTrack;
                _nextTrack = null;
                ResetFade();
            }
        }

        private void ResetFade()
        {
            _fading = false;
            _fade = 0;
        }
    }
}