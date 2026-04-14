using UnityEngine;

namespace Game.Enemy.Spawner
{
    [RequireComponent(typeof(EnemySpawner))]
    public class StartSpawnTrigger : MonoBehaviour
    {
        public void Start() => GetComponent<EnemySpawner>().StartSpawning();
    }
}