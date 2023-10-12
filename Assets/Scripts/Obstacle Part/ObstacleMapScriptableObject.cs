using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="ObstacleMap")]
public class ObstacleMapScriptableObject : ScriptableObject
{
    public Row[] obstacleValues;
    [System.Serializable]
    public class Row
    {
        public Column column;
    }

    [System.Serializable]
    public class Column
    {
        public bool[] value;
    }


    public void UpdateViewableValue(bool[,] newValue)
    {
        for (int i = 0; i < newValue.GetLength(1); i++)
        {
            for (int j = 0; j < newValue.GetLength(0); j++)
            {
                obstacleValues[i].column.value[j] = newValue[j, i];
            }
        }
    }
    public void DisplayValue()
    {
        string data = "";
        for (int i = 0; i < obstacleValues.Length; i++)
        {
            data += "||";
            for (int j = 0; j < obstacleValues[i].column.value.Length; j++)
            {
                data += obstacleValues[i].column.value[j].ToString() + "||";
            }
            Debug.Log(data);
            data = "";
        }
    }
}