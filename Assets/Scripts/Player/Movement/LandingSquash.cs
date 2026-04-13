using UnityEngine;

public class LandingSquash : MonoBehaviour
{
    [SerializeField] private Transform model;

    [Header("Jump")]
    [SerializeField] private float squashOnJump = 0.2f;

    [Header("Dash")]
    [SerializeField] private float dashStretchAmount = 0.22f;
    [SerializeField] private float dashSpringStiffness = 180f;
    [SerializeField] private float dashSpringDamping = 20f;

    [Header("Land")]
    [SerializeField] private float squashOnLandMin = 0.1f;
    [SerializeField] private float squashOnLandMax = 0.5f;
    [SerializeField] private float landSpeedMin = 1f;
    [SerializeField] private float landSpeedMax = 15f;

    [Header("Main Spring")]
    [SerializeField] private float springStiffness = 280f;
    [SerializeField] private float springDamping = 18f;

    private GroundProbe _groundProbe;
    private Rigidbody _rb;
    private bool _wasGrounded;

    private float _springValue;
    private float _springVelocity;

    private float _dashValue;
    private float _dashVelocity;
    private float _dashTarget;

    private Vector3 _baseScale;

    private void Awake()
    {
        _groundProbe = GetComponent<GroundProbe>();
        _rb = GetComponent<Rigidbody>();
        _baseScale = model.localScale;
    }

    public void NotifyJumped()
    {
        _springVelocity += squashOnJump * springStiffness * Time.fixedDeltaTime;
    }

    public void NotifyDashed()
    {
        _dashTarget = dashStretchAmount;
    }

    private void Update()
    {
        bool grounded = _groundProbe != null && _groundProbe.IsGrounded;

        if (grounded && !_wasGrounded)
            OnLand();

        _wasGrounded = grounded;

        UpdateMainSpring(Time.deltaTime);
        UpdateDashSpring(Time.deltaTime);
        ApplyScale();
    }

    private void UpdateMainSpring(float dt)
    {
        float springForce = -springStiffness * _springValue;
        float dampForce = -springDamping * _springVelocity;

        _springVelocity += (springForce + dampForce) * dt;
        _springValue += _springVelocity * dt;
    }

    private void UpdateDashSpring(float dt)
    {
        float displacement = _dashValue - _dashTarget;
        float springForce = -dashSpringStiffness * displacement;
        float dampForce = -dashSpringDamping * _dashVelocity;

        _dashVelocity += (springForce + dampForce) * dt;
        _dashValue += _dashVelocity * dt;

        _dashTarget = Mathf.MoveTowards(_dashTarget, 0f, dashStretchAmount * 6f * dt);
    }

    private void ApplyScale()
    {
        float jumpScaleY = 1f + _springValue;
        float jumpScaleXZ = 1f - _springValue * 0.5f;

        float dashScaleX = 1f + _dashValue;
        float dashScaleY = 1f - _dashValue * 0.35f;
        float dashScaleZ = 1f - _dashValue * 0.2f;

        model.localScale = new Vector3(
            _baseScale.x * jumpScaleXZ * dashScaleX,
            _baseScale.y * jumpScaleY * dashScaleY,
            _baseScale.z * jumpScaleXZ * dashScaleZ
        );
    }

    private void OnLand()
    {
        float impactSpeed = _rb != null ? Mathf.Abs(_rb.linearVelocity.y) : landSpeedMax;
        float t = Mathf.InverseLerp(landSpeedMin, landSpeedMax, impactSpeed);
        float squash = Mathf.Lerp(squashOnLandMin, squashOnLandMax, t);

        _springVelocity -= squash * springStiffness * Time.fixedDeltaTime;
    }
}