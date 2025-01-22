using UnityEngine;
using UnityEditor;
using System;
using UnityEditor.SceneManagement;

[InitializeOnLoad]
public static class HierarchyActivationDisplay 
{
    static HierarchyActivationDisplay()
    {
        EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyWindowItemOnGUI;
    }

    private static void OnHierarchyWindowItemOnGUI(int instanceID, Rect selectionRect)
    {
        GameObject gameObject = EditorUtility.InstanceIDToObject(instanceID) as GameObject;

        if (gameObject != null)
        {
            Rect toggleRect = new Rect(selectionRect);
            toggleRect.x -= 27f;
            toggleRect.width = 13f;
            bool active = EditorGUI.Toggle(toggleRect, gameObject.activeSelf);
            if(active != gameObject.activeSelf)
            {
                Undo.RecordObject(gameObject, "Changing active state of GameObject");
                gameObject.SetActive(active);
                EditorSceneManager.MarkSceneDirty(gameObject.scene);
            }
        }
    }
}
