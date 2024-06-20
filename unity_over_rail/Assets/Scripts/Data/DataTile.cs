using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace overail.DataTile
{
    public class DataTile
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
}
