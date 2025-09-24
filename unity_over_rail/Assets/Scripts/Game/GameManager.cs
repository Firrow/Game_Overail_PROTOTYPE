using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
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

    public void SetupStartScreen()
    {
        if (IsHost)
        {
            startScreen.ShowButton();
        }

        ChangeGameState(GameState.CountdownToStart);
    }

    [ClientRpc]
    public void StartCountdownClientRpc()
    {
        StartCoroutine(DecreaseCountdown()); 
    }

    private IEnumerator DecreaseCountdown()
    {
        while (countdown > 0)
        {
            startScreen.DisplayCountdown(countdown);
            yield return new WaitForSeconds(1);
            countdown--;
        }

        ChangeGameState(GameState.GamePlaying);
        startScreen.HideScreen();
    }



    private void ChangeGameState(GameState newState)
    {
        if (IsServer)
        {
            state.Value = newState;
        }
    }
}
