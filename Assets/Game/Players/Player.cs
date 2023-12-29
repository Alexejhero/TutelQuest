using SchizoQuest.Game.Items;
using TarodevController;
using UnityEngine;
using UnityEngine.Serialization;

namespace SchizoQuest.Game.Players
{
    public sealed class Player : MonoBehaviour
    {
        public static Player ActivePlayer;

        [FormerlySerializedAs("character")] public PlayerType playerType;
        public Living living;
        [FormerlySerializedAs("movement")] public PlayerController controller;
        public Inventory inventory;
        public ParticleSystem characterSwitchParticleEffect;

        public void OnEnable()
        {
            Camera.main!.GetComponent<FollowTransform>().target = transform;
            controller.movementActive = true;
            GetComponent<SpriteRenderer>().sortingOrder = 1;
            ActivePlayer = this;
            characterSwitchParticleEffect.Play();
        }

        public void OnDisable()
        {
            characterSwitchParticleEffect.Stop();
            characterSwitchParticleEffect.Clear();
            controller.movementActive = false;
            controller.
            GetComponent<SpriteRenderer>().sortingOrder = -1;
        }
    }
}
