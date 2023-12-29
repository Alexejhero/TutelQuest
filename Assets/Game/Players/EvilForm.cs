using FMODUnity;
using UnityEngine;

namespace SchizoQuest.Game.Players
{
    public sealed class EvilForm : CharacterAltForm
    {
        public ParticleSystem evilSwitchParticleEffect;
        private ParticleSystem _neuroSwitchParticleEffect;
        private CharacterSwitcher switcher;

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
            player.characterSwitchParticleEffect = isAlt
                ? evilSwitchParticleEffect
                : _neuroSwitchParticleEffect;
            switcher.music.SetParameter("Character", isAlt ? 2 : 1);
        }
    }
}