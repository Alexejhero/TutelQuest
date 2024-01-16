using System;
using PowerTools;
using SchizoQuest.Game;
using UnityEngine;

namespace SchizoQuest.Characters
{
    public class CharacterAnimator : MonoBehaviour
    {
        public enum AnimationType
        {
            None,
            IdleFront,
            IdleLeft,
            IdleRight,
            MoveLeft,
            MoveRight
        }

        public Rigidbody2D rb;
        public SpriteAnim animator;
        public AnimationClip idleFrontAnim;
        public AnimationClip idleLeftAnim;
        public AnimationClip idleRightAnim;
        public AnimationClip moveLeftAnim;
        public AnimationClip moveRightAnim;
        public bool flipLeftAnims;
        public bool flipRightAnims;
        public Player player;
        [Tooltip("Time to keep playing the moving animation after the player leaves the ground")]
        public float animationCoyoteTime = 0.1f;
        public CharacterAnimator sisterAnimator;

        private float _animationCoyoteTimer;

        public virtual AnimationClip IdleFrontAnim => idleFrontAnim;
        public virtual AnimationClip IdleLeftAnim => idleLeftAnim;
        public virtual AnimationClip IdleRightAnim => idleRightAnim;
        public virtual AnimationClip MoveLeftAnim => moveLeftAnim;
        public virtual AnimationClip MoveRightAnim => moveRightAnim;
        public virtual bool FlipLeftAnims => flipLeftAnims;
        public virtual bool FlipRightAnims => flipRightAnims;
        public virtual Vector2 MoveInput => player.controller.MoveInput;

        private float _originalScaleX;
        private int _lastMoveDirection = 0;
        private AnimationClip _currentClip;

        private bool _hintHidden;

        public AnimationType CurrentAnimation { get; private set; }

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

        /*private void OnEnable()
        {
            if (sisterAnimator && sisterAnimator.CurrentAnimation != AnimationType.None) SetAnimation(sisterAnimator.CurrentAnimation);
        }*/

        protected virtual void Update()
        {
            if (player != Player.ActivePlayer) return;
            Vector2 velocity = rb.velocity;
            Vector2 input = MoveInput;
            if (Math.Abs(velocity.x) < 1 && Math.Abs(velocity.y) < 1)
            {
                if (input.y < 0) _lastMoveDirection = 0;

                AnimationType animationType = _lastMoveDirection switch
                {
                    0 => AnimationType.IdleFront,
                    < 0 => AnimationType.IdleLeft,
                    > 0 => AnimationType.IdleRight,
                };

                SetAnimation(animationType);
            }
            else
            {
                if (input.x < 0)
                {
                    if (!_hintHidden)
                    {
                        _hintHidden = true;
                        player.SendMessage("HideHint", HintType.WASD, SendMessageOptions.DontRequireReceiver);
                    }

                    SetAnimation(AnimationType.MoveLeft);
                }
                else if (input.x > 0)
                {
                    if (!_hintHidden)
                    {
                        _hintHidden = true;
                        player.SendMessage("HideHint", HintType.WASD, SendMessageOptions.DontRequireReceiver);
                    }

                    SetAnimation(AnimationType.MoveRight);
                }
            }

            if (player.controller.groundTracker.isOnGround)
                _animationCoyoteTimer = 0;
            else
                _animationCoyoteTimer += Time.deltaTime;
        }

        public void SetAnimation(AnimationType type)
        {
            switch (type)
            {
                case AnimationType.None:
                case AnimationType.IdleFront:
                    CurrentClip = IdleFrontAnim;
                    SetFlip(false);
                    break;

                case AnimationType.IdleLeft:
                    CurrentClip = IdleLeftAnim;
                    SetFlip(FlipLeftAnims);
                    break;

                case AnimationType.IdleRight:
                    CurrentClip = IdleRightAnim;
                    SetFlip(FlipRightAnims);
                    break;

                case AnimationType.MoveLeft:
                    CurrentClip = _animationCoyoteTimer < animationCoyoteTime ? MoveLeftAnim : IdleLeftAnim;
                    _lastMoveDirection = -1;
                    SetFlip(FlipLeftAnims);
                    break;

                case AnimationType.MoveRight:
                    CurrentClip = _animationCoyoteTimer < animationCoyoteTime ? MoveRightAnim : IdleRightAnim;
                    _lastMoveDirection = 1;
                    SetFlip(FlipRightAnims);
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }

            CurrentAnimation = type;
        }

        protected void SetFlip(bool flip)
        {
            animator.transform.localScale = new Vector3(_originalScaleX * (flip ? -1 : 1), animator.transform.localScale.y, animator.transform.localScale.z);
        }
    }
}
