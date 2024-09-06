using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using overail.DataTile_;
using overail.DataSpawner_;


/// <summary>
/// API
/// All Datas about the map for IA
/// </summary>

namespace overail.DataMap_
{
    public static class DataMap
    {
        private static DataTile[,] tileMatrix;
        private static GameObject[] tiles;



        public static DataTile[,] CreateTileMatrix()
        {
            Debug.Log("CreateTileMatrix");
            GridLayout grid = GameObject.FindObjectOfType<GridLayout>();
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
                        grid.CellToWorld(grid.WorldToCell(tiles[element].transform.position)),
                        tiles[element].GetComponent<Tile>().directionOfTile,
                        tiles[element].GetComponent<Tile>().containsSpawner,
                        tiles[element].GetComponent<Tile>().isSwitch,
                        new DataTile.PositionInMatrix { x = i, y = j }
                    );
                    element++;
                }
            }

            foreach (var tile in tileMatrix)
            {
                GetNeighbors(tile);
            }

            return tileMatrix;
        }

        private static List<DataTile> GetNeighbors(DataTile dataTile) //TODO : ranger la fonction pour plus de clareté
        {
            if (dataTile.Neighbors.Count == 0)
            {
                foreach (var direction in dataTile.directionOfTile)
                {
                    // Prevents tileStarts from causing problems with their 3rd direction, which is subsequently deleted
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

        public static List<DataSpawner> GetAllSpawners(GameObject[] spawners)
        {
            Debug.Log("GetAllSpawners");
            List<DataSpawner> dtSpawners = new List<DataSpawner>();
            foreach (var spawner in spawners)
            {
                DataSpawner dataSpawner = new DataSpawner(
                        spawner.gameObject,
                        spawner.transform.parent.gameObject,
                        spawner.transform.position
                    );

                dtSpawners.Add(dataSpawner);
            }

            return dtSpawners;
        }
    }
}
