using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WallPlacement : MonoBehaviour
{
    [SerializeField] private Tilemap grid;
    [SerializeField] private CustomTile wallTile;
    [SerializeField] private CustomTile groundTile;
    private bool isPlacing;
    
    void Update()
    {
        if (!isPlacing) return;

        if (Input.GetMouseButtonUp(0))
        {
            Vector2 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int coords = (Vector3Int) AStarPathing.Quantize(position);
            
            grid.SetTile(coords, wallTile);
        }
    }

    public bool IsPlacing() => isPlacing;
    public void SetPlacing(bool placing) => isPlacing = placing;

    public void ResetGrid()
    {
        grid.ClearAllTiles();

        //Woo!! Hard coding!
        for (int x = -9; x <= 9; x++)
        {
            for (int y = -5; y <= 5; y++)
            {
                grid.SetTile(new Vector3Int(x, y), groundTile);
            }
        }
    }
}
