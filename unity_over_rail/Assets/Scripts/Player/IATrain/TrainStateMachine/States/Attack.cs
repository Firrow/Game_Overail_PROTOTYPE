using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using overail.IAResearchObject_;
using overail.DataSpawner_;

/// <summary>
/// Functions useful when IA is in Attack State
/// </summary>

public class Attack : IStateTrain
{
    private int HEALTH_LIMIT_DIVIDED = 2;
    private int BULLET_LIMIT_DIVIDED = 6;

    private IATrain train;
    private bool objectIsFind = false;
    private List<DataSpawner> objects;



    public Attack(IATrain IATrain)
    {
        train = IATrain;
    }

    public void MainExecution()
    {
        UpdateState();

        if (train.CurrentItem is null)
        {
            RequiredObject();
        }
    }



    public void UpdateState()
    {
        if (train.myData.Health < train.MaxHealth / HEALTH_LIMIT_DIVIDED)
        {
            train.ChangeState(new Defense(train));
        }
    }

    private void RequiredObject()
    {
        if (train.myData.BulletQuantity <= (train.GetComponentInChildren<Weapon>().MaxBulletQuantity / BULLET_LIMIT_DIVIDED))
        {
            DataSpawner targetDataSpawner = train.targetToMove as DataSpawner;

            // If the nearest ball is recalculated because the previous one has disappeared
            if (targetDataSpawner == null || targetDataSpawner.ObjectOnSpawnerName != "BulletObject")
            {
                objects = ObjectResearch.ListObjectsInMap(train, "BulletObject");
                objectIsFind = ObjectResearch.FindTargetInObjects(objects, train);
            }
        }
    }
}
