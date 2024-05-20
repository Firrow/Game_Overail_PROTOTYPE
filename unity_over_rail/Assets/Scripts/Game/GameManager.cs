using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public GameObject[] usualObjects;
    private int PROBABILITY_USUAL_OBJECT = 5;

    public GameObject[] unusualObjects;
    private int PROBABILITY_UNUSUAL_OBJECT = 3;

    public GameObject[] rareObjects;
    private int PROBABILITY_RARE_OBJECT = 2;

    public List<GameObject[]> listOfAllObjectLists = new List<GameObject[]>();
    public List<string> allObjectNames = new List<string>() { "heartObject", "bulletObject", "shieldObject" };

    private void Start()
    {
        listOfAllObjectLists = listOfAllObjectLists.Concat(Enumerable.Repeat(usualObjects, PROBABILITY_USUAL_OBJECT)).ToList();
        listOfAllObjectLists = listOfAllObjectLists.Concat(Enumerable.Repeat(unusualObjects, PROBABILITY_UNUSUAL_OBJECT)).ToList();
        listOfAllObjectLists = listOfAllObjectLists.Concat(Enumerable.Repeat(rareObjects, PROBABILITY_RARE_OBJECT)).ToList();
    }


}
