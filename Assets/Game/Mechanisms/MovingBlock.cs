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
                moveSound.SetParameter("Speed", rb.velocity.magnitude);
        }
    }
}