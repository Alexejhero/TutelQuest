using UnityEngine;

namespace SchizoQuest.Game
{
    public class Living : MonoBehaviour
    {
        public float health;
        public float maxHealth = 100f;

        protected virtual void Awake()
        {
            ResetHealth();
        }

        private void Update()
        {
            if (!IsAlive())
            {
                Debug.LogWarning($"{name} died");
                ResetHealth();
            }
        }

        private void ResetHealth()
        {
            health = maxHealth;
        }

        public bool IsAlive() => health > 0;
    }
}