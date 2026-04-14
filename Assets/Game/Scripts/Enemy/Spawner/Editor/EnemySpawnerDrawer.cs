#if UNITY_EDITOR

using Game.Enemy.Spawner;
using UnityEditor;

namespace Game.Editor
{
    [CustomEditor(typeof(EnemySpawner))]
    public class EnemySpawnerDrawer : UnityEditor.Editor
    {
        SerializedProperty _triggerProp;

        public void OnEnable()
        {
            _triggerProp = serializedObject.FindProperty("_spawnTrigger");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawBasicSettings();
            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Trigger", EditorStyles.boldLabel);
            SerializeReferenceDrawerHelper.DrawSerializeReferenceField(_triggerProp, "Trigger", typeof(BaseSpawnTrigger));
            EditorGUILayout.Space();

            serializedObject.ApplyModifiedProperties();
        }

        public void DrawBasicSettings()
        {
            EditorGUILayout.LabelField("Base Settings", EditorStyles.boldLabel);
        }
    }
}
#endif