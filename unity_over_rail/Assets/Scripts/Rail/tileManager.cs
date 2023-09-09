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

    //Désactiver route avec tag "FirstRoad" lorsque les joueurs sont définitivement rentrés sur le terrain
    //mettre animation barrière qui se baisse
}
