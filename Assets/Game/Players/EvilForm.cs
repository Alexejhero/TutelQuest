using FMODUnity;
using UnityEngine;

namespace SchizoQuest.Game.Players
{
    public sealed class EvilForm : CharacterAltForm
    {
        public ParticleSystem evilSwitchParticleEffect;
        private ParticleSystem _neuroSwitchParticleEffect;
        private CharacterSwitcher switcher;
        public NeuroEvilTransitionManager neuroEvilTransitionManager;
        public float switchTransitionDuration = 0.5f;

        public void Awake()
        {
            switcher = gameObject.GetComponentInParent<CharacterSwitcher>();
            _neuroSwitchParticleEffect = player.characterSwitchParticleEffect;
        }

        protected override void OnSwap(bool isAlt)
        {
            player.playerType = isAlt
                ? PlayerType.Evil
                : PlayerType.Neuro;

            neuroEvilTransitionManager.isEvil = isAlt;
            neuroEvilTransitionManager.Play(switchTransitionDuration);

            player.characterSwitchParticleEffect = isAlt
                ? evilSwitchParticleEffect
                : _neuroSwitchParticleEffect;

            player.gameObject.layer = LayerMask.NameToLayer(player.playerType.ToString());
            switcher.music.SetParameter("Character", isAlt ? 2 : 1);
        }
    }
}
