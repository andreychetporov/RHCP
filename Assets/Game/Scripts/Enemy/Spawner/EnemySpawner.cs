using UnityEngine;
using Zenject;

namespace Game.Enemy.Spawner
{
    public class EnemySpawner : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeReference] private BaseSpawnTrigger _spawnTrigger;
        //spawn when - on Start, on player near, on last enemysDeath
        //нужен еще сколько раз спавнить, с каким интервалом, начальная задержка 
        //spawn where - List<Transform>
        //spawn who - List<EnemySO>


        [Inject] private readonly IEnemyFactory _enemyFactory;

        public void Update()
        {
            if (_spawnTrigger == null || !_spawnTrigger.CanSpawn(this)) { return; }

            
        }
    }
}