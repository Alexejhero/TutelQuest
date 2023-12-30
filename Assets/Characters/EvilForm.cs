using System.Collections;
using SchizoQuest.Transition_effects;
using UnityEngine;
using UnityEngine.Rendering;

namespace SchizoQuest.Characters
{
    public sealed class EvilForm : CharacterAltForm
    {
        public ParticleSystem evilSwitchParticleEffect;
        public NeuroEvilTransitionManager neuroEvilTransitionManager;
        private ParticleSystem _neuroSwitchParticleEffect;
        private CharacterSwitcher _switcher;
        public float switchTransitionDuration = 0.5f;

        private Volume _evilVolume;
        private CameraController _cameraController;

        public void Awake()
        {
            _switcher = gameObject.GetComponentInParent<CharacterSwitcher>();
            _neuroSwitchParticleEffect = player.characterSwitchParticleEffect;

            _evilVolume = Camera.main!.GetComponent<Volume>();
            _cameraController = Camera.main!.GetComponent<CameraController>();
        }

        protected override bool CanSwap(bool toAlt) => true;

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

            _switcher._music.SetParameter("Character", isAlt ? 2 : 1);

            float startWeight = isAlt ? 0 : 1;
            float endWeight = isAlt ? 1 : 0;

            for (float t = 0; t < switchTransitionDuration / 2; t += Time.deltaTime)
            {
                _evilVolume.weight = Mathf.Lerp(startWeight, endWeight, t / switchTransitionDuration / 2);
                yield return null;
            }

            _evilVolume.weight = endWeight;
            _cameraController.SetNeuroState(isAlt ? NeuroState.Evil : NeuroState.Neuro);
        }
    }
}
