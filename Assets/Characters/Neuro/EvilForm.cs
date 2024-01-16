using SchizoQuest.Game;
using SchizoQuest.Transition_effects;
using UnityEngine;

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

        private HintType _activeSwapHint;

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
            if (_activeSwapHint != default)
            {
                HideActiveSwapHint();
                _activeSwapHint = default;
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

        public void ShowHint(HintType type)
        {
            if (type is HintType.NeuroF or HintType.NeuroF_2)
                _activeSwapHint = type;
            
            // already evil - ignore the hint
            if (isAlt) HideActiveSwapHint();
        }

        private void HideActiveSwapHint()
        {
            player.SendMessage("HideHint", _activeSwapHint, SendMessageOptions.DontRequireReceiver);
        }

        private void UpdateLayerCollision()
        {
            Physics2D.IgnoreLayerCollision(_evilLayer, _playerLayer, !isAlt);
            Physics2D.IgnoreLayerCollision(_neuroLayer, _playerLayer, isAlt);
        }
    }
}
