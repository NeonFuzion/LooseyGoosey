using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SceneLoader))]
public class SceneLoaderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        SceneLoader staticValues = (SceneLoader)target;

        GUILayout.Space(10);
        if (GUILayout.Button("Update Scene Data"))
        {
            
        }

        GUILayout.Space(10);
        if (GUILayout.Button("Reset Save"))
        {
            
        }
    }
}