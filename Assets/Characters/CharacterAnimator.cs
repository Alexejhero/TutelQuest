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
        public GroundTracker groundTracker;

        public virtual AnimationClip IdleFrontAnim => idleFrontAnim;
        public virtual AnimationClip IdleLeftAnim => idleLeftAnim;
        public virtual AnimationClip IdleRightAnim => idleRightAnim;
        public virtual AnimationClip MoveLeftAnim => moveLeftAnim;
        public virtual AnimationClip MoveRightAnim => moveRightAnim;
        public virtual bool FlipLeftAnims => flipLeftAnims;
        public virtual bool FlipRightAnims => flipRightAnims;

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
            if (Math.Abs(velocity.x) < 1 && Math.Abs(velocity.y) < 1)
            {
                if (UnityEngine.Input.GetKey(KeyCode.S) || UnityEngine.Input.GetKey(KeyCode.DownArrow)) _lastMoveDirection = 0;

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
                if (velocity.x < 0)
                {
                    if (!_hintHidden)
                    {
                        _hintHidden = true;
                        groundTracker.SendMessage("HideHint", HintType.WASD, SendMessageOptions.DontRequireReceiver);
                    }

                    CurrentClip = groundTracker.isOnGround ? MoveLeftAnim : IdleLeftAnim;
                    _lastMoveDirection = -1;
                    SetFlip(FlipLeftAnims);
                }
                else if (velocity.x > 0)
                {
                    if (!_hintHidden)
                    {
                        _hintHidden = true;
                        groundTracker.SendMessage("HideHint", HintType.WASD, SendMessageOptions.DontRequireReceiver);
                    }

                    CurrentClip = groundTracker.isOnGround ? MoveRightAnim : IdleRightAnim;
                    _lastMoveDirection = 1;
                    SetFlip(FlipRightAnims);
                }
            }
        }

        protected void SetFlip(bool flip)
        {
            animator.transform.localScale = new Vector3(_originalScaleX * (flip ? -1 : 1), animator.transform.localScale.y, animator.transform.localScale.z);
        }
    }
}
