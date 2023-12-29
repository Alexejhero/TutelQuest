using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SchizoQuest.Game
{
    public class Gymbag : MonoBehaviour
    {
        // TODO more inventory slots
        public Carryable item;
        public Transform socket;

        private void Awake()
        {
            _collisionResults = new List<Collider2D>();
        }

        public void Pickup(Carryable carryable)
        {
            item = carryable;
            Transform attachPoint = carryable.plug ? carryable.plug : carryable.transform;
            attachPoint.SetParent(socket, false);
            attachPoint.localPosition = Vector3.zero;
            carryable.OnPickedUp();
        }
        public void Drop(Carryable carryable)
        {
            item = null;
            // TODO: set down on the ground nearby
            carryable.transform.SetParent(null, true);
            carryable.OnDropped();
        }

        private List<Collider2D> _collisionResults;

        public void OnInteract()
        {
            if (!GetComponent<Player>().enabled) return;
            if (item)
            {
                Drop(item);
                return;
            }
            ContactFilter2D filter = new()
            {
                layerMask = Physics2D.AllLayers,
                useLayerMask = true
            };
            Physics2D.OverlapCircle(new Vector2(transform.position.x, transform.position.y), 5f, filter, _collisionResults);
            Carryable carryable = _collisionResults
                .Select(coll => coll.GetComponent<Carryable>())
                .FirstOrDefault(comp => comp);

            if (!carryable) return;
            Pickup(carryable);
        }
    }
}
