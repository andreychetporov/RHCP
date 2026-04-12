using UnityEngine;

public class LandingSquash : MonoBehaviour
{
    [SerializeField] private Transform model;

    [SerializeField] private float squashOnJump = 0.2f;
    [SerializeField] private float squashOnLandMin = 0.1f;
    [SerializeField] private float squashOnLandMax = 0.5f;
    [SerializeField] private float landSpeedMin = 1f;
    [SerializeField] private float landSpeedMax = 15f;
    [SerializeField] private float springStiffness = 280f;
    [SerializeField] private float springDamping = 18f;

    private GroundProbe _groundProbe;
    private Rigidbody _rb;
    private bool _wasGrounded;

    private float _springValue;
    private float _springVelocity;

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

    private void Update()
    {
        bool grounded = _groundProbe != null && _groundProbe.IsGrounded;

        if (grounded && !_wasGrounded)
            OnLand();

        _wasGrounded = grounded;

        float springForce = -springStiffness * _springValue;
        float dampForce = -springDamping * _springVelocity;

        _springVelocity += (springForce + dampForce) * Time.deltaTime;
        _springValue += _springVelocity * Time.deltaTime;

        float scaleY = _baseScale.y * (1f + _springValue);
        float scaleX = _baseScale.x * (1f - _springValue * 0.5f);

        model.localScale = new Vector3(scaleX, scaleY, scaleX);
    }

    private void OnLand()
    {
        float impactSpeed = _rb != null ? Mathf.Abs(_rb.linearVelocity.y) : landSpeedMax;
        float t = Mathf.InverseLerp(landSpeedMin, landSpeedMax, impactSpeed);
        float squash = Mathf.Lerp(squashOnLandMin, squashOnLandMax, t);

        _springVelocity -= squash * springStiffness * Time.fixedDeltaTime;
    }
}