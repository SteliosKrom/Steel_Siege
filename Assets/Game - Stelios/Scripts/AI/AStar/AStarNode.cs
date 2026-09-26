using UnityEngine;
using System.Collections.Generic;

public class AStarNode
{
    public Vector2 WorldPosition { get; set; }
    public bool IsWalkable { get; set; }
    public int GridX { get; set; }
    public int GridY { get; set; }
    public List<AStarNode> Neighbors { get; set; }
    public int GCost { get; set; }
    public int HCost { get; set; }
    public int FCost => GCost + HCost;
    public AStarNode Parent { get; set; }

    public AStarNode(bool isWalkable, Vector2 worldPosition, int gridX, int gridY)
    {
        this.IsWalkable = isWalkable;
        this.WorldPosition = worldPosition;
        this.GridX = gridX;
        this.GridY = gridY;
        this.GCost = int.MaxValue;
    }
}
