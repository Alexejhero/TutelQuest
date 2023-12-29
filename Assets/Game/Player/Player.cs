using TarodevController;
using UnityEngine;
using UnityEngine.Serialization;

namespace SchizoQuest.Game.Player
{
    public sealed class Player : MonoBehaviour
    {
        public static Player ActivePlayer;

        public Character character;
        public Living living;
        [FormerlySerializedAs("movement")] public PlayerController controller;

        public void OnEnable()
        {
            Camera.main!.GetComponent<FollowTransform>().target = transform;
            controller.movementActive = true;
            GetComponent<SpriteRenderer>().sortingOrder = 1;
            ActivePlayer = this;
        }

        public void OnDisable()
        {
            controller.movementActive = false;
            controller.
            GetComponent<SpriteRenderer>().sortingOrder = -1;
        }
    }
}
