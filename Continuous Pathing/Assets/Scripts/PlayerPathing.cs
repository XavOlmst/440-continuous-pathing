using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPathing : MonoBehaviour
{
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
    }

    private void SmoothPath()
    {
        
    }

    private void Bresenham(Vector2 startPoint, Vector2 endPoint)
    {
        
    }
}
