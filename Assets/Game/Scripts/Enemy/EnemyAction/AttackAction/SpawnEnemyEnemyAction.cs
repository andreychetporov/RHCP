using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Game.Enemy;

namespace Game.Enemy.Action
{
    [Serializable]
    public struct EnemySpawnData
    {
        [SerializeField] private Vector3 _offset;
        [SerializeField] private Vector3 _initialRotationEuler;

        public Vector3 Offset => _offset;
        public Vector3 InitialRotationEuler => _initialRotationEuler;
    }

    [Serializable]
    [EnemyActionCategory("Attack")]
    public class SpawnEnemyEnemyAction : EnemyAction
    {
        [Header("Spawn Settings")]
        [SerializeField] private EnemySO _enemySO;
        [SerializeField] private List<EnemySpawnData> _spawnDataList = new();

        public override void Enter(BaseEnemyActionController owner)
        {
            base.Enter(owner);

            if (owner == null)
            {
                Status = ActionStatus.Failure;
                return;
            }

            if (_enemySO == null)
            {
                Debug.LogWarning($"[{nameof(SpawnEnemyEnemyAction)}] EnemySO is null");
                Status = ActionStatus.Failure;
                return;
            }

            if (_spawnDataList == null || _spawnDataList.Count == 0)
            {
                Debug.LogWarning($"[{nameof(SpawnEnemyEnemyAction)}] Spawn data list is empty");
                Status = ActionStatus.Failure;
                return;
            }

            IEnemyFactory enemyFactory = ProjectContext.Instance.Container.Resolve<IEnemyFactory>();

            if (enemyFactory == null)
            {
                Debug.LogWarning($"[{nameof(SpawnEnemyEnemyAction)}] IEnemyFactory not found");
                Status = ActionStatus.Failure;
                return;
            }

            for (int i = 0; i < _spawnDataList.Count; i++)
            {
                EnemySpawnData spawnData = _spawnDataList[i];

                Vector3 spawnPosition =
                    owner.transform.position +
                    owner.transform.TransformDirection(spawnData.Offset);

                Quaternion spawnRotation =
                    owner.transform.rotation *
                    Quaternion.Euler(spawnData.InitialRotationEuler);

                EnemyController spawnedEnemy = enemyFactory.CreateEnemy(_enemySO, owner.transform);

                if (spawnedEnemy == null)
                {
                    Debug.LogWarning($"[{nameof(SpawnEnemyEnemyAction)}] Failed to spawn enemy");
                    continue;
                }

                spawnedEnemy.transform.SetPositionAndRotation(spawnPosition, spawnRotation);
            }

            Status = ActionStatus.Success;
        }
    }
}