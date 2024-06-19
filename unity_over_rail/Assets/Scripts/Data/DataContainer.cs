using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using overail.DataTain;


public class DataContainer : MonoBehaviour
{
    private GameObject[] trains;
    private List<DataTrain> dataTrains = new List<DataTrain>();
    private GameObject myTrain;
    private DataTrain myDataTrain;

    private void Start()
    {
        trains = GameObject.FindGameObjectsWithTag("Player");
        StartCoroutine(GetAllTrainInStartingGame());
    }


    private void GetAllTrains()
    {
        foreach (var train in trains)
        {
            DataTrain dataTrain = new DataTrain(
                train.gameObject,
                train.GetComponent<Train>().TrainIndex,
                train.GetComponent<Train>().TrainPosition,
                train.GetComponent<Train>().ShieldIsActivate,
                train.GetComponent<Train>().CurrentItem,
                train.GetComponent<Train>().Velocity,
                train.GetComponent<Train>().CurrentHealth,
                train.GetComponentInChildren<Weapon>().CurrentBulletQuantity
                );

            dataTrains.Add(dataTrain);

            if (myTrain == null && train.GetComponent<Train>().TrainIndex == this.GetComponent<IATrain>().TrainIndex)
            {
                myTrain = train.gameObject;
                myDataTrain = dataTrain;
            }
        }
    }


    /*public void ShowList()
    {
        foreach (var dataTrain in dataTrains)
        {
            Debug.Log("Train Index: " + dataTrain.index + ", Velocity: " + dataTrain.speed);
        }
        Debug.Log("-----------------------------------------");
    }*/

    IEnumerator GetAllTrainInStartingGame()
    {
        while (true)
        {
            GetAllTrains();
            yield return new WaitForSeconds(1f);
        }
    }
}
