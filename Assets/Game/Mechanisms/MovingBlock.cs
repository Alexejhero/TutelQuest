using FMODUnity;
using UnityEngine;
using UnityEngine.Serialization;

namespace SchizoQuest.Game.Mechanisms
{
    public class MovingBlock : Toggleable
    {
        public Vector3 offset;
        public float speed;
        [FormerlySerializedAs("scrapeSound")]
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
        private void Update()
        {
            transform.position = Vector3.SmoothDamp(transform.position, isOn ? _onPosition : _offPosition, ref _velocity, 1f/speed);
            if (makeSound && moveSound)
                moveSound.SetParameter("Speed", _velocity.magnitude);
        }

    }
}