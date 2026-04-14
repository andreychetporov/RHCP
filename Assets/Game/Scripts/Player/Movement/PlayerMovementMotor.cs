using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovementMotor : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private GroundProbe groundProbe;
    [SerializeField] private LandingSquash landingSquash;

    [Header("Movement")]
    [SerializeField] private float maxSpeed = 7f;
    [SerializeField] private float acceleration = 30f;
    [SerializeField] private float deceleration = 35f;
    [SerializeField] private float airAccelerationMultiplier = 0.5f;
    [SerializeField] private float maxAccelerationForce = 100f;

    [Header("Crouch")]
    [SerializeField] private float crouchSpeedMultiplier = 0.4f;
    [SerializeField] private float crouchAccelerationMultiplier = 0.5f;

    [Header("Dash")]
    [SerializeField] private float dashSpeed = 16f;
    [SerializeField] private float dashAcceleration = 120f;
    [SerializeField] private float dashDuration = 0.15f;
    [SerializeField] private float dashCooldown = 0.6f;

    private Vector3 _goalVelocity;

    private bool _isDashing;
    private float _dashTimer;
    private float _dashCooldownTimer;
    private float _dashDirection;
    private float _lastInputDirection = 1f;

    public bool IsDashing => _isDashing;
    public bool CanDash => !_isDashing && _dashCooldownTimer <= 0f;
    public float DashCooldownRemaining => Mathf.Max(0f, _dashCooldownTimer);

    private void Reset()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Awake()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody>();

        if (groundProbe == null)
            groundProbe = GetComponent<GroundProbe>();

        if (landingSquash == null)
            landingSquash = GetComponent<LandingSquash>();
    }

    public void TickMove(float moveInput, bool crouchHeld, float fixedDeltaTime)
    {
        if (Mathf.Abs(moveInput) > 0.01f)
            _lastInputDirection = Mathf.Sign(moveInput);

        if (_dashCooldownTimer > 0f)
            _dashCooldownTimer -= fixedDeltaTime;

        if (_isDashing)
        {
            TickDash(fixedDeltaTime);
            return;
        }

        bool isGrounded = groundProbe != null && groundProbe.IsGrounded;

        float speedMultiplier = crouchHeld ? crouchSpeedMultiplier : 1f;
        float accelMultiplier = crouchHeld ? crouchAccelerationMultiplier : 1f;

        Vector3 moveDirection = Vector3.right * moveInput;
        Vector3 targetGoalVelocity = moveDirection * (maxSpeed * speedMultiplier);

        float accelRate = Mathf.Abs(moveInput) > 0.01f
            ? acceleration
            : deceleration;

        accelRate *= accelMultiplier;

        if (!isGrounded)
            accelRate *= airAccelerationMultiplier;

        _goalVelocity = Vector3.MoveTowards(
            _goalVelocity,
            targetGoalVelocity,
            accelRate * fixedDeltaTime
        );

        Vector3 currentVelocity = rb.linearVelocity;
        Vector3 horizontalVelocity = new Vector3(currentVelocity.x, 0f, 0f);

        Vector3 neededAccel = (_goalVelocity - horizontalVelocity) / fixedDeltaTime;
        neededAccel = Vector3.ClampMagnitude(neededAccel, maxAccelerationForce);

        rb.AddForce(neededAccel * rb.mass, ForceMode.Force);
    }

    public bool TryDash(float moveInput, bool crouchHeld)
    {
        if (_isDashing)
            return false;

        if (_dashCooldownTimer > 0f)
            return false;

        if (crouchHeld)
            return false;

        if (Mathf.Abs(moveInput) > 0.01f)
            _lastInputDirection = Mathf.Sign(moveInput);

        _isDashing = true;
        _dashTimer = dashDuration;
        _dashCooldownTimer = dashCooldown;
        _dashDirection = _lastInputDirection;

        landingSquash?.NotifyDashed();

        return true;
    }

    private void TickDash(float fixedDeltaTime)
    {
        _dashTimer -= fixedDeltaTime;

        float targetDashVelocityX = _dashDirection * dashSpeed;
        float newVelocityX = Mathf.MoveTowards(
            rb.linearVelocity.x,
            targetDashVelocityX,
            dashAcceleration * fixedDeltaTime
        );

        rb.linearVelocity = new Vector3(
            newVelocityX,
            rb.linearVelocity.y,
            rb.linearVelocity.z
        );

        _goalVelocity.x = newVelocityX;

        if (_dashTimer <= 0f)
            _isDashing = false;
    }
}