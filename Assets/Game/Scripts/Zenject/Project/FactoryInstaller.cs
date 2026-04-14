using Game.Enemy;
using UnityEngine;
using Zenject;

public class FactoryInstaller : MonoInstaller
{
    [SerializeField] private EnemyController enemyControllerPrefab;

    public override void InstallBindings()
    {
        Container.Bind<EnemyController>().FromInstance(enemyControllerPrefab).AsSingle();
        Container.Bind<IEnemyFactory>().To<EnemyFactory>().AsSingle();
        Container.Bind<IPlayerInput>().To<PlayerInputReader>().AsSingle();
    }
}