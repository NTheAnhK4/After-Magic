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
        if (data.RoomInfor == null) data.RoomInfor = new List<DungeonRoomSpriteInfor>();
        while (data.RoomInfor.Count < enumValues.Length)
            data.RoomInfor.Add(new DungeonRoomSpriteInfor());
        while (data.RoomInfor.Count > enumValues.Length)
            data.RoomInfor.RemoveAt(data.RoomInfor.Count - 1);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Room Sprites", EditorStyles.boldLabel);

        EditorGUI.BeginChangeCheck();

      
        for (int i = 0; i < enumValues.Length; ++i)
        {
            var roomType = (DungeonRoomType)enumValues.GetValue(i);
            var roomInfo = data.RoomInfor[i];
            roomInfo.Name = roomType.ToString(); 

            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.LabelField(roomInfo.Name, EditorStyles.boldLabel);
            roomInfo.Sprite = (Sprite)EditorGUILayout.ObjectField("Sprite", roomInfo.Sprite, typeof(Sprite), false);

           
            roomInfo.RoomEventStrategy = (RoomEventStrategy)EditorGUILayout.ObjectField(
                "Room Event Strategy",
                roomInfo.RoomEventStrategy,
                typeof(RoomEventStrategy),
                false);

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