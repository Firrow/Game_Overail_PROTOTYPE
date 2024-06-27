using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace overail.DataSpawner
{
    public class DataSpawner
    {
        private GameObject spawner;
        private GameObject tileParent;
        private Vector3 spawnerPosition;
        private GameObject objectOnSpawner;




        public DataSpawner(GameObject spawner, GameObject tileParent, Vector3 spawnerPosition)
        {
            this.spawner = spawner;
            this.tileParent = tileParent;
            this.spawnerPosition = spawnerPosition;
        }




        public GameObject Spawner
        {
            get { return spawner; }
            set { spawner = value; }
        }

        public GameObject TileParent
        {
            get { return tileParent; }
            set { tileParent = value; }
        }

        public Vector3 SpawnerPosition
        {
            get { return spawnerPosition; }
            set { spawnerPosition = value; }
        }

        public GameObject ObjectOnSpawner
        {
            get { return this.Spawner.GetComponent<SpawnObjects>().ObjectToSpawn; }
        }

        public string ObjectOnSpawnerName
        {
            get { return ObjectOnSpawner ? ObjectOnSpawner.name : "";}
        }

    }
}
