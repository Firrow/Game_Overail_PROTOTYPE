using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface using by entity that can be considered like a target for IA
/// </summary>

public interface ITargetToMove
{
    public Vector3 Position
    {
        get;
        set;
    }
}
