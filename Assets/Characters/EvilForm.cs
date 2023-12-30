using System.Collections;
using SchizoQuest.Transition_effects;
using UnityEngine;
using UnityEngine.Rendering;

namespace SchizoQuest.Characters
{
    public sealed class EvilForm : CharacterAltForm
    {
        public ParticleSystem evilSwitchParticleEffect;
        private ParticleSystem _neuroSwitchParticleEffect;
        private CharacterSwitcher switcher;
        public NeuroEvilTransitionManager neuroEvilTransitionManager;
        public float switchTransitionDuration = 0.5f;
        public Volume evilVolume;
        public CameraController cameraController;

        public void Awake()
        {
            switcher = gameObject.GetComponentInParent<CharacterSwitcher>();
            _neuroSwitchParticleEffect = player.characterSwitchParticleEffect;
        }

        protected override void OnSwap(bool isAlt)
        {
            StartCoroutine(CoOnSwap(isAlt));
        }

        private IEnumerator CoOnSwap(bool isAlt)
        {
            player.playerType = isAlt
                ? PlayerType.Evil
                : PlayerType.Neuro;

            neuroEvilTransitionManager.isEvil = isAlt;
            neuroEvilTransitionManager.Play(switchTransitionDuration);

            player.characterSwitchParticleEffect = isAlt
                ? evilSwitchParticleEffect
                : _neuroSwitchParticleEffect;

            switcher.music.SetParameter("Character", isAlt ? 2 : 1);

            float startWeight = isAlt ? 0 : 1;
            float endWeight = isAlt ? 1 : 0;

            for (float t = 0; t < switchTransitionDuration / 2; t += Time.deltaTime)
            {
                evilVolume.weight = Mathf.Lerp(startWeight, endWeight, t / switchTransitionDuration / 2);
                yield return null;
            }

            evilVolume.weight = endWeight;
            cameraController.SetNeuroState(isAlt ? NeuroState.Evil : NeuroState.Neuro);
        }
    }
}
