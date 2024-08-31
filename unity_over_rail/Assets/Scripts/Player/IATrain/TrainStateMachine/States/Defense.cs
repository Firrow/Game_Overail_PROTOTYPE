using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using overail.IAResearchObject_;

/// <summary>
/// Functions useful when IA is in Defense State
/// </summary>

public class Defense : IStateTrain
{
    private int HEALTH_LIMIT_DIVIDED = 2;
    private int BULLET_LIMIT_DIVIDED = 6;

    private IATrain train;
    // TODO : Voir pour éviter la redondance



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
            train.ChangeState(new Attack(train));
        }
    }

    //Find Object Heart, Shield
}
