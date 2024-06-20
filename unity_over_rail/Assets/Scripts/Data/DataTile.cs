using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataTile : MonoBehaviour
{
    public string directionOfTile;
    public bool containsSpawner;

    private Vector3 tilePosition;

    private void Start()
    {
        tilePosition = this.transform.position;
    }
}
