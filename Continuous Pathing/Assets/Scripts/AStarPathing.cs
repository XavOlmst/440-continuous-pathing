using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarPathing : MonoBehaviour
{
    [SerializeField] private PlayerPathing playerPath;
    [SerializeField] private Vector2 endLocation;
    private Transform playerTransform;

    private void Start()
    {
        playerTransform = playerPath.transform;
    }

    public Vector2Int Quantize(Vector2 location)
    {
        return new Vector2Int((int) location.x, (int) location.y);
    }
}
