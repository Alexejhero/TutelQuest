using FMODUnity;
using SchizoQuest.Helpers;
using UnityEngine;
using SchizoQuest.Game.Mechanisms;
using SchizoQuest.Audio;

namespace SchizoQuest.Characters
{
    public class SetMusic : Trigger<Player>
    {
        public Collider2D collider_;
        private static BackgroundMusic _music;
        public StudioEventEmitter switchTo;
        private StudioEventEmitter _switchedFrom;

        [Tooltip("How many players must be inside the collider to trigger the music swap.")]
        [Min(1)]
        public int requiredPlayers = 1;
        private int _playersInCollider;
        [Tooltip("If true, when the number of players inside falls below the requirement, the music is swapped back")]
        public bool revertOnExit = false;
        private bool _switched;

        private void Start()
        {
            _music = MonoSingleton<BackgroundMusic>.Instance;
        }

        protected override void OnEnter(Player target) => _playersInCollider++;
        protected override void OnExit(Player target) => _playersInCollider--;

        private void FixedUpdate()
        {
            if (!_switched)
            {
                if (_playersInCollider >= requiredPlayers)
                {
                    _switchedFrom = _music.GetTrack();
                    _music.SetTrack(switchTo);
                    _switched = true;
                }
            }
            else
            {
                if (revertOnExit && _playersInCollider < requiredPlayers)
                {
                    _music.SetTrack(_switchedFrom);
                    _switchedFrom = null;
                    _switched = false;
                }
            }
        }
    }
}