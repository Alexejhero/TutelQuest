using UnityEngine;

namespace SchizoQuest.Game.Mechanisms
{
    public abstract class Trigger<TTarget> : MonoBehaviour
        where TTarget : Component
    {
        public float triggerInterval;
        private float nextTriggerTime;
    
        public void OnTriggerEnter2D(Collider2D other)
        {
            TTarget target = other.GetComponent<TTarget>();
            if (!target) return;

            if (Time.time < nextTriggerTime) return;
            nextTriggerTime = Time.time + triggerInterval;
            
            OnEnter(target);
        }

        public void OnTriggerExit2D(Collider2D other)
        {
            TTarget target = other.GetComponent<TTarget>();
            if (!target) return;

            OnExit(target);
        }

        protected abstract void OnEnter(TTarget target);
        protected abstract void OnExit(TTarget target);
    }
}
