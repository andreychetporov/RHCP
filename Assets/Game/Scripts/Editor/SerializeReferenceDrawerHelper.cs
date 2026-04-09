#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Game.Editor
{
    public static class SerializeReferenceDrawerHelper
    {
        public static void DrawSerializeReferenceField(SerializedProperty property, string label, Type baseType, Type requiredInterface = null)
        {
            var so = property.serializedObject;
            var value = property.managedReferenceValue;
            string typeName = value?.GetType().Name ?? "None";

            EditorGUILayout.BeginVertical("box");
            {
                EditorGUILayout.LabelField($"{label}: {typeName}", EditorStyles.miniBoldLabel);

                EditorGUILayout.BeginHorizontal();
                {
                    if (GUILayout.Button("Select Type"))
                    {
                        ShowTypeSelectionMenu(property, baseType, requiredInterface);
                    }

                    if (value != null && GUILayout.Button("Clear", GUILayout.Width(60)))
                    {
                        property.managedReferenceValue = null;
                        so.ApplyModifiedProperties();
                    }
                }
                EditorGUILayout.EndHorizontal();

                if (value != null)
                {
                    EditorGUILayout.Space();
                    EditorGUILayout.PropertyField(property, GUIContent.none, true);
                }
            }
            EditorGUILayout.EndVertical();
        }

        public static void DrawSerializeReferenceList(SerializedProperty listProperty, string label, Type baseType, Type requiredInterface = null)
        {
            EditorGUILayout.LabelField(label, EditorStyles.boldLabel);
            var so = listProperty.serializedObject;

            for (int i = 0; i < listProperty.arraySize; i++)
            {
                var element = listProperty.GetArrayElementAtIndex(i);
                var value = element.managedReferenceValue;
                string typeName = value?.GetType().Name ?? "None";

                EditorGUILayout.BeginVertical("box");
                {
                    EditorGUILayout.LabelField($"{label} {i}: {typeName}", EditorStyles.miniBoldLabel);

                    EditorGUILayout.BeginHorizontal();
                    {
                        if (GUILayout.Button("Select Type"))
                        {
                            ShowTypeSelectionMenu(element, baseType, requiredInterface);
                        }

                        if (value != null && GUILayout.Button("Clear", GUILayout.Width(60)))
                        {
                            element.managedReferenceValue = null;
                            so.ApplyModifiedProperties();
                        }

                        if (GUILayout.Button("Remove", GUILayout.Width(60)))
                        {
                            listProperty.DeleteArrayElementAtIndex(i);
                            so.ApplyModifiedProperties();
                            EditorGUILayout.EndVertical();
                            return;
                        }
                    }
                    EditorGUILayout.EndHorizontal();

                    if (value != null)
                    {
                        EditorGUILayout.Space();
                        EditorGUILayout.PropertyField(element, GUIContent.none, true);
                    }
                }
                EditorGUILayout.EndVertical();
                EditorGUILayout.Space();
            }

            if (GUILayout.Button($"Add {label}"))
            {
                listProperty.arraySize++;
                var newElem = listProperty.GetArrayElementAtIndex(listProperty.arraySize - 1);
                newElem.managedReferenceValue = null;
                so.ApplyModifiedProperties();
            }
        }

        public static void DrawSerializeReferenceListWithoutDetails(SerializedProperty listProperty, string label, Type baseType, Type requiredInterface = null)
        {
            EditorGUILayout.LabelField(label, EditorStyles.boldLabel);
            var so = listProperty.serializedObject;

            for (int i = 0; i < listProperty.arraySize; i++)
            {
                var element = listProperty.GetArrayElementAtIndex(i);
                var value = element.managedReferenceValue;
                string typeName = value?.GetType().Name ?? "None";

                EditorGUILayout.BeginVertical("box");
                {
                    EditorGUILayout.LabelField($"{label} {i}: {typeName}", EditorStyles.miniBoldLabel);

                    EditorGUILayout.BeginHorizontal();
                    {
                        if (GUILayout.Button("Select Type"))
                        {
                            ShowTypeSelectionMenu(element, baseType, requiredInterface);
                        }

                        if (value != null && GUILayout.Button("Clear", GUILayout.Width(60)))
                        {
                            element.managedReferenceValue = null;
                            so.ApplyModifiedProperties();
                        }

                        if (GUILayout.Button("Remove", GUILayout.Width(60)))
                        {
                            listProperty.DeleteArrayElementAtIndex(i);
                            so.ApplyModifiedProperties();
                            EditorGUILayout.EndVertical();
                            return;
                        }
                    }
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.EndVertical();
                EditorGUILayout.Space();
            }

            if (GUILayout.Button($"Add {label}"))
            {
                listProperty.arraySize++;
                var newElem = listProperty.GetArrayElementAtIndex(listProperty.arraySize - 1);
                newElem.managedReferenceValue = null;
                so.ApplyModifiedProperties();
            }
        }

        public static void ShowTypeSelectionMenu(SerializedProperty property, Type baseType, Type requiredInterface = null)
        {
            var menu = new GenericMenu();
            var types = FindAllDerivedTypes(baseType, requiredInterface);

            menu.AddItem(new GUIContent("None"), false, () =>
            {
                property.managedReferenceValue = null;
                property.serializedObject.ApplyModifiedProperties();
            });
            menu.AddSeparator("");

            foreach (var type in types)
            {
                string name = ObjectNames.NicifyVariableName(type.Name);
                menu.AddItem(new GUIContent(name), false, () =>
                {
                    try
                    {
                        property.managedReferenceValue = Activator.CreateInstance(type);
                        property.serializedObject.ApplyModifiedProperties();
                    }
                    catch (Exception e)
                    {
                        Debug.LogError($"Failed to create {type}: {e}");
                    }
                });
            }

            if (types.Count == 0)
                menu.AddDisabledItem(new GUIContent("No types found"));

            menu.ShowAsContext();
        }

        private static List<Type> FindAllDerivedTypes(Type baseType, Type requiredInterface = null)
        {
            var types = new List<Type>();
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                try
                {
                    var typesInAssembly = assembly.GetTypes()
                        .Where(t => t != null
                                 && !t.IsAbstract
                                 && !t.IsInterface
                                 && baseType.IsAssignableFrom(t)
                                 && (requiredInterface == null || t.GetInterfaces().Contains(requiredInterface)))
                        .ToList();
                    types.AddRange(typesInAssembly);
                }
                catch (ReflectionTypeLoadException) { }
            }
            return types.OrderBy(t => t.Name).ToList();
        }

        public static void DrawAllPropertiesExcluding(SerializedProperty property, params string[] exclude)
        {
            var iterator = property.Copy();
            var end = property.GetEndProperty();
            var excluded = new HashSet<string>(exclude);

            bool enterChildren = true;
            while (iterator.NextVisible(enterChildren))
            {
                if (SerializedProperty.EqualContents(iterator, end)) break;
                if (!excluded.Contains(iterator.name))
                    EditorGUILayout.PropertyField(iterator, true);
                enterChildren = false;
            }
        }

        public static void DrawSpritePreview(SerializedProperty property, GUIContent label)
        {
            property.objectReferenceValue = EditorGUILayout.ObjectField(label, property.objectReferenceValue, typeof(Sprite), false);
        }

        public static void DrawMultilineStringField(SerializedProperty property, GUIContent label, int minLines = 3)
        {
            if (!property.propertyType.Equals(SerializedPropertyType.String))
            {
                EditorGUILayout.HelpBox($"Property '{property.name}' is not a string!", MessageType.Error);
                return;
            }

            float lineHeight = EditorGUIUtility.singleLineHeight;
            float height = lineHeight * minLines + 4;

            EditorGUILayout.LabelField(label, EditorStyles.boldLabel);
            property.stringValue = EditorGUILayout.TextArea(property.stringValue, GUILayout.Height(height));
        }
    }
}

#endif