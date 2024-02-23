using FMODUnity;
using UnityEngine;

namespace SchizoQuest.Game.Mechanisms
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class MovingBlock : Toggleable
    {
        public Rigidbody2D rb;
        public Vector3 offset;
        public float speed;
        public StudioEventEmitter moveSound;
        public bool makeSound;
        [Tooltip("Raising this makes the move sound louder at lower speeds. Suitable for \"slow and heavy\" moving blocks.")]
        public float soundSpeedMulti = 1f;
        private Vector3 _offPosition;
        private Vector3 _onPosition;

        public float moveToOffPositionDelay = 0;
        private float _currentMoveToOffPositionDelay;

        private void Awake()
        {
            _offPosition = transform.position;
            _onPosition = _offPosition + offset;
        }

        private Vector3 _velocity;
        private void FixedUpdate()
        {
            bool isOnTarget = isOn;

            if (!isOn)
            {
                if (_currentMoveToOffPositionDelay > 0)
                {
                    _currentMoveToOffPositionDelay -= Time.fixedDeltaTime;
                    _velocity = Vector3.zero;
                    isOnTarget = true;
                }
            }
            else
            {
                float multiplier = Vector3.Distance(transform.position, _offPosition) / Vector3.Distance(_offPosition, _onPosition);
                _currentMoveToOffPositionDelay = moveToOffPositionDelay * multiplier;
            }

            Vector3.SmoothDamp(transform.position, isOnTarget ? _onPosition : _offPosition, ref _velocity, 1f/speed);
            rb.velocity = _velocity;
            if (makeSound && moveSound)
                moveSound.SetParameter("Speed", _velocity.magnitude * soundSpeedMulti);
        }
    }
}
