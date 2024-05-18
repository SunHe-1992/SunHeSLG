using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum TestEnum
{
    FirstEnum = 0,
    SecondEnum = 1,
}
public class Map2DEditorWIndow : EditorWindow
{
    TestEnum tenum;
    static GameObject groundPrefab;
    void OnGUI()
    {
        GUILayout.Label("test label");
        EditorGUILayout.Space(10f);

        tenum = (TestEnum)EditorGUILayout.EnumPopup("select the enum:", tenum);
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("--------------");

        if (GUILayout.Button("Test button"))
        {
            OnTestButton();
        }

        EditorGUILayout.EndHorizontal();

    }

    #region window entrance menu
    [MenuItem("SUNHE/MapEditor2D")]
    public static void MenuOpen()
    {
        DoShow();
    }
    static Map2DEditorWIndow windowInst;
    public static void DoShow()
    {
        windowInst = (Map2DEditorWIndow)EditorWindow.GetWindow(typeof(Map2DEditorWIndow));
        windowInst.Show();

    }

    #endregion

    void OnSceneGUI()
    {
        Event e = Event.current;
        Vector3 mousePosition = Event.current.mousePosition;

        if (e.type == EventType.MouseDown)
        {
            // Left mouse click detected
            //if (e.button == 0)
            {
                // Get the mouse position in world space (optional)
                mousePosition = Camera.current.ScreenToWorldPoint(e.mousePosition);
                mousePosition.y = -mousePosition.y; // Invert Y for Scene View

                // Handle left mouse click event here (e.g., place an object, modify scene elements)
                Debug.Log("Left mouse click detected at world position: " + mousePosition);
            }
            // Right mouse click or other buttons can be handled similarly with checks for e.button value
        }

        //on mouse move/click
        Event currentEvent = Event.current;
        //&& currentEvent.button == 0
        if (currentEvent.type == EventType.MouseDown)
        {
            // Left mouse button clicked within the window
            mousePosition = currentEvent.mousePosition;
            // Do something with the mouse click position (e.g., handle logic)
            Debug.Log("Left mouse button clicked at: " + mousePosition);
        }
    }

    void TestSelect()
    {
        //找到场景中的组件
        TilePresetData[] resultArray = FindObjectsOfType<TilePresetData>();
        //设置为选中
        Selection.objects = resultArray;
    }
    void OnTestButton()
    {
        Debug.Log("test button clicked");

        string prefabPath = "Assets/GameRes/MapTiles2D/Prefabs/PrefabGround.prefab";
        // Load the contents of the Prefab Asset.
        groundPrefab = PrefabUtility.LoadPrefabContents(prefabPath);
        if (groundPrefab != null)
        {
            Instantiate(groundPrefab);
        }
    }
}
