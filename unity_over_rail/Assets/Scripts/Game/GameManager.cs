using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;
using UnityEngine.SceneManagement;

/// <summary>
/// All Datas and functions useful for the game managing
/// </summary>

public class GameManager : MonoBehaviour
{
    public List<GameObject[]> listOfAllObjectLists = new List<GameObject[]>();
    public List<string> allObjectNames = new List<string>() { "HeartObject", "BulletObject", "ShieldObject" };

    public GameObject[] usualObjects;
    private int PROBABILITY_USUAL_OBJECT = 5; //5

    public GameObject[] unusualObjects;
    private int PROBABILITY_UNUSUAL_OBJECT = 3; //3

    public GameObject[] rareObjects;
    private int PROBABILITY_RARE_OBJECT = 2; //2

    //TODO : Supprimer un des deux DataContainer et faire attention aux références !
    private void Awake()
    {

    }

    private void Start()
    {
        listOfAllObjectLists = listOfAllObjectLists.Concat(Enumerable.Repeat(usualObjects, PROBABILITY_USUAL_OBJECT)).ToList();
        listOfAllObjectLists = listOfAllObjectLists.Concat(Enumerable.Repeat(unusualObjects, PROBABILITY_UNUSUAL_OBJECT)).ToList();
        listOfAllObjectLists = listOfAllObjectLists.Concat(Enumerable.Repeat(rareObjects, PROBABILITY_RARE_OBJECT)).ToList();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
