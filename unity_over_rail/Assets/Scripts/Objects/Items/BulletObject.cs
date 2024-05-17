using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletObject : Objects
{
    private float probabilitySpawn;

    private void Start()
    {
        probabilitySpawn = objectsProbabilities["bullet"];
    }
}
