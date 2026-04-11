using UnityEngine;

namespace Game.Enemy.Action
{
    [System.Serializable]
    [EnemyActionCategory("Wait")]
    public class DelayEnemyAction : EnemyAction
    {
        [Header("Settings")]
        [SerializeField] private float _delayTime = 0.0f;

        private float _currnetTime = 0.0f;

        public override void Enter(BaseEnemyActionController owner)
        {
            base.Enter(owner);

            _currnetTime = 0.0f;
        }

        public override void Process(BaseEnemyActionController owner, float dt)
        {
            _currnetTime += dt;

            if (_currnetTime >= _delayTime)
            {
                Status = ActionStatus.Success;
            }
        }
    }
}