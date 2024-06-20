using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using overail.DataSpawner;
using overail.DataTile;


public class DataMap : MonoBehaviour
{
    private List<GameObject> tilesObject = new List<GameObject>();
    private List<DataTile> dataTiles = new List<DataTile>();
    private List<DataSpawner> dataSpawners = new List<DataSpawner>();

    private GameObject[] tiles;


    private void Start()
    {
        tiles = GameObject.FindGameObjectsWithTag("Tile");
        GetAllTiles();
        //GetAllSpawners();
        ShowList1();
        //ShowList2();
    }

    // CHECK IT --> DONT WORK
    private void GetAllTiles()
    {
        Debug.Log(tiles.Length);
        foreach (var tile in tiles)
        {
            DataTile dataTile = new DataTile(
                    tile.gameObject,
                    tile.transform.position,
                    tile.GetComponent<DataTile>().directionOfTile,
                    tile.GetComponent<DataTile>().containsSpawner
                );

            dataTiles.Add(dataTile);
        }
    }

    // CHECK IT !
    private void GetAllSpawners()
    {
        foreach (var spawner in GameObject.FindGameObjectsWithTag("Spawner"))
        {
            DataSpawner dataSpawner = new DataSpawner(
                    spawner.gameObject,
                    spawner.transform.parent.gameObject,
                    spawner.transform.position,
                    spawner.GetComponent<SpawnObjects>().ContainsObject
                );

            dataSpawners.Add(dataSpawner);
        }
    }

    public void ShowList1()
    {
        foreach (var dataTile in dataTiles)
        {
            Debug.Log("Tile name: " + dataTile.Tile.name + ", Position: " + dataTile.TilePosition);
        }
        Debug.Log("-----------------------------------------");
    }

    public void ShowList2()
    {
        foreach (var dataSpawner in dataSpawners)
        {
            Debug.Log("Spawner name: " + dataSpawner.Spawner.name + ", contains object?: " + dataSpawner.SpawnerContainsObject);
        }
        Debug.Log("-----------------------------------------");
    }
}

