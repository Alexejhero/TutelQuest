using SchizoQuest.Game;
using SchizoQuest.Helpers;
using UnityEngine;

namespace SchizoQuest.Characters
{
    public class PinCamera : MonoBehaviour
    {
        public Collider2D collider_;
        public Transform pinTarget;

        private CharacterSwitcher _switcher;
        private bool isPinned;

        private float camDistFactor;
        private void Start()
        {
            _switcher = MonoSingleton<CharacterSwitcher>.Instance;
            float tanFov = Mathf.Tan(Camera.main.fieldOfView * Mathf.Deg2Rad * 0.5f);
            camDistFactor = 0.5f * 0.5625f / tanFov;
        }

        public void FixedUpdate()
        {
            foreach (var player in _switcher.availablePlayers)
            {
                if (!Physics2D.IsTouching(player.controller.collider_, collider_))
                {
                    Unpin();
                    return;
                }
            }
            if (!isPinned)
                Pin();
            else
                FrameBothPlayers();
        }

        public void Pin()
        {
            MonoSingleton<CameraController>.Instance.target = pinTarget.transform;
            isPinned = true;
        }
        public void Unpin()
        {
            MonoSingleton<CameraController>.Instance.target = Player.ActivePlayer.transform;
            isPinned = false;
            Vector3 camPos = Camera.main.transform.position;
            camPos.z = -20f;
            Camera.main.transform.position = camPos;
        }

        private void FrameBothPlayers()
        {
            Bounds bounds = new(Player.ActivePlayer.transform.position, Vector3.zero);

            foreach (var player in _switcher.availablePlayers)
            {
                bounds.Encapsulate(player.controller.collider_.bounds);
            }

            Vector3 center = bounds.center;
            pinTarget.position = center;

            float distance = bounds.size.x;
            float camDistance = distance * camDistFactor;
            Vector3 camPos = Camera.main.transform.position;
            camPos.z = -camDistance;
            Camera.main.transform.position = camPos;
        }
    }
}