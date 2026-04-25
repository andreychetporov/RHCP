using Game.Audio;
using Game.Enemy.Action;
using Game.Enemy.Slice;
using Game.Level;
using UnityEditor;
using UnityEngine;
using Zenject;

namespace Game.Enemy
{
    [RequireComponent(typeof(BaseEnemyActionController))]
    public class EnemyController : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private EnemySO _enemySO;

        public BaseEnemyActionController ActionController { get; protected set; }

        public HealthPointController HealthController { get; protected set; }

        public EnemySO EnemySO { get; protected set; }

        [Inject] private IEnemySliceFactory _sliceFactory;

        private Transform _visualModel;

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

        private void HealthController_OnDeath()
        {
            if (_visualModel == null) { _visualModel = GetComponentInChildren<MeshRenderer>().transform; }

            LevelBootstrap.Instance.EnemySliceFactory.SpawnSlicedParts(_visualModel, EnemySO.MainColor, ActionController.TargetVelocity, transform.position, transform.forward);

            SoundManager.Instance.Get().Initialize(EnemySO.DeathSFX).Play();
            SoundManager.Instance.Get().Initialize(EnemySO.TakeDamageSFX).Play();

            gameObject.SetActive(false);
        }

        private void HealthOnValueChanged(int oldValue, int newValue)
        {
            if (HealthController.IsDead) { return; }

            if (newValue < oldValue)
            {
                SoundManager.Instance.Get().Initialize(EnemySO.TakeDamageSFX).Play();
            }
        }

        private void SetupModel()
        {
            if (ActionController.VisualModel == null || EnemySO.ModelPrefab == null) { return; }

            for (int i = ActionController.VisualModel.childCount - 1; i >= 0; i--)
            {
                Destroy(ActionController.VisualModel.GetChild(i).gameObject);
            }

            _visualModel = Instantiate(EnemySO.ModelPrefab, ActionController.VisualModel);
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