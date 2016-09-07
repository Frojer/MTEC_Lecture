using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.Collections;

[CustomEditor(typeof(MyComponent))]
public class MyComponentEditor : Editor
{
    SerializedProperty intVariable;
    SerializedProperty floatVariable;
    SerializedProperty gameObjects;

    void OnEnable()
    {
        intVariable = serializedObject.FindProperty("intVariable");
        floatVariable = serializedObject.FindProperty("floatVariable");
        gameObjects = serializedObject.FindProperty("gameObjects");
    }

    public override void OnInspectorGUI()
    {
        // Automatic management
        serializedObject.Update();

        EditorGUILayout.PropertyField(intVariable, new GUIContent("int"));
        EditorGUILayout.PropertyField(floatVariable, new GUIContent("float"));
        EditorGUILayout.PropertyField(gameObjects, new GUIContent("List"), true);

        serializedObject.ApplyModifiedProperties();

        // Manual management
        MyComponent myComponent = (MyComponent)target;
        
        int a = EditorGUILayout.IntField("Int Var", myComponent.IntVar);
        if (a != myComponent.IntVar)
        {
            myComponent.IntVar = a;
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }

        if (GUILayout.Button("Do something"))
        {
            myComponent.DoSomething();
        }
    }
}
