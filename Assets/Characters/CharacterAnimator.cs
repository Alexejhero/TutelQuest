using System;
using PowerTools;
using SchizoQuest.Characters.Movement;
using SchizoQuest.Game;
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
        public PlayerController controller;
        [Tooltip("Time to keep playing the moving animation after the player leaves the ground")]
        public float animationCoyoteTime = 0.1f;
        private float _animationCoyoteTimer;

        public virtual AnimationClip IdleFrontAnim => idleFrontAnim;
        public virtual AnimationClip IdleLeftAnim => idleLeftAnim;
        public virtual AnimationClip IdleRightAnim => idleRightAnim;
        public virtual AnimationClip MoveLeftAnim => moveLeftAnim;
        public virtual AnimationClip MoveRightAnim => moveRightAnim;
        public virtual bool FlipLeftAnims => flipLeftAnims;
        public virtual bool FlipRightAnims => flipRightAnims;
        public virtual Vector2 MoveInput => controller.MoveInput;

        private float _originalScaleX;
        private int _lastMoveDirection = 0;
        private AnimationClip _currentClip;

        private bool _hintHidden;

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

        private void Start()
        {
            _originalScaleX = animator.transform.localScale.x;
        }

        protected virtual void Update()
        {
            Vector2 velocity = rb.velocity;
            Vector2 input = MoveInput;
            if (Math.Abs(velocity.x) < 1 && Math.Abs(velocity.y) < 1)
            {
                if (input.y < 0) _lastMoveDirection = 0;

                switch (_lastMoveDirection)
                {
                    case 0:
                        CurrentClip = IdleFrontAnim;
                        SetFlip(false);
                        break;

                    case < 0:
                        CurrentClip = IdleLeftAnim;
                        SetFlip(FlipLeftAnims);
                        break;

                    case > 0:
                        CurrentClip = IdleRightAnim;
                        SetFlip(FlipRightAnims);
                        break;
                }
            }
            else
            {
                if (input.x < 0)
                {
                    if (!_hintHidden)
                    {
                        _hintHidden = true;
                        controller.SendMessage("HideHint", HintType.WASD, SendMessageOptions.DontRequireReceiver);
                    }

                    CurrentClip = _animationCoyoteTimer < animationCoyoteTime ? MoveLeftAnim : IdleLeftAnim;
                    _lastMoveDirection = -1;
                    SetFlip(FlipLeftAnims);
                }
                else if (input.x > 0)
                {
                    if (!_hintHidden)
                    {
                        _hintHidden = true;
                        controller.SendMessage("HideHint", HintType.WASD, SendMessageOptions.DontRequireReceiver);
                    }

                    CurrentClip = _animationCoyoteTimer < animationCoyoteTime ? MoveRightAnim : IdleRightAnim;
                    _lastMoveDirection = 1;
                    SetFlip(FlipRightAnims);
                }
            }

            if (controller.groundTracker.isOnGround)
                _animationCoyoteTimer = 0;
            else
                _animationCoyoteTimer += Time.deltaTime;
        }

        protected void SetFlip(bool flip)
        {
            animator.transform.localScale = new Vector3(_originalScaleX * (flip ? -1 : 1), animator.transform.localScale.y, animator.transform.localScale.z);
        }
    }
}
