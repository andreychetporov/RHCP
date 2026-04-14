namespace Game.Enemy.Spawner
{
    public abstract class BaseSpawnTrigger
    {
        public abstract bool CanSpawn(EnemySpawner enemySpawner);
    }
}