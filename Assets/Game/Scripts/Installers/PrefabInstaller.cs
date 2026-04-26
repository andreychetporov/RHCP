using Game.Audio;
using Game.Blood;
using Game.Enemy;
using Game.Enemy.Slice;
using UnityEngine;
using Zenject;

public class PrefabInstaller : MonoInstaller
{
    [SerializeField] private EnemyController _enemyControllerPrefab;
    [SerializeField] private EnemySliced _slicedEnemyPrefab;

    [Space()]

    [SerializeField] private BloodEmitter _bloodEmitterTakeDamagePrefab;
    [SerializeField] private BloodEmitter _bloodEmitterKillPrefab;

    [Space()]

    [SerializeField] private SoundEmitter _soundEmitterPrefab;

    public override void InstallBindings()
    {
        Container.Bind<EnemyController>().FromInstance(_enemyControllerPrefab).AsSingle();
        Container.Bind<EnemySliced>().FromInstance(_slicedEnemyPrefab).AsSingle();

        Container.Bind<BloodEmitter>().WithId("SMALL").FromInstance(_bloodEmitterTakeDamagePrefab);
        Container.Bind<BloodEmitter>().WithId("BIG").FromInstance(_bloodEmitterKillPrefab);

        Container.Bind<SoundEmitter>().FromInstance(_soundEmitterPrefab).AsSingle();
    }
}