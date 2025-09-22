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

    public enum GameState
    {
        WaitingToStart,
        CountdownToStart,
        GamePlaying,
        GameOver
    }
    private NetworkVariable<GameState> state = new(GameState.WaitingToStart, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    public NetworkVariable<GameState> State => state;
    private float countdown = 3f;
    [SerializeField] private StartScreen startScreen;

    public event OnVariableChangeDelegate OnVariableChange;
    public delegate void OnVariableChangeDelegate(/*NetworkVariable<State> newState*/);



    private void Start()
    {
        state.OnValueChanged += OnStateChanged;

        listOfAllObjectLists = listOfAllObjectLists.Concat(Enumerable.Repeat(usualObjects, PROBABILITY_USUAL_OBJECT)).ToList();
        listOfAllObjectLists = listOfAllObjectLists.Concat(Enumerable.Repeat(unusualObjects, PROBABILITY_UNUSUAL_OBJECT)).ToList();
        listOfAllObjectLists = listOfAllObjectLists.Concat(Enumerable.Repeat(rareObjects, PROBABILITY_RARE_OBJECT)).ToList();
    }

    private void OnStateChanged(GameState previous, GameState current)
    {
        Debug.Log($"Game state changed from {previous} to {current}");
    }

    public int SetPlayerIndex()
    {
        nextPlayerIndex++;
        return nextPlayerIndex;
    }

    //TODO : A GERER PAR LE SERVER
    public void StartCountdown()
    {
        state.Value = GameState.CountdownToStart;
        startScreen.HideButton(); //TODO : RÉPLIQUER LE UNDISPLAY DU BOUTON
        StartCoroutine(DecreaseCountdown());
    }

    //TODO : A SYNCHRONISER CHEZ TOUS LES CLIENTS
    private IEnumerator DecreaseCountdown()
    {
        while (countdown > 0)
        {
            Debug.Log(countdown);
            yield return new WaitForSeconds(1);
            countdown--;
        }
        //TODO : mettre listener sur state sur tous les éléments qui doivent se lancer après
        state.Value = GameState.GamePlaying;
        startScreen.HideScreen();
    }




    /*public NetworkVariable<State> GameState
    {
        get { return state; }
        set 
        { 
            state = value;
            if (OnVariableChange != null)
                OnVariableChange(/*state);
        }
    }*/

}
