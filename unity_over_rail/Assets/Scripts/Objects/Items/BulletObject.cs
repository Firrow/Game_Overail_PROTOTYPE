using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletObject : MonoBehaviour, IObjects
{
    private GameObject ownerTrain;
    private Weapon Weapon;
    private int RECOVERY_BULLET_VALUE = 15;

    public void GetTrain(GameObject train)
    {
        ownerTrain = train;
        Weapon = ownerTrain.GetComponentInChildren<Weapon>();
    }

    public void UseObject()
    {
        Debug.Log("BULLET BEFORE : " + Weapon.CurrentBulletQuantity);
        Weapon.CurrentBulletQuantity +=
            Mathf.Min(RECOVERY_BULLET_VALUE, Weapon.MaxBulletQuantity - Weapon.CurrentBulletQuantity);
        Debug.Log("BULLET AFTER : " + Weapon.CurrentBulletQuantity);


        ownerTrain.GetComponent<Train>().ActualItem = null;
    }
}
