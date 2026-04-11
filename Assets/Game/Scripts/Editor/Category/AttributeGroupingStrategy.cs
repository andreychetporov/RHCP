using Game.Enemy.Action;
using System;
using System.Reflection;

namespace Game.Editor
{
    public class AttributeGroupingStrategy : ITypeGroupingStrategy
    {
        public string GetCategory(Type type)
        {
            var attr = type.GetCustomAttribute<EnemyActionCategoryAttribute>();
            return attr?.Category ?? "Uncategorized";
        }
    }
}