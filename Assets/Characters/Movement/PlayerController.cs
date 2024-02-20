using System;
using SchizoQuest.Helpers;
using SchizoQuest.Input;
using SchizoQuest.Menu.PauseMenu;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SchizoQuest.Characters.Movement
{
    public class PlayerController : MonoBehaviour
    {
        public MovementStats stats;
        public Rigidbody2D rb;
        public GroundTracker groundTracker;
        public bool canMove;
        public bool canJump;

        /// <summary>The latest directional input received.</summary>
        public Vector2 MoveInput => _move;
        public bool IsJumping => _jumping;
        // by popular request
        public bool IsGrounded => groundTracker.IsGrounded;
        
        private InputActions _input;
        private Vector2 _move;

        private bool _jumpPressQueued;
        private bool _jumpHeld;
        private float _bhopTimer; // time since last jump press

        private int _jumpsRemaining = 0;
        private bool _wasOnGround;
        private bool _jumping;
        private bool _cutoff;
        private bool _jumpedThisFrame; // hack because of the 1 frame delay on ground check

        private float _defaultGravMulti;
        private float _gravMultiShouldBe; // detect outside changes

        private InputAction moveInput;
        private InputAction jumpInput;

        private void Awake()
        {
            this.EnsureComponent(ref rb);
            this.EnsureComponent(ref groundTracker);
            _defaultGravMulti = _gravMultiShouldBe = rb.gravityScale;

            _input = new InputActions();
            _input.Player.Enable();

            moveInput = _input.Player.Move;
            jumpInput = _input.Player.Jump;

            _bhopTimer = float.PositiveInfinity;
        }

        private void OnEnable()
        {
            moveInput.started += OnMoveInput;
            moveInput.performed += OnMoveInput;
            moveInput.canceled += OnMoveInput;

            jumpInput.started += OnJumpInput;
            jumpInput.performed += OnJumpInput;
            jumpInput.canceled += OnJumpInput;
        }

        private void FixedUpdate()
        {
            CheckGrounded();

            HandleVertical();
            HandleHorizontal();
        }

        public void OnMoveInput(InputAction.CallbackContext ctx)
        {
            _move = ctx.ReadValue<Vector2>();
        }

        public void OnJumpInput(InputAction.CallbackContext ctx)
        {
            if (PauseMenuBehaviour.IsOpen) return;
            if (ctx.started)
            {
                //Debug.Log("Jump pressed");
                _jumpPressQueued = true;
                if (enabled && !IsGrounded)
                    _bhopTimer = 0;
            }
            _jumpHeld = ctx.performed;
        }

        private void CheckGrounded()
        {
            if (IsGrounded)
            {
                _cutoff = false;
                if (_jumpedThisFrame)
                {
                    // no more coyote time after jumping
                    groundTracker.coyoteTimer = float.PositiveInfinity;
                    _bhopTimer = float.PositiveInfinity;
                }
                else
                {
                    _jumping = false;
                }
                if (!_wasOnGround)
                {
                    if (!_jumping && _bhopTimer < stats.bunnyhopBuffer)
                    {
                        //Debug.Log("Bhop jump");
                        _jumpPressQueued = true;
                    }
                    _jumping = false;
                    _jumpsRemaining = stats.extraJumps;
                }
            }
            _wasOnGround = IsGrounded;
        }

        private void AdjustGravity()
        {
            if (rb.gravityScale != _gravMultiShouldBe) // changed from the outside
                _defaultGravMulti = rb.gravityScale;
            float scale = CalcGravity();
            rb.gravityScale = _gravMultiShouldBe = scale;
        }

        private float CalcGravity()
        {
            float scale = _defaultGravMulti;
            bool applyMidAirGravity = !IsGrounded
                || groundTracker.surfaceCollider.GetComponent<Rigidbody2D>() // moving platform hack
                || _jumpedThisFrame;
            if (applyMidAirGravity)
            {
                scale *= GetJumpGravityMulti();
            }
            if (_jumping && _cutoff && rb.velocity.y > 0.01f)
            {
                scale *= stats.earlyCutoffGravityMulti;
            }
            return scale;
        }

        private void HandleVertical()
        {
            _bhopTimer += Time.deltaTime;
            if (_bhopTimer > stats.bunnyhopBuffer)
                _bhopTimer = float.PositiveInfinity;

            if (_jumpPressQueued)
            {
                _jumpPressQueued = false;
                if (TryExecuteJump())
                {
                    //Debug.Log("Ground/coyote jump");
                    return;
                }
            }
            
            AdjustGravity();

            _jumpedThisFrame = false;
            if (_jumping)
            {
                if (!_jumpHeld && !_cutoff)
                {
                    // early cutoff (variable jump height)
                    _cutoff = true;
                    //Debug.Log("Early cutoff");
                }
            }
        }

        private bool TryExecuteJump()
        {
            if (!canJump) return false;

            bool resetFallVelocity = stats.resetFallVelocity;
            bool resetRiseVelocity = stats.resetRiseVelocity;
            if (!IsGrounded)
            {
                bool doCoyoteJump = groundTracker.IsRecentlyGrounded;
                bool doExtraJump = _jumpsRemaining > 0;
                if (doCoyoteJump)
                {
                    //Debug.Log($"Coyote");
                    resetFallVelocity = true;
                }
                else if (doExtraJump)
                {
                    //Debug.Log($"Extra jump {_jumpsRemaining}");
                    _jumpsRemaining--;
                }
                else
                {
                    //Debug.Log("No more jumps");
                    return false;
                }
            }
            bool resetVerticalSpeed = resetFallVelocity && rb.velocity.y < 0 || resetRiseVelocity && rb.velocity.y > 0;

            ExecuteJump(resetVerticalSpeed);
            return true;
        }

        public void ExecuteJump(bool resetVerticalSpeed = false)
        {
            if (resetVerticalSpeed)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0);
            }
            // Jump peak height = 1/2 * (v0 ^ 2 / gravity)
            // therefore v0 = sqrt(2 * height * gravity)
            // also, just for fun: time to peak = v0 * gravity

            // gravity will be this while rising
            float gravScale = _defaultGravMulti * GetJumpGravityMulti();
            float gravity = gravScale * -Physics2D.gravity.y;
            
            float jumpSpeed = Mathf.Sqrt(2 * stats.peakHeight * gravity);

            rb.velocity += new Vector2(0, jumpSpeed);

            _jumping = true;
            _jumpedThisFrame = true;
            _cutoff = false;
        }

        // todo cache and check stats for changes (god i miss RxNet)
        private float GetJumpGravityMulti()
        {
            // s = u*t + 0.5*a*t^2
            // calc gravity as coming down from the peak (u=0, s=height)
            // since the ideal parabola is symmetric, it also applies to the rising half
            float gravity = 2 * stats.peakHeight / (stats.timeToPeak * stats.timeToPeak);
            return gravity / -Physics2D.gravity.y;
        }

        private void HandleHorizontal()
        {
            float moveProportion = _move.x;

            if (Mathf.Abs(moveProportion) < 0.1f) // todo configurable deadzone (surely in a future patch (clueless))
            {
                float deceleration = IsGrounded
                    ? stats.idleDeceleration
                    : stats.idleAirDeceleration;
                Accelerate(-rb.velocity.x / stats.maxHorizontalSpeed, deceleration);
                return;
            }
            if (!canMove) return;
            
            float acceleration = IsGrounded
                ? stats.groundAcceleration
                : stats.airAcceleration;

            // moving in the opposite direction (turning)
            if (!Mathf.Approximately(rb.velocity.x, 0)
                && Mathf.Sign(moveProportion) != Mathf.Sign(rb.velocity.x))
            {
                acceleration *= stats.turnDecelerationMulti;
            }
            
            Accelerate(moveProportion, acceleration);
        }

        private void Accelerate(float proportion, float acceleration)
        {
            float deltaV = proportion * acceleration * Time.deltaTime;
            float maxSpeed = stats.maxHorizontalSpeed * Math.Abs(proportion);
            if (Mathf.Sign(proportion) == Mathf.Sign(rb.velocity.x))
            {
                float delta = maxSpeed - Mathf.Abs(rb.velocity.x);
                if (delta < 0.01f) return;
                // Min() on magnitude while preserving sign
                if (Mathf.Abs(deltaV) > delta)
                    deltaV = Mathf.Sign(deltaV) * delta;
            }

            rb.velocity += new Vector2(deltaV, 0);
        }

        private void OnDisable()
        {
            moveInput.started -= OnMoveInput;
            moveInput.performed -= OnMoveInput;
            moveInput.canceled -= OnMoveInput;
            jumpInput.started -= OnJumpInput;
            jumpInput.performed -= OnJumpInput;
            jumpInput.canceled -= OnJumpInput;
            
            _move = Vector2.zero;
            _jumpPressQueued = false;
            _jumpHeld = false;
            _jumping = false;
        }
    }
}