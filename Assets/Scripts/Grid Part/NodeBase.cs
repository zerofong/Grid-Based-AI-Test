using System.Collections.Generic;
using UnityEngine;

public abstract class NodeBase : MonoBehaviour
{
    public abstract void CacheNeighbors();

    public virtual void Init(bool walkable, ICoords coords)
    {
        UpdateWalkable();

        Coords = coords;
    }


    #region Coordinates
    public ICoords Coords;
    // Helper to reduce noise in pathfinding
    public float GetDistance(NodeBase other) => Coords.GetDistance(other.Coords);
    public bool Walkable;
    public void UpdateWalkable()
    {
        Walkable = CanMoveHere();
    }
    #endregion

    #region Connection
    public List<NodeBase> Neighbors { get; protected set; }
    public NodeBase Connection { get; private set; }
    public void SetConnection(NodeBase nodeBase) => Connection = nodeBase;

    #endregion

    #region Costs
    public float G { get; private set; }
    public float H { get; private set; }
    public float F => G + H;

    public void SetG(float g) => G = g;
    public void SetH(float h) => H = h;

    #endregion

    #region Information
    public enum TileType { None, Obstacle, Player, Enemy }
    public TileType type;

    #region Sample Detail Function
    public bool CanMoveHere() => type == TileType.None;
    
    #endregion

    public string TileDetails()
    {
        string details = $"Type: {type}\n" +
            $"Can Move: {CanMoveHere()}\n" +
            $"Pos: {transform.position}\n" +
            $"Coord: {Coords.Pos}";

        return details;
    }
    #endregion
}

public interface ICoords
{
    public float GetDistance(ICoords other);
    public Vector2 Pos { get; set; }
}