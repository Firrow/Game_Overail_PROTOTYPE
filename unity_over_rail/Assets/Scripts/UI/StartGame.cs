//using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StartGame : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject startGameButton;
    public TextMeshProUGUI countdownText;

    public void OnStartButtonClicked()
    {
        gameManager.StartGame();
    }

    public void UpdateCountdown(int countdown)
    {
        countdownText.text = countdown.ToString();
    }
}
