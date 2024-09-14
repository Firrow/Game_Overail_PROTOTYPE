using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using overail.DataTile_;
using overail.DataSpawner_;
using overail.DataContainer_;


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
        private List<DataSpawner> dataSpawners;



        public DataMap()
        {
            CreateTileMatrix();
            this.dataSpawners = GetAllSpawners();
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

        private Dictionary<string, DataTile> GetNeighbors(DataTile dataTile) //TODO : ranger la fonction pour plus de clareté
        {
            if (dataTile.Neighbors.Count == 0)
            {
                foreach (char direction in dataTile.DirectionsOfTile)
                {
                    // Prevents tileStarts from causing problems with their 3rd direction, which is subsequently deleted
                    try
                    {
                        switch (direction.ToString())
                        {
                            case "N":
                                dataTile.Neighbors.Add("N", tileMatrix[dataTile.Coordinates.x - 1, dataTile.Coordinates.y]);
                                break;
                            case "E":
                                dataTile.Neighbors.Add("E", tileMatrix[dataTile.Coordinates.x, dataTile.Coordinates.y + 1]);
                                break;
                            case "S":
                                dataTile.Neighbors.Add("S", tileMatrix[dataTile.Coordinates.x + 1, dataTile.Coordinates.y]);
                                break;
                            case "O":
                                dataTile.Neighbors.Add("O", tileMatrix[dataTile.Coordinates.x, dataTile.Coordinates.y - 1]);
                                break;
                        }
                    }
                    catch (IndexOutOfRangeException) { };
                }
            }

            return dataTile.Neighbors;
        }

        /// <summary>
        /// Get all next tiles on the road where the train is.
        /// </summary>
        /// <param name="tile"></param>
        /// <param name="fromDirection"></param>
        /// <returns> One or more tuple of the fromDirection and his opposite tile 
        /// (the next tile in a straight line and all next possible tiles of a switch) </returns>
        public Dictionary<string, DataTile> GetNextTiles(DataTile tile, string fromDirection)
        {
            Dictionary<string, DataTile> nextTiles = new Dictionary<string, DataTile>();

            if (tile is not null)
            {
                foreach (KeyValuePair<string, DataTile> neighbor in tile.Neighbors)
                {
                    if (neighbor.Key.ToString() != fromDirection)
                    {
                        nextTiles.Add(DataContainer.OppositeDirections[neighbor.Key.ToString()], neighbor.Value);
                    }
                }
            }

            return nextTiles;
        }

        /// <summary>
        /// Get the next switch on the road where the train is.
        /// </summary>
        /// <param name="currentTile"></param>
        /// <param name="fromDirection"></param>
        /// <returns> the dataTile of the finded switch </returns>
        /*
        public KeyValuePair<string, DataTile> GetNextSwitchOnMap(DataTile currentTile, string fromDirection)
        {
            return currentTile.IsSwitch ? new KeyValuePair<string, DataTile>(fromDirection, currentTile) : GetNextSwitchOnMap(GetNextTiles(currentTile, fromDirection).FirstOrDefault());
        }
        public KeyValuePair<string, DataTile> GetNextSwitchOnMap(KeyValuePair<string, DataTile> currentTile)
        {
            return GetNextSwitchOnMap(currentTile.Value, currentTile.Key);
        }
        */
        public KeyValuePair<string, DataTile> GetNextSwitchOnMap(KeyValuePair<string, DataTile> currentTile)
        {
            return currentTile.Value.IsSwitch ? currentTile : GetNextSwitchOnMap(GetNextTiles(currentTile.Value, currentTile.Key).FirstOrDefault());
        }
        public KeyValuePair<string, DataTile> GetNextSwitchOnMap(DataTile currentTile, string fromDirection)
        {
            return GetNextSwitchOnMap(new KeyValuePair<string, DataTile>(fromDirection, currentTile));
        }

        /// <summary>
        /// Allow to convert the next direction (NESO) in a choice (left or right) for a given switch
        /// </summary>
        /// <param name="switchTile">Tile that I look</param>
        /// <param name="fromDirection">The direction where I came from</param>
        /// <param name="nextDirection">The direction where I look for next direction</param>
        /// <returns></returns>
        public DataContainer.DirectionChoice WhichChoiceIsNextDirection(DataTile switchTile, string fromDirection, string nextDirection)
        {
            return (switchTile.DirectionsOfTile.IndexOf(nextDirection) - switchTile.DirectionsOfTile.IndexOf(fromDirection))%switchTile.directionsOfTile.Length == 1 ? DataContainer.DirectionChoice.LEFT : DataContainer.DirectionChoice.RIGHT;
        }

        /// <summary>
        /// Check if there is the finded target on the road where the train is.
        /// </summary>
        /// <param name="currentTile"></param>
        /// <param name="fromDirection"></param>
        /// <param name="target"></param>
        /// <returns> true or false </returns>
        public bool ThereIsTargetOnRoad(DataTile currentTile, string fromDirection, ITargetToMove target) 
        {
            // Using shortchut : if the first is true, we do not evaluate the 'or'
            return currentTile == target.CurrentTile || (!currentTile.IsSwitch && ThereIsTargetOnRoad(GetNextTiles(currentTile, fromDirection).FirstOrDefault(), target));
        }
        public bool ThereIsTargetOnRoad(KeyValuePair<string, DataTile> nextTile, ITargetToMove target)
        {
            return ThereIsTargetOnRoad(nextTile.Value, nextTile.Key, target);
        }

        /// <summary>
        /// Find the DataTile of a tile when we have only the tile's game object
        /// </summary>
        /// <param name="tile"></param>
        /// <returns> the DataTile of the tile </returns>
        public DataTile FindDataTile(GameObject tile)
        {
            foreach (var dataTile in tileMatrix)
            {
                if (dataTile.Tile == tile)
                {
                    return dataTile;
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
    }
}
