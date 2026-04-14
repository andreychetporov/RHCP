using Game.Enemy.Action;
using UnityEngine;

namespace Game.Enemy
{
    [System.Serializable]
    public struct EnemyCharacteristics
    {
        public int Damage;
        public int Health;
    }

    [CreateAssetMenu(fileName = "EnemySO", menuName = "Game/Enemy/EnemySO")]
    public class EnemySO : ScriptableObject
    {
        [Header("Visual")]
        public string Name;
        public string Description;

        [Space()]

        public Sprite Icon;
        public Transform ModelPrefab;

        [Header("Settings")]
        public EnemyActionBehaviorSO Behavior;
        public EnemyCharacteristics Characteristics;
    }
}