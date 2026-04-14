using Game.Enemy;
using UnityEngine;
using Zenject;

namespace Game.Zenject
{
    public class PrefabInstaller : MonoInstaller
    {
        [SerializeField] private EnemyController _enemyPrefab;

        public override void InstallBindings()
        {
            Container.Bind<EnemyController>().FromInstance(_enemyPrefab).AsSingle();
        }
    }
}