using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float health;

    public void TakeDamage(float damage)
    {
        health -= damage;
        Debug.Log("damage");
        if (health <= 0) Die();
    }

    private void Die()
    {
        Debug.Log("Dead");
    }
    
}
