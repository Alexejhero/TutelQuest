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
        [Tooltip("This is a multiplier applied to the move speed, which controls sound volume. Raise this for \"slow and heavy\" moving blocks.")]
        public float soundSpeedMulti = 1f;
        private Vector3 _offPosition;
        private Vector3 _onPosition;
        private void Awake()
        {
            _offPosition = transform.position;
            _onPosition = _offPosition + offset;
        }

        private Vector3 _velocity;
        private void FixedUpdate()
        {
            Vector3.SmoothDamp(transform.position, isOn ? _onPosition : _offPosition, ref _velocity, 1f/speed);
            rb.velocity = _velocity;
            if (makeSound && moveSound)
                moveSound.SetParameter("Speed", _velocity.magnitude * soundSpeedMulti);
        }
    }
}