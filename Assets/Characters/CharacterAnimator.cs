using System;
using System.Collections;
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
        public CharacterAnimator sisterAnimator;

        public virtual AnimationClip IdleFrontAnim => idleFrontAnim;
        public virtual AnimationClip IdleLeftAnim => idleLeftAnim;
        public virtual AnimationClip IdleRightAnim => idleRightAnim;
        public virtual AnimationClip MoveLeftAnim => moveLeftAnim;
        public virtual AnimationClip MoveRightAnim => moveRightAnim;
        public virtual bool FlipLeftAnims => flipLeftAnims;
        public virtual bool FlipRightAnims => flipRightAnims;
        public virtual Vector2 MoveInput => player.controller.MoveInput;
        protected virtual bool IsGrounded => player.IsRecentlyGrounded;

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

        private void OnEnable()
        {
            if (sisterAnimator && sisterAnimator.CurrentAnimation != AnimationType.None) StartCoroutine(CoSetAnimation());
            return;

            IEnumerator CoSetAnimation()
            {
                yield return null; // need to wait for SpriteAnim to do its thing
                SetAnimation(sisterAnimator.CurrentAnimation);
            }
        }

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
        }

        public void SetAnimation(AnimationType type)
        {
            switch (type)
            {
                case AnimationType.None:
                case AnimationType.IdleFront:
                    CurrentClip = IdleFrontAnim;
                    _lastMoveDirection = 0;
                    SetFlip(false);
                    break;

                case AnimationType.IdleLeft:
                    CurrentClip = IdleLeftAnim;
                    _lastMoveDirection = -1;
                    SetFlip(FlipLeftAnims);
                    break;

                case AnimationType.IdleRight:
                    CurrentClip = IdleRightAnim;
                    _lastMoveDirection = 1;
                    SetFlip(FlipRightAnims);
                    break;

                case AnimationType.MoveLeft:
                    CurrentClip = IsGrounded ? MoveLeftAnim : IdleLeftAnim;
                    _lastMoveDirection = -1;
                    SetFlip(FlipLeftAnims);
                    break;

                case AnimationType.MoveRight:
                    CurrentClip = IsGrounded ? MoveRightAnim : IdleRightAnim;
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
