using System.Collections;
using System.Linq;
using SchizoQuest.Characters;
using SchizoQuest.Interaction;
using SchizoQuest.VFX.Particles;
using UnityEngine;

namespace SchizoQuest.Game.Items
{
    public sealed class Item : MonoBehaviour, IInteractable
    {
        public ItemType itemType;
        public Transform plug;
        public ParticleSystemManager effectManager;
        private int _effectSavedLayer;
        private Collider2D _collider;
        private SpriteRenderer[] _renderers;
        private int[] _savedLayers;
        private bool _isTrigger;

        private void Awake()
        {
            if (effectManager != null) { _effectSavedLayer = effectManager.SortingLayer; }
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
            int layerInFront = SortingLayer.NameToID("InFrontOfPlayer");
            _isTrigger = _collider.isTrigger;
            _collider.isTrigger = true;
            if (effectManager != null) { effectManager.SortingLayer = layerInFront; }
            foreach (SpriteRenderer rend in _renderers)
            {
                rend.sortingLayerID = layerInFront;
            }
        }

        public void OnDropped()
        {
            _collider.isTrigger = _isTrigger;
            if (effectManager != null) { effectManager.SortingLayer = _effectSavedLayer; }
            for (int i = 0; i < _renderers.Length; i++)
            {
                SpriteRenderer rend = _renderers[i];
                rend.sortingLayerID = _savedLayers[i];
            }
        }

        public void DiscardAfterUse(Player player)
        {
            if (player.inventory.item == this) { player.inventory.Drop(this); }

            StartCoroutine(r());
            IEnumerator r()
            {
                _collider.enabled = false;
                foreach (SpriteRenderer r in _renderers) 
                {
                    r.enabled = false;
                }

                if (effectManager != null)
                {
                    effectManager.SetExternalForces(false);
                    effectManager.StopAll(ParticleSystemStopBehavior.StopEmitting);
                    yield return new WaitUntil(() => !effectManager.IsPlaying());
                }
                Destroy(gameObject);
            }
        }
    }
}
