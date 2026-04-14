using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private Image[] peppers;
    [SerializeField] private Color fullColor = Color.white;
    [SerializeField] private Color emptyColor = new Color(1f, 1f, 1f, 0.25f);

    public void SetPlayerHealth(PlayerHealth newPlayerHealth)
    {
        if (playerHealth != null)
            playerHealth.HealthChanged -= OnHealthChanged;

        playerHealth = newPlayerHealth;

        if (playerHealth != null)
        {
            playerHealth.HealthChanged += OnHealthChanged;
            Refresh(playerHealth.CurrentHealth);
        }
    }

    private void OnDestroy()
    {
        if (playerHealth != null)
            playerHealth.HealthChanged -= OnHealthChanged;
    }

    private void OnHealthChanged(int currentHealth, int maxHealth)
    {
        Refresh(currentHealth);
    }

    private void Refresh(int currentHealth)
    {
        for (int i = 0; i < peppers.Length; i++)
        {
            if (peppers[i] == null)
                continue;

            peppers[i].color = i < currentHealth ? fullColor : emptyColor;
        }
    }
}