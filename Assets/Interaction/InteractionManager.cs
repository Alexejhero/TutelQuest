using System.Collections.Generic;
using System.Linq;
using SchizoQuest.Characters;
using UnityEngine;

namespace SchizoQuest.Interaction
{
    public class InteractionManager : MonoBehaviour
    {
        private readonly List<Collider2D> _collisionResults = new();

        public Player player;

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

            foreach (IInteractable interactable in ordered)
            {
                if (interactable is ICompoundInteractable compound)
                {
                    foreach (IInteractable other in ordered)
                    {
                        if (compound.GetOtherType().IsInstanceOfType(other) && compound.CanCompoundInteract(player, other))
                        {
                            compound.CompoundInteract(player, other);
                            return;
                        }
                    }
                }
            }

            ordered.FirstOrDefault()?.Interact(player);
        }
    }
}
