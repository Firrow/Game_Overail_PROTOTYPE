using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontHaveObject : IStateObject
{
    private IATrain train;

    public DontHaveObject(IATrain IATrain)
    {
        train = IATrain;
    }


    public void MainExecution()
    {
        if (train.myData.CurrentObject)
        {
            train.ChangeState2(new HaveObject(train));
        }
    }
}


