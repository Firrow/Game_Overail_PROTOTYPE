using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script of the first tiles on network
/// </summary>

public class TileStart : MonoBehaviour
{
    public string directionToDelete;



    void Start()
    {
        StartCoroutine(closeFirstRoads());
    }



    /// <summary>
    /// After 2 secondes, when all trains are on the network, fromDirection are delete in all possible directions of tile start
    /// Prevents the train from leaving the network
    /// </summary>
    IEnumerator closeFirstRoads()
    {
        yield return new WaitForSeconds(2f);
        this.GetComponent<Tile>().directionOfTile = this.GetComponent<Tile>().directionOfTile.Replace(directionToDelete, "");
        //TODO : set barrier animation
    }
}
