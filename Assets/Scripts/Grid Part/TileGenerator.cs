using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGenerator : MonoBehaviour
{
    #region Init
    public static Vector2 gridSize = new Vector2(10, 10);
    public GameObject tilePrefab;
    public float tileSize = 1f;
    List<NodeBase> tiles = new List<NodeBase>();
    public Dictionary<Vector2, NodeBase> Tiles = new Dictionary<Vector2, NodeBase>();

    public void InitTiles()
    {
        for (int i = 0; i < gridSize.y; i++)
        {
            for (int j = 0; j < gridSize.x; j++)
            {
                var go = Instantiate(tilePrefab, new Vector3(i * tileSize, 0, j * tileSize), Quaternion.identity, transform);
                go.name += " " + new Vector2(i * tileSize, j * tileSize);
                var node = go.GetComponent<NodeBase>();

                node.Init(true, new SquareCoords { Pos = new Vector2(i * tileSize, j * tileSize) });
                tiles.Add(node);
                Tiles.Add(node.Coords.Pos, node);
            }
        }
    }

    public void CacheAllNeighborTiles()
    {
        foreach (var item in tiles)
        {
            item.CacheNeighbors();

            // Update Tiles dictionary to have obstacles type
            if (item.type == NodeBase.TileType.Obstacle)
            {
                GetTileFromTiles(item.Coords.Pos).type = NodeBase.TileType.Obstacle;
            }
        }

        foreach (var item in tiles)
        {
            GetTileFromTiles(item.Coords.Pos).CacheNeighbors();
        }
    }
    #endregion


    #region Helpers
    public void SetTileType(int index, NodeBase.TileType type)
    {
        tiles[index].type = type;
        tiles[index].UpdateWalkable();
    }

    // Use this if have 1 player only
    public NodeBase GetPlayerTile()
    {
        NodeBase playerTile = null;
        foreach (var tile in tiles)
        {
            if (tile.type == NodeBase.TileType.Player) playerTile = tile;
        }
        return playerTile;
    }
    // Use this if have 1 enemy only
    public NodeBase GetEnemyTile()
    {
        NodeBase enemyTile = null;
        foreach (var tile in tiles)
        {
            if (tile.type == NodeBase.TileType.Enemy) enemyTile = tile;
        }
        return enemyTile;
    }

    public void UpdateTile(NodeBase.TileType type, int index)
    {
        foreach (var tile in tiles)
        {
            if (tile.type == type)
            {
                tile.type = NodeBase.TileType.None;
                GetTileFromTiles(tile.Coords.Pos).type = NodeBase.TileType.None;
            }
        }
        tiles[index].type = type;
        GetTileFromTiles(tiles[index].Coords.Pos).type = type;
        //CacheAllNeighborTiles();
    }
    public void UpdateTile(NodeBase.TileType type, NodeBase node)
    {
        foreach (var tile in tiles)
        {
            if (tile.type == type)
            {
                UpdateNodeWalkable(tile, NodeBase.TileType.None);
            }
        }
        UpdateNodeWalkable(node, type);
        node.type = type;
        node.Walkable = type == NodeBase.TileType.None;
        GetTileFromTiles(node.Coords.Pos).type = type;
        GetTileFromTiles(node.Coords.Pos).Walkable = type == NodeBase.TileType.None;
        //CacheAllNeighborTiles();
    }
    void UpdateNodeWalkable(NodeBase tile, NodeBase.TileType type)
    {
        tile.type = type;
        tile.Walkable = type == NodeBase.TileType.None;
        GetTileFromTiles(tile.Coords.Pos).type = type;
        GetTileFromTiles(tile.Coords.Pos).Walkable = type == NodeBase.TileType.None;
    }
    
    NodeBase GetTileFromTiles(Vector2 Pos)
    {
        Tiles.TryGetValue(Pos, out var value);
        return value;
    }
    public bool TileAvailable(int index) => tiles[index].type == NodeBase.TileType.None;
    public Vector3 GetSelectedTilePosition(int index) => tiles[index].transform.position;
    public int RandomEmptyTile()
    {
        int rand;
        do
        {
            rand = Random.Range(0, 100);
        } while (!TileAvailable(rand));

        return rand;
    }
    #endregion

}
