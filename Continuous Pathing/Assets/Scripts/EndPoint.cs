using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPoint : MonoBehaviour
{
    [SerializeField] private AStarPathing pathing;
    void Update()
    {
        transform.position = pathing.GetEndPoint();
    }
}
