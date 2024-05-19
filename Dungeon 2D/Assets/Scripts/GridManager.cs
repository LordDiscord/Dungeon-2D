using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int width;
    public int height;
    public LayerMask obstaclesLayer;

    public bool IsWalkable(Vector2 position)
    {
        Collider2D hit = Physics2D.OverlapCircle(position, 0.1f, obstaclesLayer);
        return hit == null;
    }

    public List<Vector2> GetNeighbors(Vector2 position)
    {
        List<Vector2> neighbors = new List<Vector2>();
        Vector2[] directions = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };

        foreach (Vector2 direction in directions)
        {
            Vector2 neighborPos = position + direction;
            if (IsWalkable(neighborPos))
            {
                neighbors.Add(neighborPos);
            }
        }
        return neighbors;
    }
}