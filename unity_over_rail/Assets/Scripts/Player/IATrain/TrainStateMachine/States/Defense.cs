using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using overail.IAResearchObject;


public class Defense : IStateTrain
{
    private IATrain train;
    // TODO : Voir pour éviter la redondance
    private int HEALTH_LIMIT_DIVIDED = 2;
    private int BULLET_LIMIT_DIVIDED = 6;

    public Defense(IATrain IATrain)
    {
        train = IATrain;
    }

    public void MainExecution()
    {
        UpdateState();
    }

    public void UpdateState()
    {
        if (train.myData.Health >= train.MaxHealth / HEALTH_LIMIT_DIVIDED)
        {
            train.ChangeState1(new Attack(train));
        }
    }

    //Find Object Heart, Shield
}
