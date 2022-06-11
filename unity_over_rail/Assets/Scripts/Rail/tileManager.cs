using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tileManager : MonoBehaviour
{

    public int tilePositionX;
    public int tilePositionY;
    public string directionOfTile;
    public Transform[] tilesRoad;
    public bool isReferral;

    public string DirectionOfTile(GameObject tile)
    {
        return tile.GetComponent(directionOfTile).ToString();
    }
}
