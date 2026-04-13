#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace Game.Editor
{
    [CustomPropertyDrawer(typeof(Enemy.Action.EnemyAction), true)]
    public class EnemyActionDrawer : PropertyDrawer
    {
        private static ITypeGroupingStrategy _grouping = new AttributeGroupingStrategy();

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.managedReferenceValue == null || string.IsNullOrEmpty(property.managedReferenceFullTypename))
            {
                DrawTypeSelection(position, property, label);
            }
            else
            {
                DrawFullDrawer(position, property, label);
            }
        }

        private void DrawTypeSelection(Rect position, SerializedProperty property, GUIContent label)
        {
            Rect labelRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            EditorGUI.LabelField(labelRect, label, EditorStyles.boldLabel);

            Rect buttonRect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight + 2, position.width, EditorGUIUtility.singleLineHeight);
            if (GUI.Button(buttonRect, "Select Action Type (None)"))
            {
                SerializeReferenceDrawerHelper.ShowTypeSelectionMenu(property, typeof(Enemy.Action.EnemyAction), null, _grouping);
            }
        }

        private void DrawFullDrawer(Rect position, SerializedProperty property, GUIContent label)
        {
            string typeName = property.managedReferenceValue.GetType().Name;
            property.isExpanded = EditorGUI.Foldout(new Rect(position.x, position.y, position.width - 100, EditorGUIUtility.singleLineHeight), property.isExpanded, $"{label.text} ({typeName})", true);

            Rect btnRect = new Rect(position.x + position.width - 90, position.y, 90, EditorGUIUtility.singleLineHeight);
            if (GUI.Button(btnRect, "Change Type", EditorStyles.miniButton))
            {
                SerializeReferenceDrawerHelper.ShowTypeSelectionMenu(property, typeof(Enemy.Action.EnemyAction), null, _grouping);
            }

            if (property.isExpanded)
            {
                EditorGUI.indentLevel++;

                float yOffset = EditorGUIUtility.singleLineHeight + 2;
                SerializedProperty iterator = property.Copy();
                SerializedProperty end = iterator.GetEndProperty();

                bool enterChildren = true;
                while (iterator.NextVisible(enterChildren))
                {
                    if (SerializedProperty.EqualContents(iterator, end)) break;

                    float height = EditorGUI.GetPropertyHeight(iterator, true);
                    Rect childRect = new Rect(position.x, position.y + yOffset, position.width, height);
                    EditorGUI.PropertyField(childRect, iterator, true);

                    yOffset += height + 2;
                    enterChildren = false;
                }

                EditorGUI.indentLevel--;
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (property.managedReferenceValue == null) { return EditorGUIUtility.singleLineHeight * 2 + 5; }

            if (!property.isExpanded) { return EditorGUIUtility.singleLineHeight; }

            float height = EditorGUIUtility.singleLineHeight + 2;
            SerializedProperty iterator = property.Copy();
            SerializedProperty end = iterator.GetEndProperty();

            bool enterChildren = true;
            while (iterator.NextVisible(enterChildren))
            {
                if (SerializedProperty.EqualContents(iterator, end)) break;
                height += EditorGUI.GetPropertyHeight(iterator, true) + 2;
                enterChildren = false;
            }

            return height;
        }
    }
}

#endif