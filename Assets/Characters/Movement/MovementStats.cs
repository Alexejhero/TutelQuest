using UnityEngine;

namespace SchizoQuest.Characters.Movement
{
    [CreateAssetMenu]
    public class MovementStats : ScriptableObject
    {
        [Header("Horizontal Movement")]
        
        [Min(0), Tooltip("Beyond this value, the controller will not add any speed")]
        public float maxHorizontalSpeed;
        [Min(0), Tooltip("Horizontal acceleration when grounded.")]
        public float groundAcceleration;
        [Min(0), Tooltip("Deceleration when no input is pressed.")]
        public float idleDeceleration;
        [Min(0), Tooltip("Multiplier to idle deceleration when switching movement direction. Applies mid-air as well")]
        public float turnDecelerationMulti;
        [Min(0), Tooltip("Horizontal acceleration when in the air.")]
        public float airAcceleration;
        [Tooltip("Idle deceleration applied in the air. Improves air control. Unaffected by the air acceleration multiplier.")]
        public float idleAirDeceleration;
        
        [Header("Jumping")]
        
        [Min(0)]
        public float peakHeight;
        [Min(0), Tooltip("The time it takes to reach the peak of the jump. This also affects falling gravity to keep things feeling consistent.")]
        public float timeToPeak;
        [Min(0), Tooltip("Total number of mid-air jumps allowed.")]
        public int extraJumps;
        [Min(0), Tooltip("Gravity multiplier applied if the jump input is released early.")]
        public float earlyCutoffGravityMulti;
        [Tooltip("Whether to reset velocity before applying jump impulse if falling. Coyote time always resets.")]
        public bool resetFallVelocity = true;
        [Tooltip("Whether to reset velocity before applying jump impulse if rising.")]
        public bool resetRiseVelocity = true;

        [Header("Input Buffering")]

        // pretend there's an infobox here saying "Coyote time is set on the Ground Tracker"
        [Min(0), Tooltip("Executes a jump immediately upon touching the ground if the input was pressed early within this time window (in seconds)")]
        public float bunnyhopBuffer;
    }
}