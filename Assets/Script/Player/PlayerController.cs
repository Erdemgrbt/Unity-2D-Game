using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private TrailRenderer _trailRenderer;

    [Header("Hareket Parametreleri")]
    public float walkSpeed = 5f; // Yürüme Hýzý
    public float airWalkSpeed = 3f; // Havada süzülme hýzý
    public float jumpImpulse = 10f; // Zýplama kuvveti

    [Header("Dash Parametreleri")]
    [SerializeField] private float _dashingVelocity = 14f; // Dash yapma kuvveti
    [SerializeField] private float _dashingTime = 0.5f;  // Dash yapma süresi
    [SerializeField] private float _dashCooldown = 1f;  // Dash cooldown süresi
    private Vector2 _dashingDir;
    [SerializeField] private bool _isDashing;
    [SerializeField] private bool _canDash = true;
    private float _lastDashTime;  // Son dash yapýlma zamaný

    [Header("Ekstra Hareket Parametreleri")]
    public float coyoteTime = 0.2f; // Coyote Time süresi
    public float jumpBufferTime = 0.1f; // Jump Buffer süresi
    private float lastGroundedTime; // Oyuncunun yere en son dokunduðu zaman
    private float lastJumpPressTime; // Oyuncunun zýplama tuþuna en son bastýðý zaman


    Rigidbody2D rb;
    Animator animator;
    private Vector2 moveInput;

    //Referans
    TouchingDirections touchingDirections; //Touching Directions Scriptine Eriþim

    // Oyun Baþlatýldýðý An Çalýþtýrýlacak Fonksiyon
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        touchingDirections = GetComponent<TouchingDirections>();
        _trailRenderer = GetComponent<TrailRenderer>();
    }

    // Devamlý Çalýþtýrýlmasý Gereken Fizik Uygulamalarý Ýçin Fonksiyon
    private void FixedUpdate()
    {
        // Hareket hýzýna göre ilerleme
        rb.velocity = new Vector2(moveInput.x * (CurrentMoveSpeed * 100) * Time.fixedDeltaTime, rb.velocity.y);

        animator.SetFloat(AnimationStrings.yVelocity, rb.velocity.y);

        // Coyote Time: Oyuncunun yere en son dokunduðu zamaný güncelle
        if (touchingDirections.IsGrounded)
        {
            lastGroundedTime = Time.time;
        }

        // Dash esnasýnda hýz kontrolü
        if (_isDashing)
        {
            rb.velocity = _dashingDir.normalized * _dashingVelocity;
            return;
        }

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
                if (touchingDirections.IsGrounded)
                {
                    return walkSpeed;
                }
                else
                {
                    // Havada Hareket Etme
                    return airWalkSpeed;
                }
            }
            else
            {
                // Sabitken Hýz 0
                return 0;
            }
        }
    }

    [Header("Diðer")]
    // Yürüme Animasyonu
    [SerializeField]
    private bool _isMoving = false;
    public bool isMoving
    {
        get { return _isMoving; }
        private set
        {
            _isMoving = value;
            animator.SetBool(AnimationStrings.isMoving, value);
        }
    }

    // Yön Tespiti
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

    // Hareket Etme
    public void onMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

        isMoving = moveInput != Vector2.zero;

        setFacingDirection(moveInput);
    }

    private void setFacingDirection(Vector2 moveInput)
    {
        if (moveInput.x > 0 && !IsFacingRight)
        {
            // Sað
            IsFacingRight = true;
        }
        else if (moveInput.x < 0 && IsFacingRight)
        {
            // Sol
            IsFacingRight = false;
        }
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.started && _canDash && Time.time >= _lastDashTime + _dashCooldown) // Dash giriþ ve cooldown kontrolü
        {
            _isDashing = true;
            _canDash = false;
            _trailRenderer.emitting = true;

            // Farenin ekran koordinatlarýndan yönü hesapla
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _dashingDir = (mousePosition - (Vector2)transform.position).normalized;

            _lastDashTime = Time.time; // Son dash zamanýný güncelle
            StartCoroutine(StopDashing());
        }
    }

    private IEnumerator StopDashing()
    {
        yield return new WaitForSeconds(_dashingTime); // Dash süresi boyunca bekle

        // Dash sona erdiðinde hýz ayarý
        if (!touchingDirections.IsGrounded) // Karakter havadaysa
        {
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Min(rb.velocity.y, 0)); // Yukarýya hareket varsa, durdur
        }

        _trailRenderer.emitting = false;
        _isDashing = false;
    }

    // Zýplama
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            lastJumpPressTime = Time.time;
        }

        // Coyote Time ve Jump Buffer kontrolleri
        if ((Time.time - lastGroundedTime <= coyoteTime || touchingDirections.IsGrounded) &&
            Time.time - lastJumpPressTime <= jumpBufferTime)
        {
            animator.SetTrigger(AnimationStrings.jump);
            rb.velocity = new Vector2(rb.velocity.x, jumpImpulse);
        }
    }
}