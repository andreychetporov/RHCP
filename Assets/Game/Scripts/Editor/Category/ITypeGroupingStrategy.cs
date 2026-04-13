#if UNITY_EDITOR

using System;

namespace Game.Editor
{
    public interface ITypeGroupingStrategy
    {
        public string GetCategory(Type type);
    }
}

#endif