using overail.DataSpawner_;
using overail.DataTrain_;
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
        //TODO : faire un fichier accessible par tous les autres fichiers pour accéder au script de DataContainer (plutôt que de faire GameObject.FindObjectWithTag("DataContainer").GetComponent<DataContainer>();
        private List<DataTrain> dataTrains = new List<DataTrain>();
        private DataMap dataNetworkMap;
        private static Dictionary<string, string> OPPOSITE_DIRECTIONS = new Dictionary<string, string>() {
            { "N", "S" },
            { "S", "N" },
            { "E", "O" },
            { "O", "E" }
        }; //CONSTANTE GLOBALE AU JEU ENTIER



        private void Awake()
        {
            GetAllTrains();
            dataNetworkMap = new DataMap();
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




        public static Dictionary<string, string> OppositeDirections
        {
            get { return OPPOSITE_DIRECTIONS; }
        }

        public List<DataTrain> DataTrains
        {
            get { return dataTrains; }
        }

        public DataMap DataNetworkMap
        {
            get { return dataNetworkMap; }
        }
    }
}
