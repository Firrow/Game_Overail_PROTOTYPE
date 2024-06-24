using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using overail.DataTain;
using overail.DataSpawner;
using overail.DataTile;
using System;

public class DataContainer : MonoBehaviour
{
    private List<DataTrain> dataTrains = new List<DataTrain>();
    private GameObject myTrain;
    private DataTrain myDataTrain;

    private List<DataSpawner> dataSpawners = new List<DataSpawner>();

    private GameObject[] tiles;
    private GameObject[] spawners;
    private DataTile[ , ] tileMatrix;




    private void Start()
    {
        //StartCoroutine(GetAllTrainInStartingGame());
        GetAllTrains();

        tiles = GameObject.FindGameObjectsWithTag("Tile");
        CreateTileMatrix();

        spawners = GameObject.FindGameObjectsWithTag("Spawner");
        GetAllSpawners();
        ShowListSpawner();
    }

    private void Update()
    {
        ShowListSpawner();
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

    private void CreateTileMatrix()
    {
        int lineCount = GameObject.Find("TilemapRails").transform.childCount;
        int columnCount = GameObject.Find("TilemapRails").transform.GetChild(1).transform.childCount; //on ne prend pas la première ligne pour le GetChild() car 2 cases en plus
        tileMatrix = new DataTile[lineCount, columnCount];

        int element = 0;
        for (int i = 0; i < lineCount; i++)
        {
            for (int j = 0; j < columnCount; j++)
            {
                tileMatrix[i, j] = new DataTile(
                    tiles[element].gameObject,
                    tiles[element].transform.position,
                    tiles[element].GetComponent<Tile>().directionOfTile,
                    tiles[element].GetComponent<Tile>().containsSpawner,
                    new DataTile.PositionInMatrix { x = i, y = j }
                );
                element++;
            }
        }

        // Récupération des voisins de chaque tuiles
        foreach (var tile in tileMatrix)
        {
            GetNeighbors(tile);
        }
    }

    private List<DataTile> GetNeighbors(DataTile dataTile)
    {
        if (dataTile.Neighbors.Count == 0)
        {
            foreach (var direction in dataTile.directionOfTile)
            {
                switch (direction)
                {
                    case 'N':
                        dataTile.Neighbors.Add(tileMatrix[dataTile.Coordinates.x - 1, dataTile.Coordinates.y]);
                        break;
                    case 'E':
                        // Permet d'éviter que les tileStart posent des soucis avec leur 3ème direction supprimée par la suite
                        try
                        {
                            dataTile.Neighbors.Add(tileMatrix[dataTile.Coordinates.x, dataTile.Coordinates.y + 1]);
                        }
                        catch (IndexOutOfRangeException e)
                        {
                            break;
                        }
                        break;
                    case 'S':
                        dataTile.Neighbors.Add(tileMatrix[dataTile.Coordinates.x + 1, dataTile.Coordinates.y]);
                        break;
                    case 'O':
                        // Permet d'éviter que les tileStart posent des soucis avec leur 3ème direction supprimée par la suite
                        try
                        {
                            dataTile.Neighbors.Add(tileMatrix[dataTile.Coordinates.x, dataTile.Coordinates.y - 1]);
                        }
                        catch (IndexOutOfRangeException e)
                        {
                            break;
                        }
                        break;
                }
            }
        }
        return dataTile.Neighbors;
    }



    private void GetAllSpawners()
    {
        foreach (var spawner in spawners)
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








    /*IEnumerator GetAllTrainInStartingGame()
    {
        while (true)
        {
            GetAllTrains();
            yield return new WaitForSeconds(0.5f); //voir pour régler soucis précision position
            // Il faut que la position et uniquement la position soit récupérée à chaque instant
        }
    }*/











    // ------------------------------- AFFICHAGE DEBUG --------------------------------------------------------------------------------------

    public void ShowList()
    {
        foreach (var dataTrain in dataTrains)
        {
            Debug.Log("Train Index: " + dataTrain.index + ", Velocity: " + dataTrain.speed);
        }
        Debug.Log("-----------------------------------------");
    }

    public void ShowListSpawner()
    {
        foreach (var dataSpawner in dataSpawners)
        {
            Debug.Log("Spawner name: " + dataSpawner.Spawner.name + ", contains object?: " + dataSpawner.SpawnerContainsObject);
        }
        Debug.Log("-----------------------------------------");
    }
}
