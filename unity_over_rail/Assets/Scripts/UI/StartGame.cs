using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StartGame : NetworkBehaviour
{
    public GameManager gameManager;
    public TextMeshProUGUI countdownText;

    [Server]
    public void OnStartButtonClicked()
    {
        if (NetworkServer.active) // Vérifie que c'est bien l'host
        {
            Debug.Log("JE LANCE");
            gameManager.StartGame();
        }
    }

    [Server]
    public void UpdateCountdown(int countdown)
    {
        countdownText.text = countdown.ToString();
    }
}
