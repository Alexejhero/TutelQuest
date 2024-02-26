using System.Collections;
using FMODUnity;
using SchizoQuest.Characters;
using UnityEngine;

namespace SchizoQuest.Audio
{
    public sealed class CharacterSwitchAudioFade : MonoBehaviour
    {
        public float fadeTime = 0.5f;

        public StudioListener vedalListener;
        public StudioListener neuroListener;
        private StudioListener _targetListener;

        private void Awake()
        {
            _targetListener = vedalListener;
            if (Mathf.Approximately(fadeTime, 0)) fadeTime = 0.001f;
        }

        private void Update()
        {
            // fade in only the active player, fade out all the rest
            int targetListenerNumber = _targetListener ? _targetListener.ListenerNumber : 0;
            for (int i = 0; i < StudioListener.ListenerCount; i++)
            {
                RuntimeManager.StudioSystem.getListenerWeight(i, out float weight);
                
                bool isTarget = i == targetListenerNumber;
                if (isTarget && weight >= 1) continue;
                if (!isTarget && weight <= 0) continue;
                
                float delta = Time.deltaTime / fadeTime;
                weight = isTarget ? weight + delta : weight - delta;
                weight = Mathf.Clamp01(weight);

                RuntimeManager.StudioSystem.setListenerWeight(i, weight);
            }
        }

        public void OnSwitch() // unity message from CharacterSwitcher when switch actually succeeds
        {
            StartCoroutine(FadeCoro());
        }

        private IEnumerator FadeCoro()
        {
            _targetListener = Player.ActivePlayer.playerType == PlayerType.Vedal
                ? vedalListener
                : neuroListener;
            enabled = true;
            yield return new WaitForSeconds(fadeTime);
            enabled = false;
        }
    }
}
