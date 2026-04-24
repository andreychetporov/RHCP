using UnityEngine;
using UnityEngine.InputSystem;

public class ProjectileWeapon : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private WeaponProjectile projectilePrefab;
    [SerializeField] private Transform shootPoint;

    [Header("Settings")]
    [SerializeField] private WeaponSO weapon;
    [SerializeField] private float projectileSpeed = 20f;
    [SerializeField] private float maxDistance = 100f;
    [SerializeField] private LayerMask aimLayers = ~0;

    private void Awake()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        if (shootPoint == null)
            shootPoint = transform;
    }

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        if (projectilePrefab == null || weapon == null || mainCamera == null)
            return;

        Vector3 mouseWorld = mainCamera.ScreenToWorldPoint(
            new Vector3(
                Mouse.current.position.ReadValue().x,
                Mouse.current.position.ReadValue().y,
                Mathf.Abs(mainCamera.transform.position.z)
            )
        );

        mouseWorld.z = 0f;

        Vector3 spawnPos = shootPoint.position;
        spawnPos.z = 0f;

        WeaponProjectile projectile = Instantiate(
            projectilePrefab,
            spawnPos,
            Quaternion.identity
        );

        projectile.Initialize(mouseWorld, projectileSpeed, weapon.damage);
    }
}