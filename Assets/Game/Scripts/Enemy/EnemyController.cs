using Game.Enemy.Action;
using UnityEditor;
using UnityEngine;

namespace Game.Enemy
{
    [RequireComponent(typeof(BaseEnemyActionController))]
    public class EnemyController : MonoBehaviour
    {
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
            HealthController.OnDeath += HealthController_OnDeath;
        }

        private void HealthController_OnDeath()
        {
            gameObject.SetActive(false);
        }

        private void HealthOnValueChanged(int arg1, int arg2)
        {
            AudioSource.PlayClipAtPoint(clip, transform.position);


        }


        private void SetupModel()
        {
            if (ActionController.VisualModel == null || EnemySO.ModelPrefab == null) { return; }

            for (int i = ActionController.VisualModel.childCount - 1; i >= 0; i--)
            {
                Destroy(ActionController.VisualModel.GetChild(i).gameObject);
            }

            Instantiate(EnemySO.ModelPrefab, ActionController.VisualModel);
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