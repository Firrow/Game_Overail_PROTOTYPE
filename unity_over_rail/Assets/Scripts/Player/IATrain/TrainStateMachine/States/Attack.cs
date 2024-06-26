using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : IStateTrain
{
    private IATrain train;
    // TODO : Voir pour éviter la redondance
    private int HEALTH_LIMIT_DIVIDED = 2;
    private int BULLET_LIMIT_DIVIDED = 6;

    public Attack(IATrain IATrain)
    {
        train = IATrain;
    }

    public void MainExecution()
    {
        if (train.myData.Health < train.MaxHealth / HEALTH_LIMIT_DIVIDED)
        {
            train.ChangeState1(new Defense(train));
        }
    }
}
