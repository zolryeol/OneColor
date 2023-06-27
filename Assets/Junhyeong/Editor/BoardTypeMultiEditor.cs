using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(CTile))]
public class BoardTypeMultiEditor : Editor
{
    SerializedProperty colorTypeProperty;
    SerializedProperty isBlackTileProperty;
    SerializedProperty isMoveTileProperty;
    SerializedProperty isIllGeneratorProperty;

    void OnEnable()
    {
        colorTypeProperty = serializedObject.FindProperty("colorType");
//         isBlackTileProperty = serializedObject.FindProperty("isBlackTile");
//         isMoveTileProperty = serializedObject.FindProperty("isMoveTile");
//         isIllGeneratorProperty = serializedObject.FindProperty("isIllGeneratorTile");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(colorTypeProperty);
//         EditorGUILayout.PropertyField(isBlackTileProperty);
//         EditorGUILayout.PropertyField(isMoveTileProperty);
//         EditorGUILayout.PropertyField(isIllGeneratorProperty);

        GameObject[] selectedObjects = Selection.gameObjects;

        if (GUILayout.Button("Apply to selected objects"))
        {
            ApplyToSelectedObjects((target as CTile).colorType /*, (target as CTile).isBlackTile,
                (target as CTile).isMoveTile, (target as CTile).isIllGeneratorTile */);
        }

        serializedObject.ApplyModifiedProperties();
    }

    void ApplyToSelectedObjects(ETileType colorType/*, bool isBlackTile, bool isMoveTile, bool isIllGeneratorTile*/)
    {
        var selectedObjects = Selection.gameObjects;

        foreach (var obj in selectedObjects)
        {
            var tile = obj.GetComponent<CTile>();
            if (tile != null)
            {
                tile.colorType = colorType;
                //                 tile.isBlackTile = isBlackTile;
                //                 tile.isMoveTile = isMoveTile;
                //                 tile.isIllGeneratorTile = isIllGeneratorTile;
            }
        }
    }
}
