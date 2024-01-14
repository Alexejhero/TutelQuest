using SchizoQuest.Characters;
using SchizoQuest.Game.Items;
using SchizoQuest.Interaction;
using System.Collections;
using UnityEngine;

namespace SchizoQuest.Game.Mechanisms
{
    public class ItemDoor : MonoBehaviour, ICompoundInteractable<Item>
    {
        public SpriteRenderer[] renderers;
        public Collider2D coll;
        public ParticleSystem effectIdle;
        public ParticleSystem effectOnDestroy;

        public bool CanCompoundInteract(Player player, Item other)
        {
            return player.inventory.item == other && other.itemType == ItemType.Key;
        }

        public void CompoundInteract(Player player, Item other)
        {
            effectOnDestroy.Play();
            renderers = GetComponentsInChildren<SpriteRenderer>();
            foreach (var r in renderers) 
            {
                r.enabled = false;
            }
            coll.enabled = false;

            DiscardAfterUse(player);
            other.DiscardAfterUse(player);
        }

        public void DiscardAfterUse(Player player)
        {
            StartCoroutine(r());
            IEnumerator r() 
            {
                effectIdle.Stop(true, ParticleSystemStopBehavior.StopEmitting);
                yield return new WaitUntil(() => !effectIdle.isPlaying);
                gameObject.SetActive(false);
            }
        }
    }
}
