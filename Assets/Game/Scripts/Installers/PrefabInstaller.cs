using Game.Enemy;
using Game.Enemy.Slice;
using UnityEngine;
using Zenject;

public class PrefabInstaller : MonoInstaller
{
    [SerializeField] private EnemyController _enemyControllerPrefab;
    [SerializeField] private EnemySliced _slicedEnemyPrefab;

    public override void InstallBindings()
    {
        Container.Bind<EnemyController>().FromInstance(_enemyControllerPrefab).AsSingle();
        Container.Bind<EnemySliced>().FromInstance(_slicedEnemyPrefab).AsSingle();
    }
}