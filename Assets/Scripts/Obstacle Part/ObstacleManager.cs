using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    #region Singleton
    public static ObstacleManager instance;
    private void Awake()
    {
        instance = this;
    }
    #endregion

    public TileGenerator tileGenerator;
    public ObstacleMapScriptableObject obstacleMapScriptableObject;
    public GameObject obstaclePrefab;
    public Vector3 obstacleOffset;

    public void InitObstacles()
    {
        //obstacleMapScriptableObject.DisplayValue();

        int row = obstacleMapScriptableObject.obstacleValues.Length;

        for (int j = 0; j < row; j++)
        {
            for (int i = 0; i < obstacleMapScriptableObject.obstacleValues[j].column.value.Length; i++)
            {
                if (obstacleMapScriptableObject.obstacleValues[i].column.value[j] == true)
                {
                    Instantiate(obstaclePrefab, new Vector3(i * tileGenerator.tileSize, 0, j * tileGenerator.tileSize) + obstacleOffset,
                        Quaternion.identity, transform);

                    tileGenerator.SetTileType(row * i + j, NodeBase.TileType.Obstacle);
                }
            }
        }
    }
}
