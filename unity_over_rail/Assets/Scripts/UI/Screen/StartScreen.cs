using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StartScreen : MonoBehaviour
{
    [SerializeField] GameObject startButton;
    [SerializeField] TMP_Text countdownText;
    private GameManager gameManager;


    private void Start()
    {
        Show();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    //TODO : SEUL L'HOST PEUT LE FAIRE
    public void OnStartClick()
    {
        gameManager.StartCountdown();
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    public void HideButton()
    {
        startButton.SetActive(false);
    }

    public void HideScreen()
    {
        gameObject.SetActive(false);
    }

    public void DisplayCountdown(float countdownValue)
    {
        countdownText.SetText(countdownValue.ToString());
    }
}
