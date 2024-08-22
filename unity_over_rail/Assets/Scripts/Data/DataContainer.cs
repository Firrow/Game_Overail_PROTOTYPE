using overail.DataSpawner;
using overail.DataTain;
using overail.DataTile;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class DataContainer : MonoBehaviour
{
    private List<DataTrain> dataTrains = new List<DataTrain>();
    private GameObject myTrain;
    private DataTrain myDataTrain;

    private GameObject[] spawners;
    private List<DataSpawner> dataSpawners = new List<DataSpawner>();

    private GameObject[] tiles;
    private DataTile[,] tileMatrix;




    private void Start()
    {
        CreateTileMatrix();

        spawners = GameObject.FindGameObjectsWithTag("Spawner");
        GetAllSpawners();

        GetAllTrains();

        /*foreach (var dt in dataSpawners)
        {
            Debug.Log(dt.SpawnerPosition); 
        }*/
    }

    private void Update()
    {
        /*foreach (var dt in dataSpawners)
        {
            //Debug.Log(dt.ObjectOnSpawner.name);
        }*/
    }



    // --------------------------------------------------------------------------------------------------------------------------------------------------
    // ------------------------------------------------------------- TRAINS -----------------------------------------------------------------------------
    // --------------------------------------------------------------------------------------------------------------------------------------------------

    public void GetAllTrains()
    {
        foreach (var train in GameObject.FindGameObjectsWithTag("Player"))
        {
            DataTrain dataTrain = new DataTrain(
                train.gameObject,
                train.GetComponent<Train>().TrainIndex
            );

            dataTrains.Add(dataTrain);

            if (myTrain == null && train.GetComponent<Train>().TrainIndex == this.GetComponent<IATrain>().PlayerIndex)
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
        tiles = GameObject.FindGameObjectsWithTag("Tile");
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
                // Permet d'éviter que les tileStart posent des soucis avec leur 3ème direction supprimée par la suite
                try
                {
                    switch (direction)
                    {
                        case 'N':
                            dataTile.Neighbors.Add(tileMatrix[dataTile.Coordinates.x - 1, dataTile.Coordinates.y]);
                            break;
                        case 'E':
                            dataTile.Neighbors.Add(tileMatrix[dataTile.Coordinates.x, dataTile.Coordinates.y + 1]);
                            break;
                        case 'S':
                            dataTile.Neighbors.Add(tileMatrix[dataTile.Coordinates.x + 1, dataTile.Coordinates.y]);
                            break;
                        case 'O':
                            dataTile.Neighbors.Add(tileMatrix[dataTile.Coordinates.x, dataTile.Coordinates.y - 1]);
                            break;
                    }
                }
                catch (IndexOutOfRangeException e)
                {
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
                    spawner.transform.position
                );

            dataSpawners.Add(dataSpawner);
        }
    }





    public DataTrain MyDataTrain
    {
        get { return myDataTrain; }
    }
    public List<DataTrain> DataTrains
    {
        get { return dataTrains; }
    }
    public List<DataSpawner> DataSpawners
    {
        get { return dataSpawners; }
    }
    public DataTile[,] DataTileMatrix
    {
        get { return tileMatrix; }
    }

}
