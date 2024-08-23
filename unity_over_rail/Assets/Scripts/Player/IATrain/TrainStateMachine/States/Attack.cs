using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using overail.IAResearchObject;


public class Attack : IStateTrain
{
    private IATrain train;
    private int HEALTH_LIMIT_DIVIDED = 2;
    private int BULLET_LIMIT_DIVIDED = 6;
    private bool objectIsFind = false;


    public Attack(IATrain IATrain)
    {
        train = IATrain;
    }

    public void MainExecution()
    {
        UpdateState();

        RequiredObject();
    }

    public void UpdateState()
    {
        if (train.myData.Health < train.MaxHealth / HEALTH_LIMIT_DIVIDED)
        {
            train.ChangeState1(new Defense(train));
        }
    }

    private void RequiredObject()
    {
        if (!objectIsFind)
        {
            if (train.myData.BulletQuantity <= (train.GetComponentInChildren<Weapon>().MaxBulletQuantity / BULLET_LIMIT_DIVIDED))
            {
                Debug.Log("J'AI BESOIN DE BULLETS");
                objectIsFind = ObjectResearch.FindObject(train, "BulletObject");
            }
        }

    }

    //Find Object Bullet si nombre de balle (train.GetComponentInChildren<Weapon>().MaxBulletQuantity / BULLET_LIMIT_DIVIDED) avec private int BULLET_LIMIT_DIVIDED = 3;
}
