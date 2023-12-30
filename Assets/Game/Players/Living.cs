using System;
using UnityEngine;

namespace SchizoQuest.Game.Players
{
    public class Living : MonoBehaviour
    {
        public float health;
        public float maxHealth = 100f;
        public event Action OnDeath;

        protected virtual void Awake()
        {
            ResetHealth();
        }

        public void ResetHealth()
        {
            health = maxHealth;
        }

        public void Heal(float amount, bool clamp = true)
        {
            health = clamp
                ? health + amount
                : Mathf.Min(maxHealth, health + amount);
        }

        public void TakeDamage(UnityEngine.Object source, float damage)
        {
            if (Mathf.Approximately(damage, 0)) return;

            health -= damage;
            Debug.LogWarning($"{source.name} damaged {name} for {damage} ({health} health remaining)");
            if (!IsAlive())
            {
                Debug.LogWarning($"{name} died");
                OnDeath?.Invoke();
            }
        }

        public bool IsAlive() => health > 0;
    }
}