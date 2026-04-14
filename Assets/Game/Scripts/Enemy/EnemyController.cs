using Game.Enemy.Action;
using UnityEditor;
using UnityEngine;

namespace Game.Enemy
{
    [RequireComponent(typeof(BaseEnemyActionController))]
    public class EnemyController : MonoBehaviour
    {
        [Header("Referenece")]
        [SerializeField] private Transform _meshParent;

        [Header("Settings")]
        [SerializeField] private EnemySO _enemySO;
        private MeshRenderer[] meshRenderer;

        [SerializeField] private AudioClip clip;
        public BaseEnemyActionController ActionController { get; protected set; }

        public HealthPointController HealthController { get; protected set; }

        public EnemySO EnemySO { get; protected set; }

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
        }
        private void HealthOnValueChanged(int arg1, int arg2)
        {
            AudioSource.PlayClipAtPoint(clip, transform.position);
        }


        private void SetupModel()
        {
            if (_meshParent == null || EnemySO.ModelPrefab == null) { return; }

            for (int i = _meshParent.childCount - 1; i >= 0; i--)
            {
                Destroy(_meshParent.GetChild(i).gameObject);
            }

            Instantiate(EnemySO.ModelPrefab, _meshParent);
            
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            EditorApplication.delayCall += () =>
            {
                if (this == null || _meshParent == null) { return; }
                if (_enemySO == null) { return; }

                EnemySO = _enemySO;

                if (_meshParent == null)
                    return;

                for (int i = _meshParent.childCount - 1; i >= 0; i--)
                {
                    DestroyImmediate(_meshParent.GetChild(i).gameObject);
                }

                if (EnemySO.ModelPrefab != null)
                {
                    Instantiate(EnemySO.ModelPrefab, _meshParent);
                }

                gameObject.name = $"Enemy_{EnemySO.Name}";
            };
        }
#endif
    }
}