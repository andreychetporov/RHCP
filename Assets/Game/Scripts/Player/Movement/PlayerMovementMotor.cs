using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovementMotor : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private GroundProbe groundProbe;

    [Header("Movement")]
    [SerializeField] private float maxSpeed = 7f;
    [SerializeField] private float acceleration = 30f;
    [SerializeField] private float deceleration = 35f;
    [SerializeField] private float airAccelerationMultiplier = 0.5f;
    [SerializeField] private float maxAccelerationForce = 100f;

    private Vector3 _goalVelocity;

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
    }

    public void TickMove(float fixedDeltaTime)
    {
        bool isGrounded = groundProbe != null && groundProbe.IsGrounded;

        Vector3 targetGoalVelocity = Vector3.right * maxSpeed;

        float currentSpeedX = Mathf.Abs(rb.linearVelocity.x);
        float accelRate = currentSpeedX < maxSpeed ? acceleration : deceleration;

        if (!isGrounded)
            accelRate *= airAccelerationMultiplier;

        _goalVelocity = Vector3.MoveTowards(
            _goalVelocity,
            targetGoalVelocity,
            accelRate * fixedDeltaTime
        );

        Vector3 horizontalVelocity = new Vector3(rb.linearVelocity.x, 0f, 0f);
        Vector3 neededAccel = (_goalVelocity - horizontalVelocity) / fixedDeltaTime;
        neededAccel = Vector3.ClampMagnitude(neededAccel, maxAccelerationForce);

        rb.AddForce(neededAccel * rb.mass, ForceMode.Force);
    }
}