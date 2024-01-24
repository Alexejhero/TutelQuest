using FMODUnity;
using SchizoQuest.Characters;
using SchizoQuest.Game.Items;
using SchizoQuest.Interaction;
using SchizoQuest.VFX;
using System.Collections;
using UnityEngine;

namespace SchizoQuest.Game.Mechanisms
{
    public class ItemDoor : MonoBehaviour, ICompoundInteractable<Item>
    {
        public DoorAnimator animator;
        public Collider2D coll;
        public StudioEventEmitter ambientSfx;

        [Space]
        public StudioEventEmitter sfxOnDestroy;

        public bool CanCompoundInteract(Player player, Item other)
        {
            return player.inventory.item == other && other.itemType == ItemType.Key;
        }

        public void CompoundInteract(Player player, Item other)
        {
            ambientSfx.Stop();
            sfxOnDestroy.Play();
            coll.enabled = false;

            DiscardAfterUse(player);
            other.DiscardAfterUse(player);
        }

        public void DiscardAfterUse(Player player)
        {
            StartCoroutine(r());
            IEnumerator r()
            {
                animator.PlayOpen();
                yield return new WaitUntil(() => animator.IsOpen);
                gameObject.SetActive(false);
            }
        }
    }
}
