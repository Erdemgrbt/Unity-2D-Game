using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    private TrailRenderer _trailRenderer;
    private TouchingDirections touchingDirections;

    
    [Header("Hareket Parametreleri")]
    public float walkSpeed = 5f;
    public float airWalkSpeed = 3f;
    public float jumpImpulse = 10f;

    [Header("Ekstra Hareket Parametreleri")]
    public float coyoteTime = 0.2f;     // Ziplamadan sonra yere basmamis gibi davranma suresi
    public float jumpBufferTime = 0.1f; // Ziplamaya erken basildiysa hatirlama suresi
    private float lastGroundedTime;
    private float lastJumpPressTime;
    private Vector2 moveInput;

    [Header("Dash Parametreleri")]
    [SerializeField] private float _dashingVelocity = 14f;
    [SerializeField] private float _dashingTime = 0.5f;
    [SerializeField] private float _dashCooldown = 1f;

    [Header("Ses")]
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip dashSound;

    private Vector2 _dashingDir;
    [SerializeField] private bool _isDashing;
    [SerializeField] private bool _canDash = true;
    private float _lastDashTime;

    [Header("Yurutme ve Yon")]
    [SerializeField] private bool _isMoving = false;
    public bool isMoving
    {
        get { return _isMoving; }
        private set
        {
            _isMoving = value;
            animator.SetBool(AnimationStrings.isMoving, value);
        }
    }

    public bool _isFacingRight = true;
    public bool IsFacingRight
    {
        get { return _isFacingRight; }
        private set
        {
            if (_isFacingRight != value)
            {
                transform.localScale *= new Vector2(-1, 1);
            }
            _isFacingRight = value;
        }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        touchingDirections = GetComponent<TouchingDirections>();
        _trailRenderer = GetComponent<TrailRenderer>();
    }

    private void FixedUpdate()
    {
        // Normal hareket
        rb.velocity = new Vector2(moveInput.x * (CurrentMoveSpeed * 100) * Time.fixedDeltaTime, rb.velocity.y);

        animator.SetFloat(AnimationStrings.yVelocity, rb.velocity.y);

        // Coyote Time kontrolu
        if (touchingDirections.IsGrounded)
        {
            lastGroundedTime = Time.time;
        }

        // Dash yapiliyorsa sabit hiz uygula
        if (_isDashing)
        {
            rb.velocity = _dashingDir.normalized * _dashingVelocity;
            return;
        }

        // Yere basiliyorsa tekrar dash yapilabilir
        if (touchingDirections.IsGrounded)
        {
            _canDash = true;
        }
    }

    public float CurrentMoveSpeed
    {
        get
        {
            if (isMoving && !touchingDirections.IsOnWall)
            {
                return touchingDirections.IsGrounded ? walkSpeed : airWalkSpeed;
            }
            return 0;
        }
    }

    private void setFacingDirection(Vector2 moveInput)
    {
        if (moveInput.x > 0 && !IsFacingRight)
        {
            IsFacingRight = true;
        }
        else if (moveInput.x < 0 && IsFacingRight)
        {
            IsFacingRight = false;
        }
    }
    


    public void onMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        isMoving = moveInput != Vector2.zero;
        setFacingDirection(moveInput);
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            lastJumpPressTime = Time.time;
            
        }

        if ((Time.time - lastGroundedTime <= coyoteTime || touchingDirections.IsGrounded) &&
            Time.time - lastJumpPressTime <= jumpBufferTime)
        {
            animator.SetTrigger(AnimationStrings.jump);
            rb.velocity = new Vector2(rb.velocity.x, jumpImpulse);

            if(touchingDirections.IsGrounded)
                SoundManager.instance.PlaySound(jumpSound);
        }
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.started && _canDash && Time.time >= _lastDashTime + _dashCooldown)
        {
            _isDashing = true;
            _canDash = false;
            _trailRenderer.emitting = true;

            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _dashingDir = (mousePosition - (Vector2)transform.position).normalized;
            SoundManager.instance.PlaySound(dashSound);

            _lastDashTime = Time.time;
            StartCoroutine(StopDashing());
        }
    }
    public void ResetDash()
    {
        _canDash = true;
        ResetDashCooldown();
    }
    public void ResetDashCooldown()
    {
        // Cooldownu sýfýrlamak için, son dash zamanýný geçmiþe çekiyoruz
        _lastDashTime = Time.time - _dashCooldown;
    }

    private IEnumerator StopDashing()
    {
        yield return new WaitForSeconds(_dashingTime);

        if (!touchingDirections.IsGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Min(rb.velocity.y, 0));
        }

        _trailRenderer.emitting = false;
        _isDashing = false;
    }
}
