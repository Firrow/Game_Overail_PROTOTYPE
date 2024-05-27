using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartObject : MonoBehaviour, IObjects
{
    private GameObject ownerTrain;
    private int RECOVERY_HEALTH_VALUE = 5;

    public void GetTrain(GameObject train)
    {
        ownerTrain = train;
    }

    public void UseObject()
    {
        ownerTrain.GetComponent<Train>().CurrentHealth += 
            Mathf.Min(RECOVERY_HEALTH_VALUE, ownerTrain.GetComponent<Train>().MaxHealth - ownerTrain.GetComponent<Train>().CurrentHealth);

        ownerTrain.GetComponent<Train>().CurrentItem = null;
    }
}
