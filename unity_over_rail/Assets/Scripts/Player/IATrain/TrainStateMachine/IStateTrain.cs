using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStateTrain
{
    public void MainExecution();
    public void UpdateState();

    public void RequiredObject();
}
