using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class HaveObject : IStateObject
{
    private IATrain train;

    public HaveObject(IATrain IATrain)
    {
        train = IATrain;
    }

    public void MainExecution()
    {
        UpdateState();
    }

    public void UpdateState()
    {
        if (!train.myData.CurrentObject)
        {
            train.ChangeState2(new DontHaveObject(train));
        }
    }
}

