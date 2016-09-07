﻿using UnityEngine;
using UnityEditor;
using System.Collections;

public class MyEditorWindow : EditorWindow
{
    [MenuItem("My Menu/Show My Window")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(MyEditorWindow), false, "My Window");
    }

    [MenuItem("My Menu/Add MyComponent", true)]
    public static bool ValidateAddMyComponent()
    {
        if (Selection.activeGameObject == null)
            return false;

        else
            return true;
    }

    [MenuItem("My Menu/Add MyComponent")]
    public static void AddMyComponent()
    {
        if (Selection.activeGameObject != null)
        {
            Selection.activeGameObject.AddComponent<MyComponent>();
        }
    }

    void OnGUI()
    {
        Rect rectGUI;

        GUILayout.BeginHorizontal();
        GUILayout.Button("My Button1");
        GUILayout.Button("My Button2");
        GUILayout.Button("My Button3");
        GUILayout.Button("My Button4");
        GUILayout.EndHorizontal();

        rectGUI = new Rect(100, 200, 200, 30);
        if (Selection.activeGameObject == null)
        {
            GUI.Label(rectGUI, "No Selection");
        }

        else
        {
            GUI.Label(rectGUI, Selection.activeGameObject.name);

            rectGUI = new Rect(100, 300, 100, 50);
            if (GUI.Button(rectGUI, "Add MyComponent"))
            {
                Selection.activeGameObject.AddComponent<MyComponent>();
            }
        }

        if (Event.current.button == 1)
        {
            if(Event.current.type == EventType.MouseUp)
            {
                GenericMenu contextMunu = new GenericMenu();
                contextMunu.AddItem(new GUIContent("Menu 1"), true, DoMenu1);
                contextMunu.AddItem(new GUIContent("Menu 2"), false, DoMenu2);
                contextMunu.ShowAsContext();
            }
        }
    }

    void DoMenu1()
    {
        Debug.Log("Click Menu1");
    }

    void DoMenu2()
    {
        Debug.Log("Click Menu2");
    }
}