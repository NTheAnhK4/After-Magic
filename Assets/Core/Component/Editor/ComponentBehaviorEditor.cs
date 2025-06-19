using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(ComponentBehavior), true)]
public class ComponentBehaviorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        ComponentBehavior cb = (ComponentBehavior)target;

        if (GUILayout.Button("LoadComponent"))
        {
            cb.LoadComponent();
        }
    }
}
