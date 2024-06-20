using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataTile : MonoBehaviour
{
    public string directionOfTile;
    public bool containsSpawner;

    private Vector3 tilePosition;
    private GameObject tile;


    public DataTile(GameObject tile, Vector3 tilePosition, string directionOfTile, bool containsSpawner)
    {
        this.tile = tile;
        this.tilePosition = tilePosition;
        this.directionOfTile = directionOfTile;
        this.containsSpawner = containsSpawner;
    }

    private void Awake()
    {
        tilePosition = this.transform.position;
    }



    public Vector3 TilePosition
    {
        get { return tilePosition; }
        set { tilePosition = value; }
    }

    public GameObject Tile
    {
        get { return tile; }
        set { tile = value; }
    }

}
