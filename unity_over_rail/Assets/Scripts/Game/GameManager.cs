using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;
using Mirror;


public class GameManager : NetworkBehaviour
{
    public GameObject startGameUI;
    public GameObject playerManager;

    public GameObject[] usualObjects;
    private int PROBABILITY_USUAL_OBJECT = 5;

    public GameObject[] unusualObjects;
    private int PROBABILITY_UNUSUAL_OBJECT = 3;

    public GameObject[] rareObjects;
    private int PROBABILITY_RARE_OBJECT = 2;

    public List<GameObject[]> listOfAllObjectLists = new List<GameObject[]>();
    public List<string> allObjectNames = new List<string>() { "heartObject", "bulletObject", "shieldObject" };

    public int nextPlayerIndexAvailable = 0;

    public enum GameState
    {
        Lobby,
        Countdown,
        Playing
    }
    [SyncVar] public GameState currentState = GameState.Lobby;
    int countdown = 3;

    private void Start()
    {
        listOfAllObjectLists = listOfAllObjectLists.Concat(Enumerable.Repeat(usualObjects, PROBABILITY_USUAL_OBJECT)).ToList();
        listOfAllObjectLists = listOfAllObjectLists.Concat(Enumerable.Repeat(unusualObjects, PROBABILITY_UNUSUAL_OBJECT)).ToList();
        listOfAllObjectLists = listOfAllObjectLists.Concat(Enumerable.Repeat(rareObjects, PROBABILITY_RARE_OBJECT)).ToList();

        if (isServer)
        {
            startGameUI.transform.GetChild(0).gameObject.SetActive(true);
        }
        else
        {
            startGameUI.transform.GetChild(0).gameObject.SetActive(false);
        }
    }





    [Server]
    public void StartGame()
    {
        if (currentState == GameState.Lobby)
        {
            currentState = GameState.Countdown;
            StartCoroutine(CountdownCoroutine());
        }
    }

    [Server]
    private System.Collections.IEnumerator CountdownCoroutine()
    {
        while (countdown > 0)
        {
            RpcUpdateCountdown(countdown);
            countdown--;
            yield return new WaitForSeconds(1f);
        }

        currentState = GameState.Playing;
        RpcUpdateCountdown(0);
        RpcUndisplayStartScreen();
        foreach (var spawner in GameObject.FindGameObjectsWithTag("Spawner")) {
            spawner.GetComponent<SpawnObjects>().StartSpawnObject();
        }
    }

    [ClientRpc]
    void RpcUpdateCountdown(int countdownValue)
    {
        // MAJ UI pour chaque client
        startGameUI.GetComponent<StartGame>().UpdateCountdown(countdownValue);
    }

    [ClientRpc]
    void RpcUndisplayStartScreen()
    {
        startGameUI.SetActive(false);
    }
}
