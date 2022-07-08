using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

[CustomPropertyDrawer(typeof(DisableInInspectorAttribute))]
public class DisableInInspectorDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginDisabledGroup(true);
        EditorGUI.PropertyField(position, property, label, true);
        EditorGUI.EndDisabledGroup();
    }
}
#endif