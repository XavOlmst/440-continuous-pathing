using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10;
    [SerializeField] private Vector2 moveLocation;
    public Action ArrivedAtLocation;
    private bool isMoving = true;
    private bool canMove = false;

    private void Start()
    {
        moveLocation = transform.position;
    }

    private void Update()
    {
        if (canMove && Input.GetMouseButtonUp(0))
        {
            StartCoroutine(Co_MovePlayer(Camera.main.ScreenToWorldPoint(Input.mousePosition)));
        }
    }

    private IEnumerator Co_MovePlayer(Vector2 position)
    {
        yield return new WaitForEndOfFrame();
        if (canMove)
            transform.position = position;
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

    public bool CanMove() => canMove;
    public void SetCanMove(bool canMove) => this.canMove = canMove;
    
    public void SetMoveLocation(Vector2 worldSpaceLocation)
    {
        isMoving = true;
        moveLocation = worldSpaceLocation;
    }
}
