#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[CustomEditor(typeof(DungeonFloorConfig))]
public class DungeonFloorConfigEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DungeonFloorConfig config = (DungeonFloorConfig)target;

        if (config.DungeonFloors == null)
        {
            config.DungeonFloors = new List<DungeonFloor>();
        }

        for (int i = 0; i < config.DungeonFloors.Count; i++)
        {
            EditorGUILayout.BeginVertical("box");

            DungeonFloor floor = config.DungeonFloors[i];
            EditorGUILayout.LabelField($"Floor {i}", EditorStyles.boldLabel);

            floor.Width = EditorGUILayout.IntField("Width", floor.Width);
            floor.Height = EditorGUILayout.IntField("Height", floor.Height);

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Build Matrix"))
            {
                floor.ResizeMatrix();
                EditorUtility.SetDirty(config);
            }

            if (GUILayout.Button("Remove Floor", GUILayout.Width(120)))
            {
                config.DungeonFloors.RemoveAt(i);
                GUI.changed = true;
                break; 
            }
            EditorGUILayout.EndHorizontal();

         
            for (int y = 0; y < floor.RoomTypes.Count; y++)
            {
                var row = floor.RoomTypes[y].Row;
                EditorGUILayout.BeginHorizontal();
                for (int x = 0; x < row.Count; x++)
                {
                    row[x] = EditorGUILayout.IntField(row[x], GUILayout.Width(40));
                }
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndVertical();
            EditorGUILayout.Space(10);
        }

       
        if (GUILayout.Button("Add New Floor"))
        {
            config.DungeonFloors.Add(new DungeonFloor());
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(config);
        }
    }
}
#endif