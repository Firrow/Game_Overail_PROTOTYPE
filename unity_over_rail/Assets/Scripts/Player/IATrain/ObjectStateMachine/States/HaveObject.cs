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

    /*// Start is called before the first frame update
    void Start()
    {
        
    }*/

    public void MainExecution()
    {
        if (!train.myData.CurrentObject)
        {
            train.ChangeState2(new DontHaveObject(train));
        }
    }
}

