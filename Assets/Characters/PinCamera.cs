using System.Linq;
using SchizoQuest.Game;
using SchizoQuest.Game.Mechanisms;
using SchizoQuest.Helpers;
using UnityEngine;

namespace SchizoQuest.Characters
{
    public class PinCamera : Trigger<Player>
    {
        public Collider2D collider_;
        public Transform pinTarget;

        private CharacterSwitcher _switcher;
        private bool isPinned;

        private float camDistFactor;
        private int _playersInside;
        private void Start()
        {
            _switcher = CharacterSwitcher.Instance;
            float tanFov = Mathf.Tan(Camera.main.fieldOfView * Mathf.Deg2Rad * 0.5f);
            camDistFactor = 0.5f * 0.5625f / tanFov;
        }

        protected override void OnEnter(Player target)
        {
            _playersInside++;
            UpdatePin();
        }

        protected override void OnExit(Player target)
        {
            _playersInside--;
            UpdatePin();
        }

        private void UpdatePin()
        {
            if (_playersInside < _switcher.availablePlayers.Count)
            {
                Unpin();
                return;
            }
            if (!isPinned)
                Pin();
            else
                FrameAllPlayers();
        }

        public void Pin()
        {
            CameraController.Instance.target = pinTarget.transform;
            isPinned = true;
        }
        public void Unpin()
        {
            CameraController.Instance.target = Player.ActivePlayer.transform;
            isPinned = false;
            Vector3 camPos = Camera.main.transform.position;
            camPos.z = -20f;
            Camera.main.transform.position = camPos;
        }

        private void FrameAllPlayers()
        {
            if (_switcher.availablePlayers.Count == 0)
                return;

            Bounds bounds = new(_switcher.availablePlayers[0].transform.position, Vector3.zero);
            foreach (Player player in _switcher.availablePlayers.Skip(1))
            {
                bounds.Encapsulate(player.transform.position);
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