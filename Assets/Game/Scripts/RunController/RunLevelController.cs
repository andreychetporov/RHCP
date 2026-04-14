using System;
using UnityEngine;
using Zenject;

public class RunLevelController : MonoBehaviour
{
    [Header("Player Spawn")]
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private Transform _playerSpawnPoint;

    private DiContainer _container;
    private GameObject _spawnedPlayer;

    public bool IsRunStarted { get; private set; }
    public bool IsRunFinished { get; private set; }
    public float CurrentTime { get; private set; }

    public event Action OnRunStarted;
    public event Action<float> OnRunFinished;

    [Inject]
    public void Construct(DiContainer container)
    {
        _container = container;
    }

    private void Start()
    {
        SpawnPlayer();
    }

    private void Update()
    {
        if (IsRunStarted && !IsRunFinished)
        {
            CurrentTime += Time.deltaTime;
        }
    }

    private void SpawnPlayer()
    {
        if (_playerPrefab == null || _playerSpawnPoint == null)
        {
            Debug.LogWarning("RunLevelController: не назначен player prefab или spawn point");
            return;
        }

        _spawnedPlayer = _container.InstantiatePrefab(
            _playerPrefab,
            _playerSpawnPoint.position,
            _playerSpawnPoint.rotation,
            null
        );

        PlayerHealth playerHealth = _spawnedPlayer.GetComponent<PlayerHealth>();
        PlayerHealthUI healthUI = FindAnyObjectByType<PlayerHealthUI>();

        if (playerHealth != null && healthUI != null)
            healthUI.SetPlayerHealth(playerHealth);
    }

    public void StartRun()
    {
        if (IsRunStarted)
            return;

        IsRunStarted = true;
        IsRunFinished = false;
        CurrentTime = 0f;

        Debug.Log("Забег начался");
        OnRunStarted?.Invoke();
    }

    public void FinishRun()
    {
        if (!IsRunStarted || IsRunFinished)
            return;

        IsRunFinished = true;

        Debug.Log($"Забег завершён. Время: {CurrentTime:F2} сек");
        OnRunFinished?.Invoke(CurrentTime);
    }

    public void ResetRun()
    {
        IsRunStarted = false;
        IsRunFinished = false;
        CurrentTime = 0f;

        Debug.Log("Забег сброшен");
    }
}