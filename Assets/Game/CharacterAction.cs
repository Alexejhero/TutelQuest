using UnityEngine;

namespace SchizoQuest.Game
{
    public abstract class CharacterAction : MonoBehaviour
    {
        public enum Phase
        {
            Normal,
            ToAlternate,
            Alternate,
            ToNormal
        }
        public Phase phase;

        public abstract bool ShouldTransitionNext();
        public void TransitionNext()
        {
            Phase from = phase;
            switch (phase)
            {
                case Phase.Normal:
                    // start transition into alt form
                    break;
                case Phase.ToAlternate:
                    // handle the attack dealing damage
                    break;
                case Phase.Alternate:
                    // start transition back to regular form
                    break;
                case Phase.ToNormal:
                    // attack finished, start cooldown
                    nextAttackTime = Time.time + attackCooldown;
                    break;
            }
            phase = (Phase)((int)(phase + 1) % 4);
        }

        protected virtual void Update()
        {
            if (ShouldTransitionNext())
            {
                TransitionNext();

            }
        }

        protected abstract void OnTransition(Phase from);
    }
}
