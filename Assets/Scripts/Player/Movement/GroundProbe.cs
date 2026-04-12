using UnityEngine;

public class GroundProbe : MonoBehaviour
{
    [Header("Probe")]
    [SerializeField] private Transform probeOrigin;
    [SerializeField] private float probeRadius = 0.3f;
    [SerializeField] private float probeLength = 1.5f;
    [SerializeField] private LayerMask groundMask;

    public bool HasHit { get; private set; }
    public RaycastHit Hit { get; private set; }

    public bool IsGrounded => HasHit;
    public float GroundDistance => HasHit ? Hit.distance : float.PositiveInfinity;
    public Vector3 GroundNormal => HasHit ? Hit.normal : Vector3.up;
    public Rigidbody GroundRigidbody => HasHit ? Hit.rigidbody : null;

    public void TickProbe()
    {
        HasHit = Physics.SphereCast(
            probeOrigin.position,
            probeRadius,
            Vector3.down,
            out RaycastHit hit,
            probeLength,
            groundMask,
            QueryTriggerInteraction.Ignore
        );

        if (HasHit)
            Hit = hit;
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (probeOrigin == null)
            return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(probeOrigin.position, probeRadius);

        Vector3 end = probeOrigin.position + Vector3.down * probeLength;
        Gizmos.DrawLine(probeOrigin.position, end);
        Gizmos.DrawWireSphere(end, probeRadius);
    }
#endif
}