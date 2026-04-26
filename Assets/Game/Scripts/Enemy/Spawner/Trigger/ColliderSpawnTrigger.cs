using Game.Level;
using UnityEngine;

namespace Game.Enemy.Spawner
{
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(EnemySpawner))]
    public class ColliderSpawnTrigger : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private LayerMask _targetLayer;
        [SerializeField] private bool _triggerOnlyOnce = true;
        [SerializeField] private float _externSpawnValidation = 5.0f;

        private EnemySpawner _spawner;

        private bool _wasTriggered = false;

        public void Awake() { _spawner = GetComponent<EnemySpawner>(); }

        public void OnTriggerEnter(Collider other)
        {
            if (_wasTriggered && _triggerOnlyOnce) { return; }

            if (!CanTrigger()) { return; }

            if (((1 << other.gameObject.layer) & _targetLayer) != 0)
            {
                _spawner.StartSpawning();
                _wasTriggered = true;
            }
        }

        public bool CanTrigger() => (LevelBootstrap.Instance.PlayerController.transform.position - transform.position).sqrMagnitude >= (_externSpawnValidation * _externSpawnValidation);

#if UNITY_EDITOR
        public void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _externSpawnValidation);
        }
#endif
    }
}