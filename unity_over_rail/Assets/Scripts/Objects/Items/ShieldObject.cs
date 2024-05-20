using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldObject : MonoBehaviour, IObjects
{
    private GameObject OwnerTrain;

    public void GetTrain(GameObject train)
    {
        OwnerTrain = train;
    }

    public void UseObject()
    {
        Debug.Log("Use SHIELD");
        OwnerTrain.GetComponent<Train>().ActualItem = null;
    }
}
