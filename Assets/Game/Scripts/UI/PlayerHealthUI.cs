using Game.Level;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    [SerializeField] private Image[] peppers;

    private PlayerHealth _playerHealth;

    private void Start()
    {
        _playerHealth = LevelBootstrap.Instance.PlayerController.GetComponent<PlayerHealth>();
        _playerHealth.HealthChanged += PlayerHealth_HealthChanged;
    }

    private void PlayerHealth_HealthChanged(int oldValue, int newValue)
    {
        for (int i = 0; i < peppers.Length; i++)
        {
            if (peppers[i] == null) { continue; }

            peppers[i].gameObject.SetActive(i < _playerHealth.CurrentHealth);
        }
    }

    private void OnDestroy()
    {
        if (_playerHealth != null)
        {
            _playerHealth.HealthChanged -= PlayerHealth_HealthChanged;
        }
    }
}