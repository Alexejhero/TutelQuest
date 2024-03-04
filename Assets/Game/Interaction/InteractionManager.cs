using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using SchizoQuest.Characters;
using UnityEngine;

namespace SchizoQuest.Game.Interaction
{
    public class InteractionManager : MonoBehaviour
    {
        private readonly List<Collider2D> _collisionResults = new();

        public Player player;

        [UsedImplicitly]
        public void OnInteract()
        {
            if (!player.enabled) return;

            ContactFilter2D filter = new()
            {
                layerMask = Physics2D.AllLayers,
                useLayerMask = true,
                useTriggers = true
            };
            Physics2D.OverlapCircle(new Vector2(transform.position.x, transform.position.y), 3f, filter, _collisionResults);

            Debug.Log($"Found {_collisionResults.Count} colliders");

            IEnumerable<IInteractable> ordered = _collisionResults
                .OrderBy(coll => Vector2.Distance(transform.position, coll.transform.position))
                .Select(coll => coll.GetComponent<IInteractable>())
                .Where(comp => comp != null && comp.CanInteract(player))
                .ToList();

            Debug.Log($"Found {ordered.Count()} usable interactables");

            ordered.FirstOrDefault()?.Interact(player);
        }
    }
}
