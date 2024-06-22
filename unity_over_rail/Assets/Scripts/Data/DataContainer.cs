using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using overail.DataTain;
using overail.DataSpawner;
using overail.DataTile;


public class DataContainer : MonoBehaviour
{
    private GameObject[] trains;
    private List<DataTrain> dataTrains = new List<DataTrain>();
    private GameObject myTrain;
    private DataTrain myDataTrain;

    private List<DataTile> dataTiles = new List<DataTile>();
    private List<DataSpawner> dataSpawners = new List<DataSpawner>();

    private GameObject[] tiles;
    private DataTile[ , ] tileMatrix;

    private void Start()
    {
        StartCoroutine(GetAllTrainInStartingGame());

        tiles = GameObject.FindGameObjectsWithTag("Tile");

        CreateTileMatrix();
        GetAllTiles();
        GetAllSpawners();
        ShowList1();
        //ShowList2();
    }




    // --------------------------------------------------------------------------------------------------------------------------------------------------
    // ------------------------------------------------------------- TRAINS -----------------------------------------------------------------------------
    // --------------------------------------------------------------------------------------------------------------------------------------------------

    private void GetAllTrains()
    {
        foreach (var train in GameObject.FindGameObjectsWithTag("Player"))
        {
            DataTrain dataTrain = new DataTrain(
                train.gameObject,
                train.GetComponent<Train>().TrainIndex,
                train.GetComponent<Train>().TrainPosition,
                train.GetComponent<Train>().ShieldIsActivate,
                train.GetComponent<Train>().CurrentItem,
                train.GetComponent<Train>().Velocity,
                train.GetComponent<Train>().CurrentHealth,
                train.GetComponentInChildren<Weapon>().CurrentBulletQuantity
            );

            dataTrains.Add(dataTrain);

            if (myTrain == null && train.GetComponent<Train>().TrainIndex == this.GetComponent<IATrain>().TrainIndex)
            {
                myTrain = train.gameObject;
                myDataTrain = dataTrain;
            }
        }
    }



    // --------------------------------------------------------------------------------------------------------------------------------------------------
    // ------------------------------------------------------------- MAP --------------------------------------------------------------------------------
    // --------------------------------------------------------------------------------------------------------------------------------------------------
    private void GetAllTiles()
    {
        foreach (var tile in tiles)
        {
            DataTile dataTile = new DataTile(
                    tile.gameObject,
                    tile.transform.position,
                    tile.GetComponent<Tile>().directionOfTile,
                    tile.GetComponent<Tile>().containsSpawner
                );

            dataTiles.Add(dataTile);
        }
    }

    private void CreateTileMatrix()
    {
        Debug.Log(tiles.Length);
        Debug.Log("number of line : " + GameObject.Find("TilemapRails").transform.childCount); //nombre de ligne
        Debug.Log("number of column : " + GameObject.Find("TilemapRails").transform.GetChild(1).transform.childCount); //nombre de colonne //on ne prend pas la première ligne car 2 cases en plus
        tileMatrix = new DataTile[GameObject.Find("TilemapRails").transform.childCount, GameObject.Find("TilemapRails").transform.GetChild(1).transform.childCount];


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








    public void ShowList()
    {
        foreach (var dataTrain in dataTrains)
        {
            Debug.Log("Train Index: " + dataTrain.index + ", Velocity: " + dataTrain.speed);
        }
        Debug.Log("-----------------------------------------");
    }
    public void ShowList1()
    {
        foreach (var dataTile in dataTiles)
        {
            //Debug.Log("Tile name: " + dataTile.Tile.name + ", Position: " + dataTile.TilePosition);
            Debug.Log("Tile name: " + dataTile.Tile.name + ", directions: " + dataTile.directionOfTile);
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









    IEnumerator GetAllTrainInStartingGame()
    {
        while (true)
        {
            GetAllTrains();
            yield return new WaitForSeconds(0.5f); //voir pour régler soucis précision position
        }
    }
}
