using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletObject : MonoBehaviour, IObjects
{
    private GameObject ownerTrain;
    private Weapon weapon;
    private int RECOVERY_BULLET_VALUE = 15;

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
