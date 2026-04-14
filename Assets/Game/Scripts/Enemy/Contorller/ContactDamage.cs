using UnityEngine;

public class ContactDamage : MonoBehaviour
{
    [SerializeField] private int damage = 1;

    private void OnCollisionStay(Collision collision)
    {
        TryDamage(collision.collider);
    }

    private void OnTriggerStay(Collider other)
    {
        TryDamage(other);
    }

    private void TryDamage(Collider other)
    {
        PlayerHealth health = other.GetComponent<PlayerHealth>();

        if (health == null)
            health = other.GetComponentInParent<PlayerHealth>();

        if (health == null)
            return;

        health.TakeDamage(damage);
    }
}