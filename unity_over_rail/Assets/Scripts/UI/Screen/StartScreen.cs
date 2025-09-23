using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StartScreen : MonoBehaviour
{
    [SerializeField] GameObject startButton;
    [SerializeField] TMP_Text countdownText;
    private GameManager gameManager;


    private void Awake()
    {
        ShowScreen();
        HideButton();
    }

    private void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }



    public void OnStartClick()
    {
        gameManager.StartCountdownClientRpc();
        HideButton();
    }

    public void ShowButton()
    {
        startButton.SetActive(true);
    }

    private void ShowScreen()
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
