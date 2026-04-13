using UnityEngine;

namespace Game.Enemy.Action
{
    [CreateAssetMenu(fileName = "EnemyActionBeheviorSO", menuName = "Game/Enemy/EnemyActionBehevior")]
    public class EnemyActionBehaviorSO : ScriptableObject
    {
        [SerializeReference] public EnemyAction RootAction;

        public EnemyActionBehaviorSO Clone() => Instantiate(this);
    }
}