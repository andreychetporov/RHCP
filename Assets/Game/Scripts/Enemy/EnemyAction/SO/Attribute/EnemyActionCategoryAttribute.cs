using System;

namespace Game.Enemy.Action
{
    [AttributeUsage(AttributeTargets.Class)]
    public class EnemyActionCategoryAttribute : Attribute
    {
        public string Category { get; }

        public EnemyActionCategoryAttribute(string category)
        {
            Category = category;
        }
    }
}