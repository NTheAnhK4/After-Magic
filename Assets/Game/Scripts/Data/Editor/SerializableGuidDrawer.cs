# if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(SerializableGuid))]
public class SerializableGuidPropertyDrawer : PropertyDrawer {
    private bool isShown = true;

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) 
        => isShown ? base.GetPropertyHeight(property, label) + EditorGUIUtility.singleLineHeight * 4 
        : base.GetPropertyHeight(property, label);

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
       
        EditorGUI.BeginProperty(position, label, property);
        
        isShown = EditorGUI.BeginFoldoutHeaderGroup(position, isShown, label);
    
        EditorGUI.BeginDisabledGroup(true);
      
        property.Next(true);
      
        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;
        
        if (isShown) {
            var anchorX = EditorGUIUtility.singleLineHeight;
            var anchorY = position.y + EditorGUIUtility.singleLineHeight;
            for (int offsetY = 0; offsetY < 4; offsetY++) {
                for (int offsetX = 0; offsetX < 4; offsetX++) {
                    var x = anchorX + offsetX * position.width / 4;
                    var y = anchorY + offsetY * 20;
                    var index = offsetY << 2 | offsetX;
                    EditorGUI.PropertyField(new Rect(x, y, position.width / 4, EditorGUIUtility.singleLineHeight), property.GetArrayElementAtIndex(index), GUIContent.none);
                }
            }
        }
      
        EditorGUI.indentLevel = indent;
       
        EditorGUI.EndDisabledGroup();
      
        EditorGUI.EndFoldoutHeaderGroup();
       
        EditorGUI.EndProperty();
    }
}
#endif