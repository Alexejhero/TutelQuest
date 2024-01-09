using System;
using UnityEngine;

namespace SchizoQuest.Characters.Movement
{
    [CreateAssetMenu]
    public class Stats : ScriptableObject
    {
        [Header("Horizontal Movement")]
        
        [Min(0), Tooltip("Beyond this value, the controller will not add any speed")]
        public float maxHorizontalSpeed;
        [Min(0), Tooltip("Horizontal acceleration when grounded.")]
        public float groundAcceleration;
        [Min(0), Tooltip("Deceleration when no input is pressed.")]
        public float idleDeceleration;
        [Min(0), Tooltip("Multiplier to idle deceleration when switching movement direction.")]
        public float turnDecelerationMulti;
        [Min(0), Tooltip("Multiplier to apply to horizontal acceleration when in the air. Affects turn deceleration as well")]
        public float airAccelerationMulti;
        
        [Header("Jumping")]
        
        [Min(0)]
        public float peakHeight;
        [Min(0), Tooltip("The time it takes to reach the peak of the jump")]
        public float timeToPeak;
        [Min(0), Tooltip("Total number of mid-air jumps allowed.")]
        public int extraJumps;
        [Min(0), Tooltip("Maximum fall speed.")]
        public float terminalVelocity;
        [Min(0), Tooltip("Gravity multiplier applied while rising during a jump.")]
        public float jumpUpwardsGravityMulti;
        [Min(0), Tooltip("Gravity multiplier applied if the jump input is released early.")]
        public float earlyCutoffGravityMulti;
        [Min(0), Tooltip("Gravity multiplier applied while falling")]
        public float fallGravityMulti;
        [Tooltip("Whether to reset velocity before applying air jump impulse if falling. Coyote time always resets.")]
        public bool airJumpOverrideFallVelocity = true;
        [Tooltip("Whether to reset velocity before applying air jump impulse if rising.")]
        public bool airJumpOverrideRiseVelocity = true;

        [Header("Input Buffering")]

        [Min(0), Tooltip("Allows to jump mid-air after leaving the ground if the input was pressed late within this time window (in seconds)")]
        public float coyoteTime;
        [Min(0), Tooltip("Allows to jump immediately upon touching the ground if the input was pressed early within this time window (in seconds)")]
        public float bunnyhopBuffer;
    }
}