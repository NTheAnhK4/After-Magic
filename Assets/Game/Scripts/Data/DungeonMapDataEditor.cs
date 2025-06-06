#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System;
using System.Collections.Generic;

[CustomEditor(typeof(DungeonMapData))]
public class DungeonMapDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
       
        DungeonMapData data = (DungeonMapData)target;

    
        EditorGUILayout.PropertyField(serializedObject.FindProperty("RoomPrefab"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("VerticalPath"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("HorizontalPath"));

       
        var enumValues = Enum.GetValues(typeof(DungeonRoomType));
        if (data.RoomSprites == null) data.RoomSprites = new List<DungeonRoomSpriteInfor>();
        while (data.RoomSprites.Count < enumValues.Length)
            data.RoomSprites.Add(new DungeonRoomSpriteInfor());
        while (data.RoomSprites.Count > enumValues.Length)
            data.RoomSprites.RemoveAt(data.RoomSprites.Count - 1);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Room Sprites", EditorStyles.boldLabel);

        EditorGUI.BeginChangeCheck();

      
        for (int i = 0; i < enumValues.Length; ++i)
        {
            var roomType = (DungeonRoomType)enumValues.GetValue(i);
            var roomInfo = data.RoomSprites[i];
            roomInfo.Name = roomType.ToString(); 

            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.LabelField(roomInfo.Name, EditorStyles.boldLabel);
            roomInfo.Sprite = (Sprite)EditorGUILayout.ObjectField("Sprite", roomInfo.Sprite, typeof(Sprite), false);
            EditorGUILayout.EndVertical();
        }

        if (EditorGUI.EndChangeCheck())
        {
            EditorUtility.SetDirty(data);
        }

        serializedObject.ApplyModifiedProperties();
    }
}
#endif