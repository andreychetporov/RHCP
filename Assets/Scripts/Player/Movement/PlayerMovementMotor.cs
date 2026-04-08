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

    public void TickMove(float moveInput, float fixedDeltaTime)
    {
        bool isGrounded = groundProbe != null && groundProbe.IsGrounded;

        Vector3 moveDirection = Vector3.right * moveInput;
        Vector3 targetGoalVelocity = moveDirection * maxSpeed;

        float accelRate;

        if (Mathf.Abs(moveInput) > 0.01f)
            accelRate = acceleration;
        else
            accelRate = deceleration;

        if (!isGrounded)
            accelRate *= airAccelerationMultiplier;

        _goalVelocity = Vector3.MoveTowards(
            _goalVelocity,
            targetGoalVelocity,
            accelRate * fixedDeltaTime
        );

        Vector3 currentVelocity = rb.linearVelocity;
        Vector3 neededAccel = (_goalVelocity - currentVelocity) / fixedDeltaTime;

        neededAccel.y = 0f;
        neededAccel.z = 0f;

        neededAccel = Vector3.ClampMagnitude(neededAccel, maxAccelerationForce);

        rb.AddForce(neededAccel * rb.mass, ForceMode.Force);
    }
}