using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldObject : Objects
{
    private float probabilitySpawn;

    private void Start()
    {
        probabilitySpawn = objectsProbabilities["shield"];
    }
}
