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
    public class DataMap
    {
        private DataTile[,] tileMatrix;
        private GameObject[] tiles;
        private List<DataTile> dataTiles = new List<DataTile>();
        private List<DataSpawner> dataSpawners;



        public DataMap()
        {
            CreateTileMatrix();
            this.dataSpawners = GetAllSpawners();
            //GetAllDataTiles();
        }



        public void CreateTileMatrix()
        {
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
        }

        private Dictionary<char, DataTile> GetNeighbors(DataTile dataTile) //TODO : ranger la fonction pour plus de clareté
        {
            if (dataTile.Neighbors.Count == 0)
            {
                foreach (char direction in dataTile.DirectionsOfTile)
                {
                    // Prevents tileStarts from causing problems with their 3rd direction, which is subsequently deleted
                    try
                    {
                        switch (direction)
                        {
                            case 'N':
                                dataTile.Neighbors.Add('N', tileMatrix[dataTile.Coordinates.x - 1, dataTile.Coordinates.y]);
                                break;
                            case 'E':
                                dataTile.Neighbors.Add('E', tileMatrix[dataTile.Coordinates.x, dataTile.Coordinates.y + 1]);
                                break;
                            case 'S':
                                dataTile.Neighbors.Add('S', tileMatrix[dataTile.Coordinates.x + 1, dataTile.Coordinates.y]);
                                break;
                            case 'O':
                                dataTile.Neighbors.Add('O', tileMatrix[dataTile.Coordinates.x, dataTile.Coordinates.y - 1]);
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

        public DataTile GetNextTile(DataTile tile, string fromDirection)
        {
            foreach (char direction in tile.DirectionsOfTile)
            {
                string directionString = direction.ToString();
                if (directionString != fromDirection)
                {
                    return tile.Neighbors[direction];
                }
            }

            return null;
        }

        public DataTile GetNextSwitchOnMap(DataTile currentTile, string fromDirection)
        {
            GridLayout grid = GameObject.FindObjectOfType<GridLayout>();
            DataTile nextTile = GetNextTile(currentTile, fromDirection);

            if (nextTile.IsSwitch)
            {
                return nextTile;
            }
            else
            {
                return GetNextSwitchOnMap(nextTile, fromDirection);
            }
        }

        /*private void GetAllDataTiles()
        {
            foreach (var tile in tileMatrix)
            {
                dataTiles.Add(tile);
            }
        }*/

        public DataTile FindDataTile(GameObject tile)
        {
            foreach (var dataTile in tileMatrix)
            {
                if (dataTile.Tile == tile)
                {
                    return dataTile;
                }
                //dataTiles.Add(dataTile);
            }

            return null;
        }

        public DataTile GetDataTileOfCurrentTile(GameObject currentTile, List<DataTile> tiles)
        {
            foreach (var tile in tiles)
            {
                if (tile.Tile == currentTile)
                {
                    return tile;
                }
            }

            return null;
        }

        public List<DataSpawner> GetAllSpawners()
        {
            GameObject[] spawners = GameObject.FindGameObjectsWithTag("Spawner");
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



        public List<DataSpawner> DataSpawners
        {
            get { return dataSpawners; }
        }

        public DataTile[,] TileMatrix
        {
            get { return tileMatrix; }
        }

        public List<DataTile> DataTiles //marche ?
        {
            get { return dataTiles; }
        }
    }
}
