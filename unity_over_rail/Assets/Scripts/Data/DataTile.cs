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



        public DataTile(GameObject tile, Vector3 tilePosition, PositionInMatrix coordinates)
        {
            this.tile = tile;
            this.tilePosition = tilePosition;
            this.coordinates = coordinates;
        }

        public override bool Equals(object obj)
        {
            DataTile otherTile = obj as DataTile;
            return otherTile.TilePosition == this.TilePosition;
        }
        public override int GetHashCode()
        {
            return Tile.GetHashCode();
        }

        public override string ToString()
        {
            return this.DirectionsOfTile + " - " + this.TilePosition;
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

        public Dictionary<string, DataTile> Neighbors
        {
            get { return neighbors; }
            set { neighbors = value; }
        }

        public string DirectionsOfTile
        {
            get { return Tile.GetComponent<Tile>().directionOfTile; }
        }
        public bool ContainsSpawner
        {
            get { return Tile.GetComponent<Tile>().containsSpawner; }
        }

        public bool IsSwitch
        {
            get { return Tile.GetComponent<Tile>().isSwitch; }
        }

    }
}
