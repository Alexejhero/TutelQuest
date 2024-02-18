using SchizoQuest.Characters;
using SchizoQuest.Characters.Movement;
using UnityEngine;

namespace SchizoQuest.Movement
{
    [RequireComponent(typeof(Rigidbody2D))]
    public sealed class Noclip : MonoBehaviour
    {
        public Rigidbody2D rb;
        public Player player;
        public PlayerController controller;
        public float flySpeed;
        private void Start()
        {
            if (!Debug.isDebugBuild) Destroy(this);
        }
        private void OnEnable()
        {
            rb.isKinematic = true;
        }

        private void OnDisable()
        {
            rb.isKinematic = false;
        }

        private void Update()
        {
            if (player != Player.ActivePlayer) return;
            rb.velocity = Vector2.zero;
            Vector2 movement = flySpeed * Time.deltaTime * controller.MoveInput;
            transform.position += (Vector3)movement;
        }

        public void OnNoclip()
        {
            if (player != Player.ActivePlayer) return;
            enabled = !enabled;
        }
    }
}