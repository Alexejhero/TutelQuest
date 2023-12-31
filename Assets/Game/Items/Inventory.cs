using UnityEngine;

namespace SchizoQuest.Game.Items
{
    public class Inventory : MonoBehaviour
    {
        public Item item;
        public Transform socket;

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
            item = null;
            // TODO: set down on the ground nearby
            carryable.transform.SetParent(null, true);
            carryable.OnDropped();
        }
    }
}
