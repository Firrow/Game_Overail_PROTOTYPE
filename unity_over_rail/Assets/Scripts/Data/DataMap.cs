using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


namespace overail.DataMap
{
    public class DataMap : MonoBehaviour
    {
        private List<GameObject> tilesObject = new List<GameObject>();
        private List<DataTile> dataTiles = new List<DataTile>();
        private List<DataSpawner> spawners = new List<DataSpawner>();



        private void GetAllTiles()
        {
            foreach (var tile in GameObject.FindGameObjectsWithTag("Tile"))
            {
                DataTile dataTile = new DataTile(
                        tile.gameObject,
                        tile.GetComponent<DataTile>().TilePosition,
                        tile.GetComponent<DataTile>().directionOfTile,
                        tile.GetComponent<DataTile>().containsSpawner
                    );

                dataTiles.Add(dataTile);
            }
        }

        private void GetAllSpawners()
        {

        }
    }
}

