using UnityEngine;
using UnityEditor;

public class ObstacleManagerEditor : EditorWindow
{
    bool[,] fieldsArray = new bool[10, 10];
    int width = 10;
    int height = 10;

    ObstacleMapScriptableObject obstacleMapScriptableObject;

    [MenuItem("Window/Obstacle Map Editor")]
    public static void ShowWindow()
    {
        GetWindow(typeof(ObstacleManagerEditor));
    }

    private void Awake()
    {
        obstacleMapScriptableObject = Resources.Load<ObstacleMapScriptableObject>("Scriptable Objects/ObstacleMap");

        if (width != fieldsArray.GetLength(0) || height != fieldsArray.GetLength(1))
        {
            fieldsArray = new bool[(int)TileGenerator.gridSize.x, (int)TileGenerator.gridSize.y];
        }

        LoadFieldArrayData();
    }

    void OnGUI()
    {
        #region Flexible Height/Width Section
        //GUILayout.Label("LEVEL NUMBER", EditorStyles.boldLabel);
        //levelNumber = EditorGUILayout.IntField("Level Number", levelNumber);

        //GUILayout.Label("Array width/height", EditorStyles.boldLabel);
        //width = EditorGUILayout.IntField("Width", width);
        //height = EditorGUILayout.IntField("Width", height);
        #endregion


        ChangeArrayWidthAndHeight();

        if (GUILayout.Button("Update Obstacle Map"))
        {
            obstacleMapScriptableObject.UpdateViewableValue(fieldsArray);

            EditorUtility.SetDirty(obstacleMapScriptableObject);
            Close();
        }
    }

    /// <summary>
    /// Draw toggle array based on height and width
    /// </summary>
    void ChangeArrayWidthAndHeight()
    {
        for (int j = 0; j < height; j++)
        {
            EditorGUILayout.BeginHorizontal();
            for (int i = 0; i < width; i++)
            {
                fieldsArray[i, j] = EditorGUILayout.Toggle(fieldsArray[i, j]);
            }
            EditorGUILayout.EndHorizontal();
        }
    }

    /// <summary>
    /// Assign value to array based on scriptable object
    /// </summary>
    void LoadFieldArrayData()
    {
        for (int j = 0; j < height; j++)
        {
            for (int i = 0; i < width; i++)
            {
                fieldsArray[i, j] = obstacleMapScriptableObject.obstacleValues[j].column.value[i];
            }
        }
    }
}