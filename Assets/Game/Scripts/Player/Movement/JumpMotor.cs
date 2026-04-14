using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class JumpMotor : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private GroundProbe groundProbe;
    [SerializeField] private HoverSuspension hoverSuspension;
    [SerializeField] private LandingSquash landingSquash;

    [Header("Jump")]
    [SerializeField] private float jumpHeight = 2.5f;
    [SerializeField] private float gravity = 20f;
    [SerializeField] private float jumpHoldGravityScale = 0.4f;

    [Header("Feel")]
    [Tooltip("Seconds after leaving ground where jump is still allowed.")]
    [SerializeField] private float coyoteTime = 0.12f;

    [Tooltip("Seconds before landing where jump input is remembered.")]
    [SerializeField] private float jumpBufferTime = 0.15f;

    [Tooltip("How long the suspension is suppressed after a jump so the spring doesn't pull the player down.")]
    [SerializeField] private float suspensionSuppressDuration = 0.25f;




    private float _coyoteTimer;
    private float _jumpBufferTimer;
    private float _suspensionSuppressTimer;
    private bool _isJumping;
    private bool _jumpHeld;


    private void Reset()
    {
        rb = GetComponent<Rigidbody>();
        groundProbe = GetComponent<GroundProbe>();
        hoverSuspension = GetComponent<HoverSuspension>();
    }

    private void Awake()
    {
        if (rb == null) rb = GetComponent<Rigidbody>();
        if (groundProbe == null) groundProbe = GetComponent<GroundProbe>();
        if (hoverSuspension == null) hoverSuspension = GetComponent<HoverSuspension>();
        if (landingSquash == null) landingSquash = GetComponent<LandingSquash>();
    }

    public void TickJump(bool jumpPressed, bool jumpReleased, bool jumpHeld, float dt)
    {
        _jumpHeld = jumpHeld;

        bool grounded = groundProbe != null && groundProbe.IsGrounded;

        if (grounded)
        {
            _coyoteTimer = coyoteTime;
            _isJumping = false;
        }
        else
        {
            _coyoteTimer = Mathf.Max(0f, _coyoteTimer - dt);
        }

        if (jumpPressed)
            _jumpBufferTimer = jumpBufferTime;
        else
            _jumpBufferTimer = Mathf.Max(0f, _jumpBufferTimer - dt);

        _suspensionSuppressTimer = Mathf.Max(0f, _suspensionSuppressTimer - dt);

        bool canJump = _coyoteTimer > 0f && !_isJumping;

        if (_jumpBufferTimer > 0f && canJump)
        {
            ExecuteJump();
            _jumpBufferTimer = 0f;
            _coyoteTimer = 0f;
        }

        ApplyJumpGravity(dt);

        if (hoverSuspension != null)
            hoverSuspension.SuspensionEnabled = _suspensionSuppressTimer <= 0f;
    }

    private void ExecuteJump()
    {
        _isJumping = true;
        _suspensionSuppressTimer = suspensionSuppressDuration;

        float jumpSpeed = Mathf.Sqrt(2f * jumpHeight * Physics.gravity.magnitude);

        Vector3 vel = rb.linearVelocity;
        vel.y = 0f;
        rb.linearVelocity = vel;

        rb.AddForce(Vector3.up * jumpSpeed, ForceMode.VelocityChange);
        landingSquash?.NotifyJumped();
    }

    private void ApplyJumpGravity(float dt)
    {
        if (groundProbe != null && groundProbe.IsGrounded)
            return;

        float verticalVel = rb.linearVelocity.y;

        if (verticalVel < 0f)
        {
            rb.AddForce(Vector3.down * gravity, ForceMode.Acceleration);
        }
        else if (_isJumping && !_jumpHeld)
        {
            rb.AddForce(Vector3.down * gravity * (1f - jumpHoldGravityScale), ForceMode.Acceleration);
        }
    }
}