namespace Game.Enemy.Action
{
    [System.Serializable]
    public abstract class EnemyAction
    {
        public enum ActionStatus { Running, Success, Failure }

        public ActionStatus Status { get; set; }

        public virtual void Enter(BaseEnemyActionController owner)
        {
            Status = ActionStatus.Running;
        }

        public virtual void Process(BaseEnemyActionController owner, float dt) { }
        
        public virtual void Exit(BaseEnemyActionController owner) { }
    }
}