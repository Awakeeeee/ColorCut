using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LevelDesignParam))]
public class LevelDesignParamCustomEditor : Editor
{
    private SerializedProperty id_property;
    private SerializedProperty name_property;
    private SerializedProperty color_property;
    private SerializedProperty lines_property;

    GUIStyle slabel;
    GUIStyle swarnning;

    private void OnEnable()
    {
        id_property = serializedObject.FindProperty("level_ID");
        name_property = serializedObject.FindProperty("level_name");
        color_property = serializedObject.FindProperty("pass_color");
        lines_property = serializedObject.FindProperty("lines");

        slabel = new GUIStyle();
        slabel.fontSize = 17;
        slabel.normal.textColor = Color.green;

        swarnning = new GUIStyle();
        swarnning.fontSize = 12;
        swarnning.normal.textColor = Color.yellow;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        //DrawDefaultInspector();
        EditorGUILayout.LabelField("General level config", slabel);
        EditorGUILayout.BeginVertical(GUI.skin.box);
        EditorGUI.indentLevel++;
        EditorGUILayout.PropertyField(id_property);
        EditorGUILayout.PropertyField(name_property);
        EditorGUILayout.PropertyField(color_property);
        EditorGUI.indentLevel--;
        EditorGUILayout.EndVertical();

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Config params for each line", slabel);
        EditorGUILayout.LabelField("!!!", swarnning);
        EditorGUILayout.BeginVertical(GUI.skin.box);
        EditorGUI.indentLevel++;
        EditorGUILayout.PropertyField(lines_property, true);
        EditorGUI.indentLevel--;
        EditorGUILayout.EndVertical();

        serializedObject.ApplyModifiedProperties();
    }
}
