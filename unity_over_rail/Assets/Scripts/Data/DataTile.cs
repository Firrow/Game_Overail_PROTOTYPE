using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// API
/// All Datas about Tiles on map useful for IA
/// </summary>

namespace overail.DataTile_
{
    public class DataTile
    {
        public string directionsOfTile;
        public bool containsSpawner;
        public struct PositionInMatrix
        {
            public int x;
            public int y;
        }

        private PositionInMatrix coordinates;
        private Vector3 tilePosition;
        private GameObject tile;
        private Dictionary<string, DataTile> neighbors = new Dictionary<string, DataTile>();
        private bool isSwitch;



        public DataTile(GameObject tile, Vector3 tilePosition, string directionsOfTile, bool containsSpawner, bool isSwitch, PositionInMatrix coordinates)
        {
            this.tile = tile;
            this.tilePosition = tilePosition;
            this.directionsOfTile = directionsOfTile;
            this.containsSpawner = containsSpawner;
            this.isSwitch = isSwitch;
            this.coordinates = coordinates;
        }

        public override bool Equals(object obj)
        {
            DataTile otherTile = obj as DataTile;
            return otherTile.Tile != null && otherTile.Tile == this.Tile;
        }
        public override int GetHashCode()
        {
            return Tile.GetHashCode();
        }



        public GameObject Tile
        {
            get { return tile; }
            set { tile = value; }
        }

        public Vector3 TilePosition
        {
            get { return tilePosition; }
            set { tilePosition = value; }
        }

        public PositionInMatrix Coordinates
        {
            get { return coordinates; }
            set { coordinates = value; }
        }

        /// <summary>
        /// Lazy evaluation : need to use DataContainer.GetNeighbors() to get and set the Neighbors value
        /// </summary>
        public Dictionary<string, DataTile> Neighbors
        {
            get { return neighbors; }
            set { neighbors = value; }
        }

        public string DirectionsOfTile
        {
            get { return directionsOfTile; }
            set { directionsOfTile = value; }
        }

        public bool IsSwitch
        {
            get { return isSwitch; }
            set { isSwitch = value; }
        }

    }
}
