using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using overail.DataSpawner;


public class DataMap : MonoBehaviour
{
    private List<Tile> tiles = new List<Tile>();
    private List<GameObject> tilesObject = new List<GameObject>();
    private List<DataSpawner> spawners = new List<DataSpawner>();


    private void Start()
    {
        /*Tilemap tilemap = GetComponent<Tilemap>();

        BoundsInt bounds = tilemap.cellBounds;
        TileBase[] allTiles = tilemap.GetTilesBlock(bounds);

        Debug.Log(tilemap);
        Debug.Log(allTiles.Length);*/

        foreach (var tile in GameObject.FindGameObjectsWithTag("Tile"))
        {
            tiles.Add(tile.GetComponent<Tile>());
            tilesObject.Add(tile);
        }
    }
}
