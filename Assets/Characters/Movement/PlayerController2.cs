using SchizoQuest.Helpers;
using SchizoQuest.Input;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SchizoQuest.Characters.Movement
{
    public class PlayerController2 : MonoBehaviour
    {
        public MovementStats stats;
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
        private bool _jumpedThisFrame; // hack because of the 1 frame delay on ground check

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
#region debug delete later
        private float _peak;
        private float _base;
        private float _peakTimer;
        [Range(0,2)]
        public float timescale = 1;
        private void Update()
        {
            Time.timeScale = timescale;
        }
#endregion
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
                //Debug.Log("Jump pressed");
                _jumpPressQueued = true;
                if (!groundTracker.isOnGround)
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
                if (!_jumpedThisFrame)
                    _jumping = false;
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
            _wasOnGround = isOnGround;
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
            float scale = _defaultGravMulti * GetJumpGravityMulti();
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
            _coyoteTimer += Time.deltaTime;
            if (_coyoteTimer > stats.coyoteTime)
                _coyoteTimer = float.PositiveInfinity;

            if (_jumpPressQueued)
            {
                _jumpPressQueued = false;
                if (TryExecuteJump())
                {
                    //Debug.Log($"Ground/coyote jump - frame {Time.frameCount}");
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
                if (transform.position.y > _peak)
                {
                    _peak = transform.position.y;
                    _peakTimer += Time.deltaTime;
                }
                else if (transform.position.y < _peak && _peakTimer > 0)
                {
                    //Debug.Log($"Actual peak time {_peakTimer}");
                    _peakTimer = 0;
                }
            }
            else
            {
                if (_peak != 0)
                {
                    //Debug.Log($"Actual peak {_peak - _base}");
                }
                _peak = 0;
            }
        }

        private bool TryExecuteJump()
        {
            if (!canJump) return false;

            bool resetVerticalSpeed = false;

            if (!groundTracker.isOnGround)
            {
                float timeLeftForCoyote = stats.coyoteTime - _coyoteTimer;
                bool doCoyoteJump = timeLeftForCoyote >= 0 && rb.velocity.y < 0;
                bool doExtraJump = _jumpsRemaining > 0;
                bool resetFallVelocity = stats.airResetFallVelocity;
                bool resetRiseVelocity = stats.airResetRiseVelocity;
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

                resetVerticalSpeed = resetFallVelocity && rb.velocity.y < 0 || resetRiseVelocity && rb.velocity.y > 0;
            }

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

            // gravity will be this while rising
            float gravScale = _defaultGravMulti * GetJumpGravityMulti();
            float gravity = gravScale * -Physics2D.gravity.y;
            
            float jumpSpeed = Mathf.Sqrt(2 * stats.peakHeight * gravity);

            rb.velocity += new Vector2(0, jumpSpeed);
            /*
            float calcPeakHeight = jumpSpeed * jumpSpeed / (2 * gravity);
            // ut = 0.5at^2
            // u = 0.5at
            // t = 
            float calcTimeToPeak = 2 * jumpSpeed / gravity;
            Debug.Log($"Jumping with gravity {gravity} (scale {gravScale}) (calc peak {calcPeakHeight})");
            Debug.Log($"velocity {rb.velocity.y} ({jumpSpeed} added); calc time to peak {calcTimeToPeak}");
            */
            _jumping = true;
            _jumpedThisFrame = true;
            _cutoff = false;
            _base = transform.position.y;
            // no more coyote time after jumping
            _coyoteTimer = float.PositiveInfinity;
            _bhopTimer = float.PositiveInfinity;
        }

        // todo cache and check stats for changes
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
            if (!canMove) moveProportion = 0;

            if (Mathf.Approximately(moveProportion, 0))
            {
                float deceleration = groundTracker.isOnGround
                    ? stats.idleDeceleration
                    : stats.idleAirDeceleration;
                Accelerate(-rb.velocity.x / stats.maxHorizontalSpeed, deceleration);
                return;
            }
            
            float acceleration = groundTracker.isOnGround
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
            if (Mathf.Sign(proportion) == Mathf.Sign(rb.velocity.x))
            {
                float delta = stats.maxHorizontalSpeed - Mathf.Abs(rb.velocity.x);
                if (delta < 0.01f) return;
                // Min() on magnitude while preserving sign
                if (Mathf.Abs(deltaV) > delta)
                    deltaV = Mathf.Sign(deltaV) * delta;
            }

            rb.velocity += new Vector2(deltaV, 0);
        }
    }
}