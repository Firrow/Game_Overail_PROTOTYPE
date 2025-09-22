using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class GameManager : NetworkBehaviour
{
    public GameObject[] usualObjects;
    private int PROBABILITY_USUAL_OBJECT = 5;

    public GameObject[] unusualObjects;
    private int PROBABILITY_UNUSUAL_OBJECT = 3;

    public GameObject[] rareObjects;
    private int PROBABILITY_RARE_OBJECT = 2;

    public List<GameObject[]> listOfAllObjectLists = new List<GameObject[]>();
    public List<string> allObjectNames = new List<string>() { "heartObject", "bulletObject", "shieldObject" };

    public GameObject[] playerSpawnPoints;

    private int nextPlayerIndex = -1;

    private enum State
    {
        WaitingToStart,
        CountdownToStart,
        GamePlaying,
        GameOver
    }
    private State state = State.WaitingToStart;
    private float countdown = 3f;
    [SerializeField] private StartScreen startScreen;


    private void Start()
    {
        listOfAllObjectLists = listOfAllObjectLists.Concat(Enumerable.Repeat(usualObjects, PROBABILITY_USUAL_OBJECT)).ToList();
        listOfAllObjectLists = listOfAllObjectLists.Concat(Enumerable.Repeat(unusualObjects, PROBABILITY_UNUSUAL_OBJECT)).ToList();
        listOfAllObjectLists = listOfAllObjectLists.Concat(Enumerable.Repeat(rareObjects, PROBABILITY_RARE_OBJECT)).ToList();
    }

    public int SetPlayerIndex()
    {
        nextPlayerIndex++;
        return nextPlayerIndex;
    }

    //TODO : A GERER PAR LE SERVER
    public void StartCountdown()
    {
        state = State.CountdownToStart;
        startScreen.HideButton(); //TODO : RÉPLIQUER LE UNDISPLAY DU BOUTON
        StartCoroutine(DecreaseCountdown());
    }

    //TODO : A SYNCHRONISER CHEZ TOUS LES CLIENTS
    private IEnumerator DecreaseCountdown()
    {
        Debug.Log(countdown);
        while (countdown > 0)
        {
            yield return new WaitForSeconds(1);
            countdown--;
            startScreen.DisplayCountdown(countdown);
            Debug.Log(countdown);
        }
        //TODO : mettre listener sur state sur tous les éléments qui doivent se lancer après
        state = State.GamePlaying;
        startScreen.HideScreen();
    }
}
