using overail.DataTile;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileStart : MonoBehaviour
{
    public string directionToDelete;

    void Start()
    {
        StartCoroutine(closeFirstRoads());
    }

    IEnumerator closeFirstRoads()
    {
        yield return new WaitForSeconds(2f);
        this.GetComponent<DataTile>().directionOfTile = this.GetComponent<DataTile>().directionOfTile.Replace(directionToDelete, "");
        //mettre animation barriere
    }
}
