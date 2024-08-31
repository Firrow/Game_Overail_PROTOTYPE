using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Object Bullet and his effect on bullet quantity (Weapon.cs)
/// Use IObjects interface 
/// </summary>

public class BulletObject : MonoBehaviour, IObjects
{
    private int RECOVERY_BULLET_VALUE = 15;

    private GameObject ownerTrain;
    private Weapon weapon;
    


    public void GetTrain(GameObject train)
    {
        ownerTrain = train;
        weapon = ownerTrain.GetComponentInChildren<Weapon>();
    }

    public void UseObject()
    {
        weapon.CurrentBulletQuantity +=
            Mathf.Min(RECOVERY_BULLET_VALUE, weapon.MaxBulletQuantity - weapon.CurrentBulletQuantity);
        weapon.GetComponentInParent<HumanTrain>().UpdateBulletBar(weapon.CurrentBulletQuantity);

        ownerTrain.GetComponent<Train>().CurrentItem = null;
    }
}
