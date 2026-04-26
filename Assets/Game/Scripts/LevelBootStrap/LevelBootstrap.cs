using Game.Audio;
using Game.Enemy.Slice;
using System;
using UnityEngine;
using Zenject;

namespace Game.Level
{
    public class LevelBootstrap : MonoBehaviour
    {
        public event Action OnRunStarted;
        public event Action<float> OnRunFinished;

        public static LevelBootstrap Instance { get; private set; }

        [Header("Player Spawn")]
        [SerializeField] private PlayerController _playerPrefab;
        [SerializeField] private Transform _playerSpawnPoint;

        [Header("Ambient")]
        [SerializeField] private SoundData _ambient;

        public PlayerController PlayerController { get; private set; }

        [Inject] public IEnemySliceFactory EnemySliceFactory { get; private set; }
        [Inject] private DiContainer _diContainer;

        public bool IsRunStarted { get; private set; }
        public bool IsRunFinished { get; private set; }
        public float CurrentTime { get; private set; }

        public void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(this);
                return;
            }
        }

        private void Start()
        {
            PlayerController = FindAnyObjectByType<PlayerController>();

            if (PlayerController == null) { SpawnPlayer(); }
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

            PlayerController = _diContainer.InstantiatePrefabForComponent<PlayerController>(_playerPrefab, _playerSpawnPoint.position, _playerSpawnPoint.rotation, null);
        }

        public void StartRun()
        {
            if (IsRunStarted) { return; }

            IsRunStarted = true;
            IsRunFinished = false;
            CurrentTime = 0f;

            SoundManager.Instance.Get().Initialize(_ambient).Play();

            Debug.Log("Забег начался");
            OnRunStarted?.Invoke();
        }

        public void FinishRun()
        {
            if (!IsRunStarted || IsRunFinished) { return; }

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

        public void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }
    }
}