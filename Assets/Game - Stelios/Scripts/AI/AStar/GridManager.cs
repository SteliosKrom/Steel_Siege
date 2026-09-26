using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    private AStarNode[,] grid;
    [SerializeField] private int gridWidth;
    [SerializeField] private int gridHeight;
    [SerializeField] private float nodeSize;

    [SerializeField] private LayerMask obstacleLayer;

    private void Start()
    {
        CreateGrid();
    }

    public void CreateGrid()
    {
        grid = new AStarNode[gridWidth, gridHeight];

        float offsetX = (gridWidth * nodeSize) / 2f;
        float offsetY = (gridHeight * nodeSize) / 2f;

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                Vector2 worldPosition = new Vector2(x * nodeSize - offsetX, y * nodeSize - offsetY);
                Collider2D hit = Physics2D.OverlapCircle(worldPosition, nodeSize * 0.5f, obstacleLayer);
                bool isWalkable = hit == null;
                grid[x, y] = new AStarNode(isWalkable, worldPosition, x, y);
            }
        }
    }

    public List<AStarNode> GetNeighbors(AStarNode currentNode)
    {
        List<AStarNode> neighbors = new List<AStarNode>();

        // UP
        int neighborX = currentNode.GridX;
        int neighborY = currentNode.GridY + 1;

        if (neighborX >= 0 && neighborX < gridWidth &&
            neighborY >= 0 && neighborY < gridHeight)
        {
            neighbors.Add(grid[neighborX, neighborY]);
        }

        // DOWN
        neighborX = currentNode.GridX;
        neighborY = currentNode.GridY - 1;

        if (neighborX >= 0 && neighborX < gridWidth &&
            neighborY >= 0 && neighborY < gridHeight)
        {
            neighbors.Add(grid[neighborX, neighborY]);
        }

        // LEFT
        neighborX = currentNode.GridX - 1;
        neighborY = currentNode.GridY;

        if (neighborX >= 0 && neighborX < gridWidth &&
            neighborY >= 0 && neighborY < gridHeight)
        {
            neighbors.Add(grid[neighborX, neighborY]);
        }

        // RIGHT
        neighborX = currentNode.GridX + 1;
        neighborY = currentNode.GridY;

        if (neighborX >= 0 && neighborX < gridWidth &&
            neighborY >= 0 && neighborY < gridHeight)
        {
            neighbors.Add(grid[neighborX, neighborY]);
        }
        return neighbors;
    }

    public AStarNode NodeFromWorldPoint(Vector2 worldPoint)
    {
        int x = Mathf.FloorToInt(worldPoint.x / nodeSize);
        int y = Mathf.FloorToInt(worldPoint.y / nodeSize);

        x = Mathf.Clamp(x, 0, gridWidth - 1);
        y = Mathf.Clamp(y, 0, gridHeight - 1);

        return grid[x, y];
    }

    public void OnDrawGizmos()
    {
        if (grid == null)
            return;

        foreach (AStarNode node in grid)
        {
            if (node.IsWalkable)
                Gizmos.color = Color.white;
            else
                Gizmos.color = Color.red;

            Gizmos.DrawWireCube(node.WorldPosition, Vector3.one * nodeSize);
        }
    }
}
