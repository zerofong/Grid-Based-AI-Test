using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SquareNode : NodeBase
{
    public enum DirectionType { Four, Eight}
    private DirectionType direction = DirectionType.Four;

    private static readonly List<Vector2> Dirs8 = new List<Vector2>() {
            new Vector2(0, 1), new Vector2(-1, 0), new Vector2(0, -1), new Vector2(1, 0),
            new Vector2(1, 1), new Vector2(1, -1), new Vector2(-1, -1), new Vector2(-1, 1)
        };
    private static readonly List<Vector2> Dirs4 = new List<Vector2>() {
            new Vector2(0, 1), new Vector2(-1, 0), new Vector2(0, -1), new Vector2(1, 0)
        };

    public override void CacheNeighbors()
    {
        Neighbors = new List<NodeBase>();

        if (direction == DirectionType.Eight)
        {
            foreach (var tile in Dirs8.Select(dir => GridManager.instance.GetTileAtPosition(Coords.Pos + dir)).Where(tile => tile != null))
            {
                Neighbors.Add(tile);
            }
        }
        else
        {
            foreach (var tile in Dirs4.Select(dir => GridManager.instance.GetTileAtPosition(Coords.Pos + dir)).Where(tile => tile != null))
            {
                Neighbors.Add(tile);
            }
        }
    }

    void CheckNeighborData()
    {
        string coordPos = "|| ";
        string posData = "|| ";
        foreach (var item in Neighbors)
        {
            coordPos += item.Coords.Pos + " || ";
            posData += item.transform.position + " || ";
        }
        Debug.Log("Coord: " + coordPos);
        Debug.Log("Pos: " + posData);
    }

    public override void Init(bool walkable, ICoords coords)
    {
        base.Init(walkable, coords);
    }
}

public struct SquareCoords : ICoords
{

    public float GetDistance(ICoords other)
    {
        var dist = new Vector2Int(Mathf.Abs((int)Pos.x - (int)other.Pos.x), Mathf.Abs((int)Pos.y - (int)other.Pos.y));

        var lowest = Mathf.Min(dist.x, dist.y);
        var highest = Mathf.Max(dist.x, dist.y);

        var horizontalMovesRequired = highest - lowest;

        return lowest * 14 + horizontalMovesRequired * 10;
    }

    public Vector2 Pos { get; set; }
}