using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private TrailRenderer _trailRenderer;

    [Header("Hareket Parametreleri")]
    public float walkSpeed = 5f; // Y�r�me H�z�
    public float airWalkSpeed = 3f; // Havada s�z�lme h�z�
    public float jumpImpulse = 10f; // Z�plama kuvveti

    [Header("Dash Parametreleri")]
    [SerializeField] private float _dashingVelocity = 14f; // Dash yapma kuvveti
    [SerializeField] private float _dashingTime = 0.5f;  // Dash yapma s�resi
    [SerializeField] private float _dashCooldown = 1f;  // Dash cooldown s�resi
    private Vector2 _dashingDir;
    [SerializeField] private bool _isDashing;
    [SerializeField] private bool _canDash = true;
    private float _lastDashTime;  // Son dash yap�lma zaman�

    [Header("Ekstra Hareket Parametreleri")]
    public float coyoteTime = 0.2f; // Coyote Time s�resi
    public float jumpBufferTime = 0.1f; // Jump Buffer s�resi
    private float lastGroundedTime; // Oyuncunun yere en son dokundu�u zaman
    private float lastJumpPressTime; // Oyuncunun z�plama tu�una en son bast��� zaman


    Rigidbody2D rb;
    Animator animator;
    private Vector2 moveInput;

    //Referans
    TouchingDirections touchingDirections; //Touching Directions Scriptine Eri�im

    // Oyun Ba�lat�ld��� An �al��t�r�lacak Fonksiyon
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        touchingDirections = GetComponent<TouchingDirections>();
        _trailRenderer = GetComponent<TrailRenderer>();
    }

    // Devaml� �al��t�r�lmas� Gereken Fizik Uygulamalar� ��in Fonksiyon
    private void FixedUpdate()
    {
        // Hareket h�z�na g�re ilerleme
        rb.velocity = new Vector2(moveInput.x * (CurrentMoveSpeed * 100) * Time.fixedDeltaTime, rb.velocity.y);

        animator.SetFloat(AnimationStrings.yVelocity, rb.velocity.y);

        // Coyote Time: Oyuncunun yere en son dokundu�u zaman� g�ncelle
        if (touchingDirections.IsGrounded)
        {
            lastGroundedTime = Time.time;
        }

        // Dash esnas�nda h�z kontrol�
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
                // Sabitken H�z 0
                return 0;
            }
        }
    }

    [Header("Di�er")]
    // Y�r�me Animasyonu
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

    // Y�n Tespiti
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
            // Sa�
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
        if (context.started && _canDash && Time.time >= _lastDashTime + _dashCooldown) // Dash giri� ve cooldown kontrol�
        {
            _isDashing = true;
            _canDash = false;
            _trailRenderer.emitting = true;

            // Farenin ekran koordinatlar�ndan y�n� hesapla
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _dashingDir = (mousePosition - (Vector2)transform.position).normalized;

            _lastDashTime = Time.time; // Son dash zaman�n� g�ncelle
            StartCoroutine(StopDashing());
        }
    }

    private IEnumerator StopDashing()
    {
        yield return new WaitForSeconds(_dashingTime); // Dash s�resi boyunca bekle

        // Dash sona erdi�inde h�z ayar�
        if (!touchingDirections.IsGrounded) // Karakter havadaysa
        {
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Min(rb.velocity.y, 0)); // Yukar�ya hareket varsa, durdur
        }

        _trailRenderer.emitting = false;
        _isDashing = false;
    }

    // Z�plama
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