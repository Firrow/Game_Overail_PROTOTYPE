using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tileManager : MonoBehaviour
{
    public string directionOfTile;
    
    public string DirectionOfTile(GameObject tile)
    {
        return tile.GetComponent(directionOfTile).ToString();
    }
}
