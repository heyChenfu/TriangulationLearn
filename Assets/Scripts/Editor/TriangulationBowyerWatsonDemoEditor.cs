using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;
using static Unity.VisualScripting.Icons;

[CustomEditor(typeof(TriangulationBowyerWatsonDemo), true)]
public class TriangulationBowyerWatsonDemoEditor : Editor
{

    SerializedProperty m_randomCount;
    SerializedProperty m_pointList;
    GUIContent m_randomCountContent;
    GUIContent m_pointListContent;

    private void OnEnable()
    {
        //m_randomCount = serializedObject.FindProperty("RandomPointCount");
        //m_pointList = serializedObject.FindProperty("PointList");
        //m_randomCountContent = new GUIContent("RandomPointCount");
        //m_pointListContent = new GUIContent("PointList");

    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        //EditorGUILayout.PropertyField(m_randomCount, m_randomCountContent);
        //EditorGUILayout.PropertyField(m_pointList, m_pointListContent);
        if (GUILayout.Button("Rebuild"))
        {
            var script = target as TriangulationBowyerWatsonDemo;
            script?.Build();
        }

    }

}
