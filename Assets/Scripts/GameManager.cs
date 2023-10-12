using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private void Awake()
    {
        instance = this;
    }

    [Header("Player")]
    GameObject player;
    public GameObject playerPrefab;
    public Vector3 playerSpawnOffset;
    
    [Header("Enemy")]
    GameObject enemy;
    public GameObject enemyPrefab;
    public Vector3 enemySpawnOffset;

    [Header("References")]
    public TileGenerator tileGenerator;

    private void Start()
    {
        #region Init Turn Text
        UpdateTurnText();
        turnTextGO.SetActive(true);
        #endregion

        #region Spawn Tiles and Obstacles
        tileGenerator.InitTiles();
        ObstacleManager.instance.InitObstacles();
        tileGenerator.CacheAllNeighborTiles();
        #endregion
        
        #region Spawn Players
        var rand = tileGenerator.RandomEmptyTile();
        player = Instantiate(playerPrefab, tileGenerator.GetSelectedTilePosition(rand) + playerSpawnOffset, Quaternion.identity);
        tileGenerator.SetTileType(rand, NodeBase.TileType.Player);
        #endregion

        #region Spawn Enemy
        rand = tileGenerator.RandomEmptyTile();
        enemy = Instantiate(enemyPrefab, tileGenerator.GetSelectedTilePosition(rand) + enemySpawnOffset, Quaternion.identity);
        tileGenerator.SetTileType(rand, NodeBase.TileType.Enemy);
        #endregion

        #region Start With Player Turn
        ChangeState(GameState.PLAYER);
        UpdateTurnText();
        #endregion
    }

    #region Turn Management
    [Header("Turn Manager")]
    public TextMeshProUGUI turnText;
    public GameObject turnTextGO;

    public enum GameState { INIT, PLAYER, ENEMY}
    public static GameState gameState = GameState.INIT;

    public static void ChangeState(GameState newState)
    {
        gameState = newState;
        switch (gameState)
        {
            case GameState.PLAYER:
                //Debug.Log("<color=blue>Turn: Player</color>");
                break;
            case GameState.ENEMY:
                //Debug.Log("<color=red>Turn: Enemy</color>");
                break;
            default:
                break;
        }
    }

    public void UpdateTurnText(string newText)
    {
        turnText.text = $"TURN: {gameState}\n"+newText;
    }
    public void UpdateTurnText()
    {
        turnText.text = $"TURN: {gameState}";
    }
    public void UpdateTurnTextCustom(string newText)
    {
        turnText.text = newText;
    }
    #endregion
}
