using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartObject : MonoBehaviour, IObjects
{
    private GameObject OwnerTrain;
    private int RECOVERY_HEALTH_VALUE = 5;

    public void GetTrain(GameObject train)
    {
        OwnerTrain = train;
    }

    public void UseObject()
    {
        Debug.Log("PV BEFORE : " + OwnerTrain.GetComponent<Train>().CurrentHealth);
        OwnerTrain.GetComponent<Train>().CurrentHealth += 
            Mathf.Min(RECOVERY_HEALTH_VALUE, OwnerTrain.GetComponent<Train>().MaxHealth - OwnerTrain.GetComponent<Train>().CurrentHealth);
        Debug.Log("PV AFTER : " + OwnerTrain.GetComponent<Train>().CurrentHealth);

        OwnerTrain.GetComponent<Train>().ActualItem = null;
    }
}
