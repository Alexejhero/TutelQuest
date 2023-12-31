using System.Collections;
using SchizoQuest.Game;
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
        //private CameraController _cameraController;

        private int _evilLayer;
        private int _neuroLayer;
        private int _playerLayer;
        public bool enableSwitching = true;

        private bool _hintHidden;

        public void Awake()
        {
            _switcher = gameObject.GetComponentInParent<CharacterSwitcher>();
            _neuroSwitchParticleEffect = player.characterSwitchParticleEffect;

            _evilVolume = Camera.main!.GetComponent<Volume>();
            //_cameraController = Camera.main!.GetComponent<CameraController>();
            _evilLayer = LayerMask.NameToLayer("EvilOnly");
            _neuroLayer = LayerMask.NameToLayer("NeuroOnly");
            _playerLayer = LayerMask.NameToLayer("Player");
            UpdateLayerCollision();
        }

        protected override bool CanSwap(bool toAlt) => enableSwitching;

        protected override void OnSwap()
        {
            StartCoroutine(CoOnSwap());
        }

        private IEnumerator CoOnSwap()
        {
            if (!_hintHidden)
            {
                _hintHidden = true;
                player.SendMessage("HideHint", HintType.F2, SendMessageOptions.DontRequireReceiver);
            }

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
            //_cameraController.SetNeuroState(isAlt);
            UpdateLayerCollision();
        }


        private void UpdateLayerCollision()
        {
            Physics2D.IgnoreLayerCollision(_evilLayer, _playerLayer, !isAlt);
            Physics2D.IgnoreLayerCollision(_neuroLayer, _playerLayer, isAlt);
        }
    }
}
