using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public string directionOfTile;
    private GameObject[] tiles;
    public Transform TileTrigger;
    public HumanTrain trainMovement;
    public bool onNetwork;
    public List<GameObject> firstRoad = new List<GameObject>();


    void Start()
    {
        tiles = GameObject.FindGameObjectsWithTag("FirstRoad");
        TileTrigger = this.gameObject.transform.GetChild(0);
    }

    private void Update()
    {
        if (onNetwork == true) 
            closeFirstRoads();
    }

    public string DirectionOfTile(GameObject tile)
    {
        return tile.GetComponent(directionOfTile).ToString();
    }

    
    public void closeFirstRoads()
    {
        //Désactiver route avec tag "FirstRoad" lorsque les joueurs sont définitivement rentrés sur le terrain
        //mettre animation barrière qui se baisse
        
    }
}
