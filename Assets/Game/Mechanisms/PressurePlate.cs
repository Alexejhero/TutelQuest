using SchizoQuest.Characters;
using UnityEngine;

namespace SchizoQuest.Game.Mechanisms
{
    public class PressurePlate : Trigger<Player>
    {
        public Switch activates;
        private int playersOnPlate;
        private bool _isPressed;
        private const float _releaseDelay = 0.12f;
        private float _releaseDelayTimer;

        protected override void OnEnter(Player target) => playersOnPlate++;
        protected override void OnExit(Player target) => playersOnPlate--;

        private void FixedUpdate()
        {
            if (!_isPressed)
            {
                if (playersOnPlate > 0)
                {
                    _isPressed = true;
                    _releaseDelayTimer = 0;
                    activates.Toggle();
                }
            }
            else if (playersOnPlate == 0)
            {
                if (_releaseDelayTimer <= 0)
                {
                    _releaseDelayTimer = _releaseDelay;
                }
                else
                {
                    _releaseDelayTimer -= Time.fixedDeltaTime;
                    if (_releaseDelayTimer <= 0)
                    {
                        _isPressed = false;
                        activates.Toggle();
                    }
                }
            }
        }
    }
}
