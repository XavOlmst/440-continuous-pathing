using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10;
    [SerializeField] private Vector2 moveLocation;
    public Action ArrivedAtLocation;
    private bool isMoving = true;

    private void Start()
    {
        moveLocation = transform.position;
    }

    private void FixedUpdate()
    {
        if (isMoving && Vector2.Distance(transform.position, moveLocation) > 0.01f)
        {
            transform.position = Vector2.MoveTowards(transform.position, moveLocation, moveSpeed * Time.deltaTime);
        }
        else
        {
            isMoving = false;
            ArrivedAtLocation?.Invoke();
        }
    }

    public void SetMoveLocation(Vector2 worldSpaceLocation)
    {
        isMoving = true;
        moveLocation = worldSpaceLocation;
    }
}
