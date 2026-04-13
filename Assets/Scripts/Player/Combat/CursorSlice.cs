using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(TrailRenderer))]
public class CursorSlice : MonoBehaviour
{
    [SerializeField] TrailRenderer trail;
    [SerializeField] private float distanceFromCamera = 10f;

    private void Awake()
    {
        if (trail == null)
            trail = GetComponent<TrailRenderer>();
        trail.emitting = false;
    }

    public void SetEmitting(bool isEmitting)
    {
        trail.emitting = isEmitting;
    }
    public void TickSlice()
    {
        Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(mouseScreenPos.x, mouseScreenPos.y, distanceFromCamera));
        transform.position = mousePos;
    }

}
