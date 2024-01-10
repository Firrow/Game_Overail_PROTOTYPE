using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    // idee : mettre un trigger sur les 4 tuiles de départ et quand le train quitte le trigger -> on ferme les routes 

    public string directionOfTile;
    public bool trainOnNetwork; // train

    private List<GameObject> firstRoad = new List<GameObject>();
    private GameObject[] tiles;


    void Start()
    {
        tiles = GameObject.FindGameObjectsWithTag("FirstRoad");
    }

    private void Update()
    {
        if (trainOnNetwork == true) 
            closeFirstRoads();
    }
    
    public void closeFirstRoads()
    {
        // Désactiver route avec tag "FirstRoad" lorsque les joueurs sont définitivement rentrés sur le terrain
        // mettre animation barrière qui se baisse + détruire courbe bé
        
    }
}
