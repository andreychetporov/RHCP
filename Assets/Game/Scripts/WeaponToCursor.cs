using UnityEngine;

public class WeaponToCursor : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private Transform _target;

    [Header("Settings")]
    [SerializeField] private float _frequency = 15.0f;
    [SerializeField] private float _damping = 0.3f;

    private DampedSpringMotionParams _springParamMove;
    private Vector3 _springVelMove;

    private DampedSpringMotionParams _springParamRotate;
    private Vector3 _springVelRotate;

    public void Update()
    {
        // 1. Позиция (как было)
        MathSpring.CalculateSpringParams(ref _springParamMove, Time.deltaTime, _frequency, _damping);
        Vector3 position = transform.position;
        MathSpring.UpdateSpring(ref position, ref _springVelMove, _target.position, ref _springParamMove);
        transform.position = position;
    }
}