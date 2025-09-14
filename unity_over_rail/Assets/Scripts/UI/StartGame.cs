using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StartGame : NetworkBehaviour
{
    public GameManager gameManager;
    public GameObject startGameButton;
    public TextMeshProUGUI countdownText;

    //[Server]
    public void OnStartButtonClicked()
    {
        if (!NetworkServer.active) { return; }
        
        gameManager.StartGame();
    }

    //[Server]
    public void UpdateCountdown(int countdown)
    {
        countdownText.text = countdown.ToString();
    }
}
