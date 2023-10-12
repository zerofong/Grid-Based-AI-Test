using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager instance;
    void Awake() => instance = this;

    public TileGenerator tileGenerator;
    public NodeBase GetTileAtPosition(Vector2 pos) => tileGenerator.Tiles.TryGetValue(pos, out var tile) ? tile : null;
}
