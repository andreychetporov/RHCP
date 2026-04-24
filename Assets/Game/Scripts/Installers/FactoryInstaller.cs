using Game.Enemy;
using Game.Enemy.Slice;
using Zenject;

public class FactoryInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<IPlayerInput>().To<PlayerInputReader>().AsSingle();

        Container.Bind<IEnemyFactory>().To<EnemyFactory>().AsSingle();
        Container.Bind<IEnemySliceFactory>().To<EnemyScliceFactory>().AsSingle();
    }
}