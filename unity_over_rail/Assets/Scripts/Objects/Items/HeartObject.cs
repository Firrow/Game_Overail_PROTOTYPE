using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartObject : Objects
{
    private float probabilitySpawn;

    private void Start()
    {
        probabilitySpawn = objectsProbabilities["heart"];
    }
}
