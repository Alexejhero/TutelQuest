using System.Linq;
using SchizoQuest.Game.Players;
using SchizoQuest.Interaction;
using UnityEngine;

namespace SchizoQuest.Game.Items
{
    public sealed class Carryable : MonoBehaviour, IInteractable
    {
        public ItemType itemType;
        public Transform plug;
        private Collider2D _collider;
        private SpriteRenderer[] _renderers;
        private int[] _savedLayers;
        private bool _isTrigger;

        private void Awake()
        {
            _collider = GetComponent<Collider2D>();
            _renderers = GetComponentsInChildren<SpriteRenderer>();
            _savedLayers = _renderers
                .Select(rend => rend.sortingLayerID)
                .ToArray();
        }

        public bool CanInteract(Player player)
        {
            return player.inventory && (!player.inventory.item || player.inventory.item == this);
        }

        public void Interact(Player player)
        {
            if (player.inventory.item == this)
            {
                player.inventory.Drop(this);
                return;
            }

            player.inventory.Pickup(this);
        }

        public void OnPickedUp()
        {
            _isTrigger = _collider.isTrigger;
            _collider.isTrigger = true;
            foreach (SpriteRenderer rend in _renderers)
            {
                rend.sortingLayerName = "InFrontOfPlayer";
            }
        }

        public void OnDropped()
        {
            _collider.isTrigger = _isTrigger;
            for (int i = 0; i < _renderers.Length; i++)
            {
                SpriteRenderer rend = _renderers[i];
                rend.sortingLayerID = _savedLayers[i];
            }
        }
    }
}
