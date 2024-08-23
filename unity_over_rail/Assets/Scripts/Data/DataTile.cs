using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace overail.DataTile_
{
    public class DataTile
    {
        public string directionOfTile;
        public bool containsSpawner;
        public bool isSwitch;

        private Vector3 tilePosition;
        private GameObject tile;

        public struct PositionInMatrix
        {
            public int x;
            public int y;
        }
        private PositionInMatrix coordinates;

        private List<DataTile> neighbors = new List<DataTile>();




        public DataTile(GameObject tile, Vector3 tilePosition, string directionOfTile, bool containsSpawner, bool isSwitch, PositionInMatrix coordinates)
        {
            this.tile = tile;
            this.tilePosition = tilePosition;
            this.directionOfTile = directionOfTile;
            this.containsSpawner = containsSpawner;
            this.isSwitch = containsSpawner;
            this.coordinates = coordinates;
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

        public PositionInMatrix Coordinates
        {
            get { return coordinates; }
            set { coordinates = value; }
        }


        /// <summary>
        /// Lazy evaluation : need to use DataContainer.GetNeighbors() to get and set the Neighbors value
        /// </summary>
        public List<DataTile> Neighbors
        {
            get { return neighbors; }
            set { neighbors = value; }
        }
    }

}
