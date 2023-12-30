using System;
using PowerTools;
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
                        CurrentClip = idleFrontAnim;
                        _flipped = false;
                        break;

                    case < 0:
                        CurrentClip = idleLeftAnim;
                        DoFlip(flipLeftAnims);
                        break;

                    case > 0:
                        CurrentClip = idleRightAnim;
                        DoFlip(flipRightAnims);
                        break;
                }
            }
            else
            {
                if (velocity.x < 0)
                {
                    CurrentClip = moveLeftAnim;
                    _lastMoveDirection = -1;
                    DoFlip(flipLeftAnims);
                }
                else
                {
                    CurrentClip = moveRightAnim;
                    _lastMoveDirection = 1;
                    DoFlip(flipRightAnims);
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
