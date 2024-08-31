using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Object Shield and his effect on players' shield
/// Use IObjects interface 
/// </summary>

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
        ownerTrain.GetComponent<Train>().ShieldIsActivate = shieldOnTrain.activeSelf;
        ownerTrain.GetComponent<Train>().CurrentItem = null;
    }
}
