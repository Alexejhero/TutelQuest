using UnityEngine;

namespace SchizoQuest.Game
{
    public sealed class Collectible : MonoBehaviour
    {
        public Character collectibleBy;
        public void OnTriggerEnter2D(Collider2D other)
        {
            TryCollect(other.gameObject);
        }

        public void OnCollisionEnter2D(Collision2D other)
        {
            TryCollect(other.gameObject);
        }

        private bool TryCollect(GameObject collector)
        {
            Player.Player player = collector.GetComponent<Player.Player>();
            if (!player) return false;
            if (!collectibleBy.HasFlag(player.character)) return false;

            OnCollected?.Invoke(this, player);
            gameObject.SetActive(false);
            return true;
        }

        public delegate void CollectedHandler(Collectible collectible, Player.Player player);
        public static CollectedHandler OnCollected;
    }
}
