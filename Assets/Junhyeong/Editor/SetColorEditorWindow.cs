using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class SetColorEditorWindow : EditorWindow
{
    private bool showButton = false;
    private bool showDropdownMenu = false;
    [MenuItem("Editor/TileColorEditor")]
    private static void OpenWindow()
    {
        GetWindow<SetColorEditorWindow>("ColorSetter");
    }

    private void OnGUI()
    {
        GUILayout.Label("EditorList", EditorStyles.boldLabel);

        GUILayout.Space(10f);

        EditorGUI.BeginChangeCheck();

        if (GUILayout.Button(showButton ? "Close Editor In SceneView" : "Open Editor In SceneView"))
        {
            showButton = true;
        }

        if (EditorGUI.EndChangeCheck())
        {
            SceneView.RepaintAll();
        }

        GUILayout.Space(10f);
    }

    private void OnEnable()
    {
        SceneView.duringSceneGui += OnSceneGUI;
    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
    }

    private void OnSceneGUI(SceneView sceneView)
    {
        Event e = Event.current;

        if (showButton)
        {
            Handles.BeginGUI();

            var buttonSize = new Vector2(100, 30);
            var buttonRect = GUILayoutUtility.GetRect(buttonSize.x, buttonSize.y);
            buttonRect.x = 10;
            buttonRect.y = 10;
            buttonRect.width = 100f;

            if (GUI.Button(buttonRect, "ColorEditor") && e.button == 0 && e.type == EventType.MouseDown)
            {
                showDropdownMenu = true;
                Debug.Log("버튼 호출");
            }
            // 
            //             if (showDropdownMenu)
            //             {
            //                 var dropdownMenuRect = new Rect(10f, 50f, 120f, 220f);
            //                 GUI.Box(dropdownMenuRect, "");
            // 
            //                 var toggleSize = new Vector2(100f, 20f);
            //                 for (int i = 0; i < 8; i++)
            //                 {
            //                     var toggleRect = new Rect(15f, 60f + (i * 25f), toggleSize.x, toggleSize.y);
            //                     GUI.Toggle(toggleRect, false, "Toggle " + i.ToString());
            //                 }
            // 
            //                 if (Event.current.type == EventType.MouseDown && !dropdownMenuRect.Contains(Event.current.mousePosition))
            //                 {
            //                     showDropdownMenu = false;
            //                 }
            //             }

            Handles.EndGUI();
        }
    }
}