using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Script of tile
/// </summary>

public class Tile : MonoBehaviour
{
    public Vector3 POSITION; //TEMP
    public string directionOfTile;
    public bool containsSpawner;
    public bool isSwitch;

    private void Start()
    {
        GridLayout grid = GameObject.FindObjectOfType<GridLayout>();
        POSITION = grid.CellToWorld(grid.WorldToCell(this.transform.position));
    }
}
