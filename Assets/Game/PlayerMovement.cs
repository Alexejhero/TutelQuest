using SchizoQuest.Input;
using UnityEngine;

namespace SchizoQuest.Game
{
    public sealed class PlayerMovement : MonoBehaviour
    {
        public Rigidbody2D rb;

        public float acceleration;
        public float runMultiplier = 1.5f;
        public float maxVelocity = 10f;

        public InputActions input;

        public void Awake()
        {
            input = new InputActions();
        }

        private Vector3 _accel;
        public void Update()
        {
            Vector2 moveInput = input.Player.Move.ReadValue<Vector2>();

            float maxVel = maxVelocity;
            bool running = input.Player.Run.IsPressed();
            if (running)
                maxVel *= runMultiplier;
            rb.velocity = Vector3.SmoothDamp(rb.velocity, moveInput * maxVel, ref _accel, 1f / acceleration);
        }
    }
}
