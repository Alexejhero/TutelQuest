using UnityEngine;

namespace SchizoQuest.Characters.Vedal
{
    public class TutelAnimator : CharacterAnimator
    {
        public AnimationClip tutelSlideAnim;
        public TutelForm tutelForm;
        public bool slideFlipLeft;
        public bool slideFlipRight;

        private void Awake()
        {
            // only spin
            animationCoyoteTime = float.PositiveInfinity;
        }

        public override AnimationClip MoveLeftAnim => tutelForm.IsDashing ? tutelSlideAnim : base.MoveLeftAnim;
        public override AnimationClip MoveRightAnim => tutelForm.IsDashing ? tutelSlideAnim : base.MoveRightAnim;
        public override bool FlipLeftAnims => tutelForm.IsDashing ? slideFlipLeft : base.FlipLeftAnims;
        public override bool FlipRightAnims => tutelForm.IsDashing ? slideFlipRight : base.FlipRightAnims;
        public override Vector2 MoveInput => tutelForm.IsDashing ? tutelForm.rb.velocity : base.MoveInput;
    }
}
