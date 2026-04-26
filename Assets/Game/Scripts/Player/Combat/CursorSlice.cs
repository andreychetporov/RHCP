using UnityEngine;
using UnityEngine.InputSystem;
using Game.Enemy;
using Game.Audio;

[RequireComponent(typeof(TrailRenderer))]
public class CursorSlice : MonoBehaviour
{

    [SerializeField] private TrailRenderer _trail;
    [SerializeField] private float _sliceLenght = 100.0f;
    [SerializeField] private float _distanceFromCamera = 10.0f;

    [SerializeField] private LayerMask ignoreMask;
    private WeaponSO weapon;


    private WeaponSO _weapon;

    private Vector2 _previousMSP;

    private void Awake()
    {
        if (_trail == null) { _trail = GetComponent<TrailRenderer>(); }
        _trail.emitting = false;
    }

    public void SetEmitting(bool isEmitting) => _trail.emitting = isEmitting;

    public void SetWeapon(WeaponSO newWeapon)
    {
        _weapon = newWeapon;
        _trail.material = _weapon.TrailMaterial;
    }

    public void UpdateSlice()
    {
        Vector2 currentMSP = Mouse.current.position.ReadValue();
        
        if (Vector2.Distance(_previousMSP, currentMSP) > _sliceLenght) 
        {
            Debug.Log("FIND");
            FindEnemy();
            _previousMSP = currentMSP;
        }

        transform.position = GetMouseWorldPos();
    }

    private void FindEnemy()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.SphereCast(ray, 0.5f, out RaycastHit hit, 1000.0f))

        {
            EnemyController enemy = hit.collider.GetComponentInParent<EnemyController>();
            if (enemy != null)
            {
                Debug.Log(enemy.EnemySO.Name);
                enemy.HealthController.TakeDamage(_weapon.Damage);
                SoundManager.Instance.Get().Initialize(_weapon.HitSound).Play();
            }
        }
    }

    private Vector3 GetMouseWorldPos()
    {
        Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
        return Camera.main.ScreenToWorldPoint(new Vector3(mouseScreenPos.x, mouseScreenPos.y, _distanceFromCamera));
    }

    public void Reset()
    {
        transform.position = GetMouseWorldPos();
        _trail.Clear();
        _previousMSP = Mouse.current.position.ReadValue();
    }

}
