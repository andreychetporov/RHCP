using DG.Tweening;
using Game.Audio;
using Game.Blood;
using Game.Enemy.Action;
using Game.Enemy.Slice;
using Game.Level;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Game.Enemy
{
    [RequireComponent(typeof(BaseEnemyActionController))]
    public class EnemyController : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private EnemySO _enemySO;

        [Header("Squash & Stretch")]
        [SerializeField] private float _punchScaleAmount = 0.3f;
        [SerializeField] private float _punchDuration = 0.35f;
        [SerializeField] private int _punchVibrato = 6;
        [SerializeField] private float _punchElasticity = 0.5f;
        public BaseEnemyActionController ActionController { get; protected set; }

        public HealthPointController HealthController { get; protected set; }

        public EnemySO EnemySO { get; protected set; }

        private Transform _visualModel;

        private Tween _punchTween;


        private void Awake()
        {
            ActionController = GetComponent<BaseEnemyActionController>();
            if (_enemySO != null) { Initialize(_enemySO); }
        }

        public void Initialize(EnemySO enemySO)
        {
            EnemySO = enemySO;

            ActionController.Initialize(EnemySO.Behavior);
            HealthController = new HealthPointController(EnemySO.Characteristics.Health);

            SetupModel();

            gameObject.name = $"Enemy_{EnemySO.Name}";
            HealthController.Health.OnValueChanged += HealthOnValueChanged;
            HealthController.OnDeath += HealthController_OnDeath;
        }

        public void PlayHit()
        {
            SoundManager.Instance.Get().Initialize(EnemySO.TakeDamageSFX).Play();
            BloodManager.Instance.GetForDamage().Initialize(transform.position, EnemySO.MainColor).Play();
            BloodCanvas.Instance.SpawnBloodSpot(EnemySO.MainColor);

            PlayPunch();
        }

        private void PlayPunch()
        {
            _punchTween?.Kill(complete: true);

            Vector3 punch = new Vector3(
                -_punchScaleAmount,
                 _punchScaleAmount,
                -_punchScaleAmount
            );

            _punchTween = transform
                .DOPunchScale(punch, _punchDuration, _punchVibrato, _punchElasticity)
                .SetEase(Ease.OutQuad);
        }

        private void HealthController_OnDeath()
        {

            if (_visualModel == null) { _visualModel = GetComponentInChildren<MeshRenderer>().transform; }

            LevelBootstrap.Instance.EnemySliceFactory.SpawnSlicedParts(_visualModel, EnemySO.MainColor, ActionController.TargetVelocity, transform.position, transform.forward);

            SoundManager.Instance.Get().Initialize(EnemySO.DeathSFX).Play();
            SoundManager.Instance.Get().Initialize(EnemySO.TakeDamageSFX).Play();

            BloodCanvas.Instance.SpawnBloodSpot(EnemySO.MainColor);

            CoinsManager.Instance.SpawnCoins(transform.position, EnemySO.coinsAmont);
            ParticlesManager.Instance.SpawnParticles(transform.position, EnemySO.ultaParticlesAmount);

            gameObject.SetActive(false);
        }

        private void HealthOnValueChanged(int oldValue, int newValue)
        {
            if (HealthController.IsDead) { return; }

            if (newValue < oldValue) { PlayHit(); }
        }

        private void SetupModel()
        {
            if (ActionController.VisualModel == null || EnemySO.ModelPrefab == null) { return; }

            for (int i = ActionController.VisualModel.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(ActionController.VisualModel.GetChild(i).gameObject);
            }

            _visualModel = Instantiate(EnemySO.ModelPrefab, ActionController.VisualModel);
        }

        private void OnDestroy()
        {
            _punchTween?.Kill();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            EditorApplication.delayCall += () =>
            {
                if (this == null) { return; }

                if (_enemySO == null) { return; }
                EnemySO = _enemySO;

                BaseEnemyActionController temp = GetComponent<BaseEnemyActionController>();
                if (temp == null || temp.VisualModel == null) { return; }

                for (int i = temp.VisualModel.childCount - 1; i >= 0; i--)
                {
                    DestroyImmediate(temp.VisualModel.GetChild(i).gameObject);
                }

                if (EnemySO.ModelPrefab != null)
                {
                    Instantiate(EnemySO.ModelPrefab, temp.VisualModel);
                }

                gameObject.name = $"Enemy_{EnemySO.Name}";
            };
        }
#endif
    }
}