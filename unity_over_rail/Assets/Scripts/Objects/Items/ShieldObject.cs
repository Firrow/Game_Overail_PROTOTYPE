using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldObject : MonoBehaviour, IObjects
{
    private GameObject ownerTrain;
    private GameObject shieldOnTrain;
    

    public void GetTrain(GameObject train)
    {
        ownerTrain = train;
        shieldOnTrain = ownerTrain.transform.GetChild(1).gameObject;
    }

    public void UseObject()
    {
        shieldOnTrain.SetActive(true);
        ownerTrain.GetComponent<Train>().ActualItem = null;
    }
}
