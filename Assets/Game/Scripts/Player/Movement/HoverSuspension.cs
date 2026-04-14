using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class HoverSuspension : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private GroundProbe groundProbe;

    [Header("Suspension")]
    [SerializeField] private float rideHeight = 1.0f;
    [SerializeField] private float springStrength = 120f;
    [SerializeField] private float springDamper = 25f;
    [SerializeField] private float maxSpringForce = 200f;

    public bool SuspensionEnabled { get; set; } = true;

    private void Reset()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void TickSuspension()
    {
        if (!SuspensionEnabled)
            return;

        if (groundProbe == null || !groundProbe.HasHit)
            return;

        Vector3 rayDir = Vector3.down;

        Rigidbody hitBody = groundProbe.GroundRigidbody;

        Vector3 bodyVelocity = rb.linearVelocity;
        Vector3 hitBodyVelocity = hitBody != null ? hitBody.linearVelocity : Vector3.zero;

        float bodyVelAlongRay = Vector3.Dot(rayDir, bodyVelocity);
        float hitVelAlongRay = Vector3.Dot(rayDir, hitBodyVelocity);
        float relativeVel = bodyVelAlongRay - hitVelAlongRay;

        float currentHeight = groundProbe.GroundDistance;
        float heightError = currentHeight - rideHeight;

        float springForce = (heightError * springStrength) - (relativeVel * springDamper);
        springForce = Mathf.Clamp(springForce, -maxSpringForce, maxSpringForce);

        rb.AddForce(rayDir * springForce, ForceMode.Acceleration);

        if (hitBody != null)
        {
            hitBody.AddForceAtPosition(rayDir * -springForce, groundProbe.Hit.point, ForceMode.Acceleration);
        }
    }
}