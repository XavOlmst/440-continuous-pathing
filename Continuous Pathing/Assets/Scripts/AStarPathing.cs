using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Tilemaps;

class AStarNode : IComparable
{
    public AStarNode parentNode;
    public Vector2Int coords;
    int startDistance = 0;
    public int nodeCost = 0;

    public AStarNode(AStarNode parent, int distance, Vector2Int coords)
    {
        parentNode = parent;

        startDistance = parent != null ? parent.startDistance + 1 : 0;

        nodeCost = startDistance + distance;
        this.coords = coords;
    }

    

    public static bool operator <(AStarNode n1, AStarNode n2) => n1.nodeCost < n2.nodeCost;
    public static bool operator >(AStarNode n1, AStarNode n2) => n1.nodeCost > n2.nodeCost;

    public static bool operator <=(AStarNode n1, AStarNode n2) => n1.nodeCost <= n2.nodeCost;
    public static bool operator >=(AStarNode n1, AStarNode n2) => n1.nodeCost >= n2.nodeCost;

    public int CompareTo(object obj)
    {
        if (obj == null) return 1;

        AStarNode otherNode = obj as AStarNode;

        if(otherNode != null)
        {
            return nodeCost.CompareTo(otherNode.nodeCost);
        }
        else
        {
            throw new ArgumentException("Object is not an A Star Node");
        }
    }
}

public class AStarPathing : MonoBehaviour
{
    [SerializeField] private PlayerPathing playerPath;
    [SerializeField] private Tilemap grid; 
    [SerializeField] private Vector2 endLocation;
    private bool canSetEndPoint;
    private Transform playerTransform;

    private Vector2Int[] directions = { Vector2Int.left, Vector2Int.down, Vector2Int.right, Vector2Int.up };
    private void Start()
    {
        playerTransform = playerPath.transform;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
            playerPath.SetPath(GetPath());
        }

        if (canSetEndPoint && Input.GetMouseButtonUp(0))
        {
            endLocation =  Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }

    public void SetPath() => playerPath.SetPath(GetPath());
    
    public Vector2 GetEndPoint() => endLocation;
    public bool CanSetEndPoint() => canSetEndPoint;
    public void SetEndPointSetting(bool canSet) => canSetEndPoint = canSet;
    
    public List<Vector2> GetPath()
    {
        List<Vector2> path = new();
        List<AStarNode> frontier = new();
        List<AStarNode> explored = new();

        Vector2Int playerCoords = Quantize(playerTransform.position);
        Vector2Int endCoords = Quantize(endLocation);

        AStarNode startNode = new(null, (endCoords - playerCoords).sqrMagnitude, playerCoords);

        frontier.Add(startNode);

        while(frontier.Count > 0)
        {
            if (frontier.Count > 1000)
            {
                Debug.Log("No path could be found :'(");
                return path;
            }
            
            explored.Add(frontier[0]);

            foreach(var dir in directions)
            {
                Vector2Int coords = frontier[0].coords + dir;
                CustomTile tile = grid.GetTile((Vector3Int)coords) as CustomTile;
                
                if (tile && tile.isWall) continue; 
                
                AStarNode node = new(frontier[0], (endCoords - coords).sqrMagnitude, coords);
                
                if(node.coords == endCoords)
                {
                    path.Clear();
                    AStarNode currentNode = node.parentNode;
                    path.Add(endLocation);
                    //back track and add path
                    while(currentNode != startNode)
                    {
                        path.Add(currentNode.coords);
                        currentNode = currentNode.parentNode;
                    }

                    path.Reverse();

                    return path;
                }

                if(explored.All(x => x.coords != node.coords))
                {
                    frontier.Add(node);
                }
            }

            frontier.RemoveAt(0);
            frontier.Sort();
        }

        return path;
    }

    public static Vector2Int Quantize(Vector2 location)
    {
        return new Vector2Int(Mathf.RoundToInt(location.x), Mathf.RoundToInt(location.y));
    }

    //public Vector2 GetCenterPoint(Vector2Int coords) => new Vector2(coords.x + 0.5f, coords.y + 0.5f);
}
