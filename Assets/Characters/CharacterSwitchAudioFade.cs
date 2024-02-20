using System.Collections;
using FMOD.Studio;
using FMODUnity;
using SchizoQuest.Game;
using UnityEngine;

namespace SchizoQuest.Characters
{
    public sealed class CharacterSwitchAudioFade : MonoBehaviour
    {
        public float fadeOutTime = 0.5f;
        public float fadeInTime = 0.5f;

        public EventReference fadeSnapshot;
        private EventInstance _fadeSnapshot;
        public StudioListener audioListener;
        private float _fade;
        private float _fadeDirection;

        private void Awake()
        {
            _fadeSnapshot = RuntimeManager.CreateInstance(fadeSnapshot);
            if (!audioListener) audioListener = FindObjectOfType<StudioListener>();
        }

        private void Update()
        {
            if (!_fadeSnapshot.hasHandle()) return;
            if (_fadeDirection == 0) return;

            _fade += _fadeDirection * Time.deltaTime / (_fadeDirection < 0 ? fadeOutTime : fadeInTime);
            _fade = Mathf.Clamp01(_fade);
            _fadeSnapshot.setParameterByName("Intensity", _fade);
        }

        public void OnSwitchCharacter()
        {
            StartCoroutine(FadeCoro());
        }

        private IEnumerator FadeCoro()
        {
            enabled = true;
            audioListener.transform.SetParent(null, true);
            _fadeDirection = 1; // fade out
            yield return new WaitUntil(() => _fade >= 1);
            _fadeDirection = 0; // hold muted
            yield return new WaitUntil(() => CameraController.DistanceToActivePlayer < 50f);
            _fadeDirection = -1; // fade back in
            audioListener.transform.SetParent(Camera.main!.transform, false);
            audioListener.transform.localPosition = Vector3.zero;
            yield return new WaitUntil(() => _fade <= 0);
            enabled = false;
        }

        private void OnEnable()
        {
            _fadeSnapshot.start();
        }

        private void OnDisable()
        {
            _fadeSnapshot.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        }

        private void OnDestroy()
        {
            _fadeSnapshot.release();
        }
    }
}
