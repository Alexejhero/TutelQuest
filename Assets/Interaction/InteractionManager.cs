using System.Collections.Generic;
using System.Linq;
using SchizoQuest.Game.Players;
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
            Physics2D.OverlapCircle(new Vector2(transform.position.x, transform.position.y), 5f, filter, _collisionResults);
            _collisionResults.OrderBy(coll => Vector2.Distance(transform.position, coll.transform.position))
                .Select(coll => coll.GetComponent<IInteractable>())
                .FirstOrDefault(comp => comp != null && comp.CanInteract(player))?
                .Interact(player);
        }
    }
}
