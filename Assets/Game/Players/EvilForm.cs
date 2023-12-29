using UnityEngine;

namespace SchizoQuest.Game.Players
{
    public sealed class EvilForm : CharacterAltForm
    {
        public ParticleSystem evilSwitchParticleEffect;
        private ParticleSystem _neuroSwitchParticleEffect;

        public void Start()
        {
            _neuroSwitchParticleEffect = player.characterSwitchParticleEffect;
        }
        protected override void OnSwap(bool isAlt)
        {
            player.playerType = isAlt
                ? PlayerType.Evil
                : PlayerType.Neuro;
            player.characterSwitchParticleEffect = isAlt
                ? evilSwitchParticleEffect
                : _neuroSwitchParticleEffect;
        }
    }
}