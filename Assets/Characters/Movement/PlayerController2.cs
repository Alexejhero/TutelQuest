using SchizoQuest.Helpers;
using SchizoQuest.Input;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SchizoQuest.Characters.Movement
{
    public class PlayerController2 : MonoBehaviour
    {
        public Stats stats;
        public Rigidbody2D rb;
        public Collider2D groundTestCollider;
        public GroundTracker groundTracker;
        public bool canMove;
        public bool canJump;

        private InputActions _input;
        private Vector2 _move;
        private bool _jumpPressQueued;
        private bool _jumpHeld;
        private float _bhopTimer; // time since last jump press
        private float _coyoteTimer; // time since left ground

        private int _jumpsRemaining = 0;
        private bool _wasOnGround;
        private bool _jumping;
        private bool _cutoff;

        private float _defaultGravMulti;
        private float _gravMultiShouldBe; // detect outside changes

        private void Awake()
        {
            this.EnsureComponent(ref rb);
            if (!groundTestCollider) groundTestCollider = GetComponent<Collider2D>();
            this.EnsureComponent(ref groundTracker);
            _defaultGravMulti = _gravMultiShouldBe = rb.gravityScale;
            _input = new InputActions();
            _input.Player.Enable();
            InputAction moveInput = _input.Player.Move;
            moveInput.started += OnMoveInput;
            moveInput.performed += OnMoveInput;
            moveInput.canceled += OnMoveInput;
            InputAction jumpInput = _input.Player.Jump;
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
            if (ctx.started)
            {
                _jumpPressQueued = true;
                _bhopTimer = 0;
            }
            _jumpHeld = ctx.performed;
        }

        private void CheckGrounded()
        {
            bool isOnGround = groundTracker.isOnGround;
            if (isOnGround)
            {
                _coyoteTimer = 0;
                _cutoff = false;
                if (!_wasOnGround)
                {
                    _jumping = false;
                    if (_bhopTimer < stats.bunnyhopBuffer)
                    {
                        AdjustGravity();
                        _jumpPressQueued = false;
                        TryExecuteJump();
                    }
                    _jumpsRemaining = stats.extraJumps;
                }
            }
            _wasOnGround = isOnGround;
        }

        private void AdjustGravity()
        {
            if (rb.gravityScale != _gravMultiShouldBe)
                _defaultGravMulti = rb.gravityScale;
            float scale = _defaultGravMulti;
            if (!groundTracker.isOnGround)
            {
                if (_jumping && rb.velocity.y > 0.01f)
                {
                    if (_cutoff)
                        scale *= stats.earlyCutoffGravityMulti; 
                    else
                        scale *= stats.jumpUpwardsGravityMulti;
                }
                if (rb.velocity.y < 0.01f)
                    scale *= stats.fallGravityMulti;
            }
            rb.gravityScale = _gravMultiShouldBe = scale;
        }

        private void Update()
        {
            AdjustGravity();
            rb.gravityScale *= GetJumpGravity() / Physics2D.gravity.y;
            _gravMultiShouldBe = rb.gravityScale;
        }

        private void HandleVertical()
        {
            _bhopTimer += Time.deltaTime;
            _coyoteTimer += Time.deltaTime;

            AdjustGravity();
            if (_jumpPressQueued)
            {
                _jumpPressQueued = false;
                if (TryExecuteJump())
                    return;
            }
            if (_jumping)
            {
                if (!_jumpHeld && !_cutoff)
                {
                    // early cutoff (variable jump height)
                    _cutoff = true;
                    Debug.Log("Early cutoff");
                }
            }
            if (rb.velocity.y < -stats.terminalVelocity)
                rb.velocity = new Vector2(rb.velocity.x, -stats.terminalVelocity);
        }

        private bool TryExecuteJump()
        {
            if (!canJump) return false;

            if (!groundTracker.isOnGround)
            {
                float timeLeftForCoyote = stats.coyoteTime - _coyoteTimer;
                bool doCoyoteJump = timeLeftForCoyote >= 0 && rb.velocity.y < 0;
                bool doExtraJump = _jumpsRemaining > 0;
                bool resetFallVelocity = stats.airJumpOverrideFallVelocity;
                bool resetRiseVelocity = stats.airJumpOverrideRiseVelocity;
                if (doCoyoteJump)
                {
                    //Debug.Log($"Coyote {timeLeftForCoyote}");
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

                if (resetFallVelocity && rb.velocity.y < 0 || resetRiseVelocity && rb.velocity.y > 0)
                {
                    rb.velocity = new Vector2(rb.velocity.x, 0);
                }
            }
            else
            {
                // no coyote time when jumping off ground
                _coyoteTimer = stats.coyoteTime;
            }

            ExecuteJump();
            _jumping = true;
            _cutoff = false;
            _bhopTimer = stats.bunnyhopBuffer;
            return true;
        }

        public void ExecuteJump()
        {
            // Jump peak height = 1/2 * (v0 ^ 2 / gravity)
            // therefore v0 = sqrt(2 * height * gravity)
            float gravity = _defaultGravMulti * stats.jumpUpwardsGravityMulti * -Physics2D.gravity.y;
            // gravity += -GetJumpGravity();
            Debug.Log($"Jumping with gravity {gravity}");
            float jumpSpeed = Mathf.Sqrt(2 * stats.peakHeight * gravity);

            rb.velocity += new Vector2(0, jumpSpeed);
        }

        private float GetJumpGravity() => -2 * stats.peakHeight / (stats.timeToPeak * stats.timeToPeak);

        private void HandleHorizontal()
        {
            float acceleration = stats.groundAcceleration;
            float moveProportion = _move.x;
            if (!canMove) moveProportion = 0;

            if (Mathf.Approximately(moveProportion, 0))
            {
                if (groundTracker.isOnGround)
                    Accelerate(-rb.velocity.x / stats.maxHorizontalSpeed, stats.idleDeceleration);
                return;
            }
            else if (!Mathf.Approximately(rb.velocity.x, 0)
                && Mathf.Sign(moveProportion) != Mathf.Sign(rb.velocity.x))
            {
                acceleration *= stats.turnDecelerationMulti;
            }
            
            if (!groundTracker.isOnGround)
                acceleration *= stats.airAccelerationMulti;
            
            Accelerate(moveProportion, acceleration);
        }

        private void Accelerate(float proportion, float acceleration)
        {
            float delta = Mathf.Abs(rb.velocity.x) - stats.maxHorizontalSpeed;
            if (Mathf.Sign(proportion) == Mathf.Sign(rb.velocity.x)
                && delta >= 0) return;

            float deltaV = proportion * acceleration * Time.deltaTime;
            if (Mathf.Abs(deltaV) > Mathf.Abs(delta))
                deltaV = Mathf.Sign(deltaV) * -delta;

            rb.velocity += new Vector2(deltaV, 0);
        }
    }
}