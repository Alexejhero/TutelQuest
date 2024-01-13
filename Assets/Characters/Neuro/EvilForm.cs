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
        public float switchTransitionDuration = 0.5f;

        private int _evilLayer;
        private int _neuroLayer;
        private int _playerLayer;
        public bool enableSwitching = true;

        private bool _hintHidden;

        private void Awake()
        {
            _neuroSwitchParticleEffect = player.characterSwitchParticleEffect;

            _evilLayer = LayerMask.NameToLayer("EvilOnly");
            _neuroLayer = LayerMask.NameToLayer("NeuroOnly");
            _playerLayer = LayerMask.NameToLayer("Player");
            UpdateLayerCollision();
        }

        protected override bool CanSwap(bool toAlt) => enableSwitching && !player.dying;

        protected override void OnSwap()
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

            switcher.music.SetCharacter(player.playerType);

            UpdateLayerCollision();
        }

        private void UpdateLayerCollision()
        {
            Physics2D.IgnoreLayerCollision(_evilLayer, _playerLayer, !isAlt);
            Physics2D.IgnoreLayerCollision(_neuroLayer, _playerLayer, isAlt);
        }
    }
}
