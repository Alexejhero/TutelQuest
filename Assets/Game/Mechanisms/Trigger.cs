using System.Collections.Generic;
using SchizoQuest.Helpers;
using UnityEngine;

namespace SchizoQuest.Game.Mechanisms
{
    public abstract class Trigger<TTarget> : MonoBehaviour
        where TTarget : Component
    {
        public float triggerInterval;
        private float nextTriggerTime;
        public bool ignoreTriggers;
        private List<Collider2D> _entered;

        protected virtual void Awake()
        {
            _entered = new List<Collider2D>(2);
        }

        public void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.isActiveAndEnabled) return;
            if (ignoreTriggers && other.isTrigger) return;
            TTarget target = other.GetComponentInParent<TTarget>();
            if (!target) return;

            if (Time.time < nextTriggerTime) return;
            nextTriggerTime = Time.time + triggerInterval;

            _entered.Add(other);
            OnEnter(target);
        }

        public void OnTriggerExit2D(Collider2D other)
        {
            if (ignoreTriggers && other.isTrigger) return;
            TTarget target = other.GetComponentInParent<TTarget>();
            if (!target) return;
            int idx = _entered.IndexOf(other);
            if (idx < 0) return;
            _entered.RemoveSwap(idx);

            OnExit(target);
        }

        protected virtual void OnEnter(TTarget target) {}
        protected virtual void OnExit(TTarget target) {}
    }
}
