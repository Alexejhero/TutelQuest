using FMODUnity;
using SchizoQuest.Characters;
using SchizoQuest.Game.Items;
using SchizoQuest.Interaction;
using System.Collections;
using SchizoQuest.VFX.Materials.Door;
using UnityEngine;
using PowerTools;

namespace SchizoQuest.Game.Mechanisms
{
    public class ItemDoor : MonoBehaviour, ICompoundInteractable<Item>
    {
        public GameObject doorObject;
        public Collider2D doorCollider;
        public DoorAnimator doorAnimator;
        [Space]
        public SpriteAnim consoleAnim;
        public AnimationClip consoleOpen;
        [Space]
        public StudioEventEmitter ambientSfx;

        public StudioEventEmitter sfxOnDestroy;

        public bool CanCompoundInteract(Player player, Item other)
        {
            return other && player.inventory.item == other && other.itemType == ItemType.Key;
        }

        public void CompoundInteract(Player player, Item other)
        {
            DiscardAfterUse(player);
            other.DiscardAfterUse(player);
        }

        public void DiscardAfterUse(Player player)
        {
            StartCoroutine(r());
            IEnumerator r()
            {
                ambientSfx.Stop();
                sfxOnDestroy.Play();

                doorCollider.enabled = false;
                doorAnimator.PlayOpen();
                if (consoleAnim) consoleAnim.Play(consoleOpen);
                yield return new WaitUntil(() => doorAnimator.IsOpen);
                doorObject.SetActive(false);
            }
        }
    }
}
