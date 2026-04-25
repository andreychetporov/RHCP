using Game.Audio;
using Game.Enemy;
using Game.Enemy.Slice;
using UnityEngine;
using Zenject;

public class PrefabInstaller : MonoInstaller
{
    [SerializeField] private EnemyController _enemyControllerPrefab;
    [SerializeField] private EnemySliced _slicedEnemyPrefab;
    [SerializeField] private SoundEmitter _soundEmitterPrefab;

    public override void InstallBindings()
    {
        Container.Bind<EnemyController>().FromInstance(_enemyControllerPrefab).AsSingle();
        Container.Bind<EnemySliced>().FromInstance(_slicedEnemyPrefab).AsSingle();
        Container.Bind<SoundEmitter>().FromInstance(_soundEmitterPrefab).AsSingle();
    }
}