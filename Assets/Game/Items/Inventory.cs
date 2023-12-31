using SchizoQuest.Characters;
using UnityEngine;

namespace SchizoQuest.Game.Items
{
    public class Inventory : MonoBehaviour
    {
        public Item item;
        public Transform socket;

        private ContactFilter2D _filter;
        private RaycastHit2D[] _raycasts;
        private void Awake()
        {
            _raycasts = new RaycastHit2D[1];
            _filter = new ContactFilter2D()
            {
                layerMask = ~(1 << LayerMask.NameToLayer("Player")),
                useLayerMask = true,
                useTriggers = false,
            };
        }

        public void Pickup(Item carryable)
        {
            item = carryable;
            Transform attachPoint = carryable.plug ? carryable.plug : carryable.transform;
            attachPoint.SetParent(socket, false);
            attachPoint.localPosition = Vector3.zero;
            carryable.OnPickedUp();
        }
        public void Drop(Item carryable)
        {
            if (!Player.ActivePlayer.controller._grounded) return;
            // drop at the player's feet
            int rays = Physics2D.Raycast(transform.position, Vector2.down, _filter, _raycasts, 3f);
            if (rays == 0)
            {
                Debug.LogWarning("Drop raycast hit nothing even though we're grounded");
                return;
            }
            
            DetachItem();
            RaycastHit2D rc = _raycasts[0];
            Debug.Log($"Drop raycast {rc.collider} {rc.point}");
            Vector2 dropPoint = rc.point;
            dropPoint.y += 0.5f * carryable.GetComponent<Collider2D>().bounds.size.y;
            carryable.transform.position = dropPoint;
            carryable.OnDropped();
        }

        public void DetachItem()
        {
            if (!item) return;
            item.transform.SetParent(null, true);
            item = null;
        }
    }
}
