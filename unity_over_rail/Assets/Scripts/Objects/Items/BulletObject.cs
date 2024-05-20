using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletObject : MonoBehaviour, IObjects
{
    private GameObject OwnerTrain;

    public void GetTrain(GameObject train)
    {
        OwnerTrain = train;
    }

    public void UseObject()
    {
        Debug.Log("Use BULLET");
        OwnerTrain.GetComponent<Train>().ActualItem = null;
    }
}
