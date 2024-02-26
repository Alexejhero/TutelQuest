using SchizoQuest.Audio;
using SchizoQuest.Game;
using SchizoQuest.VFX.Transition;
using UnityEngine;

namespace SchizoQuest.Characters.Neuro
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
            neuroEvilTransitionManager.NeuroTransform = transform;
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

            player.playerType = IsAlt
                ? PlayerType.Evil
                : PlayerType.Neuro;

            neuroEvilTransitionManager.Play(IsAlt, switchTransitionDuration);

            player.characterSwitchParticleEffect = IsAlt
                ? evilSwitchParticleEffect
                : _neuroSwitchParticleEffect;

            AudioSystem.SetCharacter(player.playerType);
            OnFormSwapRegistry.OnFormSwap(player.playerType, IsAlt);

            UpdateLayerCollision();
        }

        public void ShowHint(HintType type)
        {
            if (type is HintType.NeuroF or HintType.NeuroF_2)
                _activeSwapHint = type;

            // already evil - ignore the hint
            if (IsAlt) HideActiveSwapHint();
        }

        private void HideActiveSwapHint()
        {
            player.SendMessage("HideHint", _activeSwapHint, SendMessageOptions.DontRequireReceiver);
        }

        private void UpdateLayerCollision()
        {
            Physics2D.IgnoreLayerCollision(_evilLayer, _playerLayer, !IsAlt);
            Physics2D.IgnoreLayerCollision(_neuroLayer, _playerLayer, IsAlt);
        }
    }
}
