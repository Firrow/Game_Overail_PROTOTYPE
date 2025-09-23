using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class StartNetwork : MonoBehaviour
{
    private GameManager gameManager;



    private void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    public void StartHost()
    {
        NetworkManager.Singleton.StartHost();

        gameManager.SetupStartScreen();
    }

    public void StartClient()
    {
        NetworkManager.Singleton.StartClient();
    }
}
