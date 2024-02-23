using System.Linq;
using SchizoQuest.Characters;
using SchizoQuest.Characters.Movement;
using SchizoQuest.Helpers;
using UnityEngine;

namespace SchizoQuest.Game.Items
{
    public class Inventory : MonoBehaviour
    {
        public Item item;
        public Transform socket;
        public GroundTracker groundTracker;

        private ContactFilter2D _filter;
        private RaycastHit2D[] _raycasts;

        private bool _hintHidden;

        private int _playerLayer;
        private LayerMask _notPlayer;
        private void Awake()
        {
            this.EnsureComponent(ref groundTracker);
            _raycasts = new RaycastHit2D[1];
            _playerLayer = LayerMask.NameToLayer("Player");
            _notPlayer = ~(1 << _playerLayer);
            _filter = new ContactFilter2D()
            {
                layerMask = _notPlayer,
                useLayerMask = true,
                useTriggers = false,
            };
        }

        public void Pickup(Item carryable)
        {
            if (!_hintHidden)
            {
                _hintHidden = true;
                Player.ActivePlayer.SendMessage("HideHint", HintType.E, SendMessageOptions.DontRequireReceiver);
            }

            item = carryable;
            Transform target = carryable.transform;
            target.SetParent(socket, true);

            // move the target transform so the plug is on the socket
            Transform plug = carryable.plug;
            Vector2 offset = plug
                ? socket.InverseTransformPoint(plug.position)
                : Vector2.zero;
            target.localPosition = -offset;

            carryable.OnPickedUp();
        }

        public void Drop(Item carryable)
        {
            if (!groundTracker.IsRecentlyGrounded) return;
            
            // prevent placing on objects out of phase (e.g. neuro block while evil & vice versa)
            _filter.SetLayerMask(_notPlayer & Physics2D.GetLayerCollisionMask(_playerLayer));

            // drop at the player's feet
            Vector2 raycastStart = transform.position;
            raycastStart.y = GetComponentsInChildren<Collider2D>()
                .Where(t => !t.isTrigger)
                .Encapsulate(transform.position)
                .min.y + 0.1f; // safety offset
            
            int rays = Physics2D.Raycast(raycastStart, Vector2.down, _filter, _raycasts, 3f);
            if (rays == 0) return; // can happen if the center of the player capsule is over the edge (or in coyote time)
            
            RaycastHit2D rc = _raycasts[0];

            // items need to move if placed on moving blocks
            // possible edge case if one player drops the item on another but we don't let it happen anyway (players touching wins the game)
            DetachItem(rc.transform);

            Debug.Log($"Drop raycast {rc.collider} {rc.point}");
            Vector2 dropPoint = rc.point;
            dropPoint.y += 0.5f * carryable.GetComponents<Collider2D>().Encapsulate().size.y;
            carryable.transform.position = dropPoint;
            carryable.OnDropped();
        }

        public void DetachItem(Transform parentTo = null)
        {
            if (!item) return;
            item.transform.SetParent(parentTo, true);
            item = null;
        }
    }
}
