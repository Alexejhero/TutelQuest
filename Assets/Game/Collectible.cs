using UnityEngine;

namespace SchizoQuest.Game
{
    public sealed class Collectible : MonoBehaviour
    {
        public Character collectibleBy;
        public void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log($"entered {other}");
            var player = other.GetComponent<Player>();
            if (!player) return;
            if (collectibleBy.HasFlag(player.character))
            {
                OnCollected?.Invoke(this, player);
                gameObject.SetActive(false);
            }
        }

        public delegate void CollectedHandler(Collectible collectible, Player player);
        public static CollectedHandler OnCollected;
    }
}