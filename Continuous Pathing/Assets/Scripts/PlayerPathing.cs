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

    private void UpdateLocation()
    {
        if (path.Count == 0) return;

        movement.SetMoveLocation(path[0]);
        path.RemoveAt(0);
    }
}
