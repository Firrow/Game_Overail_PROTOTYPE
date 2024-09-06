using overail.DataSpawner_;
using overail.DataTrain_;
using overail.DataTile_;
using overail.DataMap_;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// API
/// Represents a data layer for AI, like Models in a MVC framework.
/// This layer allow AI to access all the information it might need, regardless of how it accesses it.
/// DataContainer is the main entry point of data for IATrain
/// TODO : faire de DataContainer une classe static
/// </summary>

namespace overail.DataContainer_
{
    public class DataContainer : MonoBehaviour
    {
        private List<DataTrain> dataTrains = new List<DataTrain>();
        private DataTrain myDataTrain;
        private GameObject[] spawners;
        private List<DataSpawner> dataSpawners = new List<DataSpawner>();
        private DataTile[,] tileMatrix;



        private void Awake() //temporaire (delete quand DataContainer sera une classe static
        {
            GetAllTrains();
        }

        private void Start()
        {
            tileMatrix = DataMap.CreateTileMatrix();

            spawners = GameObject.FindGameObjectsWithTag("Spawner");
            dataSpawners = DataMap.GetAllSpawners(spawners);
        }



        // --------------------------------------------------------------------------------------------------------------------------------------------------
        // ------------------------------------------------------------- TRAINS -----------------------------------------------------------------------------
        // --------------------------------------------------------------------------------------------------------------------------------------------------

        public void GetAllTrains()
        {
            foreach (GameObject train in GameObject.FindGameObjectsWithTag("Player"))
            {
                DataTrain dataTrain = new DataTrain(
                    train
                );

                dataTrains.Add(dataTrain);
            }
        }

        public DataTrain GetTheTrain(int trainIndex)
        {
            return dataTrains.FirstOrDefault<DataTrain>((dt) => { return trainIndex == dt.Index; });
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
}
