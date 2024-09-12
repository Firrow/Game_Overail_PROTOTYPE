using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using overail.DataTile_;

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

    public DataTile CurrentTile
    {
        get;
    }

}
