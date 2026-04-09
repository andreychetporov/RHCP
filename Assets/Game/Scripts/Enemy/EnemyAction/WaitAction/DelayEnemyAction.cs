using UnityEngine;

namespace Game.Enemy.Action
{
    [System.Serializable]
    public class DelayEnemyAction : EnemyAction
    {
        [Header("Settings")]
        [SerializeField] private float _delayTime = 0.0f;

        private float _currnetTime = 0.0f;

        public override void Enter(EnemyActionController owner)
        {
            base.Enter(owner);

            _currnetTime = 0.0f;
        }

        public override void Process(EnemyActionController owner, float dt)
        {
            _currnetTime += dt;

            if (_currnetTime >= _delayTime)
            {
                Status = ActionStatus.Success;
            }
        }
    }
}