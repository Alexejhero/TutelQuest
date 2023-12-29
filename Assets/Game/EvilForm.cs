using UnityEngine;

namespace SchizoQuest.Game
{
    public sealed class EvilForm : CharacterAction
    {
        public bool isEvil;

        public float attackCooldown;
        private float nextAttackTime;

        public override bool ShouldTransitionNext()
        {
            return phase switch
            {
                Phase.Normal => Time.time > nextAttackTime,
                Phase.ToAlternate => true, // TODO wait for animation to finish
                Phase.Alternate => true,
                Phase.ToNormal => true,
                _ => throw new System.InvalidOperationException(),
            };
        }
    }
}