using Game.Enemy;
using UnityEngine;
using Zenject;

namespace Game.Zenject
{
    public class FactoryInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IEnemyFactory>().To<EnemyFactory>().AsSingle();
        }
    }
}