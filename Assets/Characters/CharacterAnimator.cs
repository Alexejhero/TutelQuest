using System;
using PowerTools;
using TarodevController;
using UnityEngine;

namespace SchizoQuest.Characters
{
    public class CharacterAnimator : MonoBehaviour
    {
        public Rigidbody2D rb;
        public SpriteAnim animator;
        public AnimationClip idleFrontAnim;
        public AnimationClip idleLeftAnim;
        public AnimationClip idleRightAnim;
        public AnimationClip moveLeftAnim;
        public AnimationClip moveRightAnim;
        public bool flipLeftAnims;
        public bool flipRightAnims;
        public PlayerController playerController;

        public virtual AnimationClip IdleFrontAnim => idleFrontAnim;
        public virtual AnimationClip IdleLeftAnim => idleLeftAnim;
        public virtual AnimationClip IdleRightAnim => idleRightAnim;
        public virtual AnimationClip MoveLeftAnim => moveLeftAnim;
        public virtual AnimationClip MoveRightAnim => moveRightAnim;
        public virtual bool FlipLeftAnims => flipLeftAnims;
        public virtual bool FlipRightAnims => flipRightAnims;

        private bool _flipped;
        private int _lastMoveDirection = 0;
        private AnimationClip _currentClip;

        protected AnimationClip CurrentClip
        {
            get => _currentClip;
            set
            {
                if (_currentClip == value) return;
                _currentClip = value;
                animator.Play(_currentClip);
            }
        }

        protected virtual void Update()
        {
            Vector2 velocity = rb.velocity;
            if (Math.Abs(velocity.x) < 0.1 && Math.Abs(velocity.y) < 0.1)
            {
                switch (_lastMoveDirection)
                {
                    case 0:
                        CurrentClip = IdleFrontAnim;
                        _flipped = false;
                        break;

                    case < 0:
                        CurrentClip = IdleLeftAnim;
                        DoFlip(FlipLeftAnims);
                        break;

                    case > 0:
                        CurrentClip = IdleRightAnim;
                        DoFlip(FlipRightAnims);
                        break;
                }
            }
            else
            {
                if (velocity.x < 0)
                {
                    CurrentClip = playerController._grounded ? MoveLeftAnim : IdleLeftAnim;
                    _lastMoveDirection = -1;
                    DoFlip(FlipLeftAnims);
                }
                else
                {
                    CurrentClip = playerController._grounded ? MoveRightAnim : IdleRightAnim;
                    _lastMoveDirection = 1;
                    DoFlip(FlipRightAnims);
                }
            }
        }

        protected void DoFlip(bool check)
        {
            if (check != _flipped)
            {
                _flipped = !_flipped;
                animator.transform.localScale = new Vector3(-animator.transform.localScale.x, animator.transform.localScale.y, animator.transform.localScale.z);
            }
        }
    }
}
