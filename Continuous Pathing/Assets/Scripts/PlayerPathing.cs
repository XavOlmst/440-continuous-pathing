using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerPathing : MonoBehaviour
{
    [SerializeField] private Tilemap grid;
    [SerializeField] private CustomTile wallTile;
    [SerializeField] private PlayerMovement movement;
    [SerializeField] private List<Vector2> path;
    // Start is called before the first frame update
    void Start()
    {
        movement.ArrivedAtLocation += UpdateLocation;
    }

    public void SetPath(List<Vector2> newPath) => path = newPath; 

    private void UpdateLocation()
    {
        if (path == null || path.Count == 0) return;

        movement.SetMoveLocation(path[0]);
        path.RemoveAt(0);
        
        SmoothPath();
    }

    private void SmoothPath()
    {
        if (path.Count == 3)
        {
            path[1] = path[2];
            path.RemoveAt(2);
        }
        
        if(path.Count < 3) return;
        
        while (true)
        {
            if (path.Count < 3) return;
            
            var coords = Bresenham(path[0], path[2]);

            foreach (Vector3Int coord in coords)
            {
                if (grid.GetTile(coord) == wallTile)
                    return;
            }

            path[1] = path[2];
            path.RemoveAt(2);
        }
    }

    //gotten from https://github.com/anushaihalapathirana/Bresenham-line-drawing-algorithm/blob/master/src/index.js
    //by Anusha Ihalapathirana
    private List<Vector3Int> Bresenham(Vector2 startPoint, Vector2 endPoint)
    {
        List<Vector3Int> coords = new();

        var startCoords = AStarPathing.Quantize(startPoint);
        var endCoords = AStarPathing.Quantize(endPoint);

        int dx = endCoords.x - startCoords.x;
        int dy = endCoords.y - startCoords.y;

        int absDX = Mathf.Abs(dx);
        int absDY = Mathf.Abs(dy);

        int x = startCoords.x;
        int y = startCoords.y;
        
        if (absDX > absDY)
        {
            var slope = 2 * absDY - absDX;

            for (int i = 0; i < absDX; i++)
            {
                x = dx < 0 ? x - 1 : x + 1;

                if (slope < 0)
                {
                    slope += 2 * absDY;
                }
                else
                {
                    y = dy < 0 ? y - 1 : y + 1;
                    slope += 2 * (absDY - absDX);
                }

                coords.Add(new(x, y));
            }
        }
        else
        {
            int slope = 2 * absDX - absDY;

            for (int i = 0; i < absDY; i++)
            {
                y = dy < 0 ? y - 1 : y + 1;

                if (slope < 0)
                {
                    slope += 2 * absDX;
                }
                else
                {
                    x = dx < 0 ? x - 1 : x + 1;
                    slope += 2 * (absDX - absDY);
                }
                
                coords.Add(new(x,y));
            }
        }
        
        return coords;
    }
}
