namespace Game.Enemy.Action
{
    public abstract class ConditionEnemyAction : EnemyAction
    {
        public abstract bool Evaluate(BaseEnemyActionController owner);
    }
}