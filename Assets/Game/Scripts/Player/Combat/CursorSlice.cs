using UnityEngine;
using UnityEngine.InputSystem;
using Game.Enemy;

[RequireComponent(typeof(TrailRenderer))]
public class CursorSlice : MonoBehaviour
{
    [SerializeField] private TrailRenderer trail;
    [SerializeField] private float sliceLenght = 100.0f;
    [SerializeField] private float distanceFromCamera = 10f;
    private WeaponSO weapon;

    private Vector2 previousMSP; //MSP - mouse screen pos
    private Vector2 currentMSP; 

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
    public void SetWeapon(WeaponSO newWeapon)
    {
        weapon = newWeapon;
        trail.material = weapon.trailMaterial;
    }
    public void UpdateSlice()
    {
        currentMSP = Mouse.current.position.ReadValue();
        
        if (Vector2.Distance(previousMSP, currentMSP) > sliceLenght) 
        {
            Debug.Log(previousMSP);
            FindEnemy();
            previousMSP = currentMSP;
        }

        transform.position = GetMouseWorldPos();
    }

    private void FindEnemy()
    {
        RaycastHit[] hits = Physics.RaycastAll(previousMSP, currentMSP);
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray, out RaycastHit hit, 100.0f))
        {
            Debug.Log(hit.collider.gameObject.name);

            EnemyController enemy = hit.collider.GetComponentInParent<EnemyController>();

            if (enemy != null) {
                Debug.Log(enemy.EnemySO.Name);
                enemy.HealthController.TakeDamage(weapon.damage);
            }
        }

    }

    private Vector3 GetMouseWorldPos()
    {
        Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
        return Camera.main.ScreenToWorldPoint(new Vector3(mouseScreenPos.x, mouseScreenPos.y, distanceFromCamera));
    }

    public void Reset()
    {
        transform.position = GetMouseWorldPos();
        trail.Clear();
        previousMSP = Mouse.current.position.ReadValue();
    }

}
