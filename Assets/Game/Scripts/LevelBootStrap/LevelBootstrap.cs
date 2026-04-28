using DG.Tweening;
using Game.Audio;
using Game.Enemy.Slice;
using Game.SceneLoaderSystem;
using System;
using System.Threading.Tasks;
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

        [Header("DeathSound")]
        [SerializeField] private SoundData _deathSound;

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
                Destroy(gameObject);
                return;
            }

            PlayerController = FindAnyObjectByType<PlayerController>();

            if (PlayerController == null) { SpawnPlayer(); }

            PlayerController.GetComponent<PlayerHealth>().Died += async () =>
            {
                PlayerController.GetComponent<PlayerMovementMotor>().enabled = false;

                SoundManager.Instance.StopAllSound();

                PlayerController.transform.DOScale(Vector3.one * 2.0f, 0.3f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);

                SoundManager.Instance.Get().Initialize(_deathSound).Play();

                await Task.Delay(1500);

                SceneLoader.Instance.LoadScene(SceneEnum.MapLevel);
            };

        }

        private void Start()
        {
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

            SceneLoader.Instance.LoadScene(SceneEnum.MapLevel);
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