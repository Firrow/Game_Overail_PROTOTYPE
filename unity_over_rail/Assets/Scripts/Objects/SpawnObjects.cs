using System.Collections;
using System.Linq;
using Unity.Netcode;
using UnityEngine;


public class SpawnObjects : NetworkBehaviour
{
    private bool containsObject = false;
    private bool coroutineIsAllowed = false;
    IEnumerator waitBeforeSpawn;
    private GameManager gameManager;
    private GameObject objectToSpawn;


    public void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        coroutineIsAllowed = true;

        gameManager.State.OnValueChanged += OnGameStateChanged;
    }



    private void OnGameStateChanged(GameManager.GameState previous, GameManager.GameState current)
    {
        if (IsServer && current == GameManager.GameState.GamePlaying)
        {
            waitBeforeSpawn = SpawnObject();
            StartCoroutine(waitBeforeSpawn);
        }
    }

    IEnumerator SpawnObject()
    {
        while (coroutineIsAllowed)
        {
            yield return new WaitForSeconds(8f);
            if (!containsObject && Random.Range(0, 3) != 0)
            {
                containsObject = true;
                objectToSpawn = GetObjectToSpawn();

                GameObject obj = Instantiate(objectToSpawn, this.transform.position, Quaternion.Euler(0, 0, 0));
                NetworkObject objSpawned = obj.GetComponent<NetworkObject>();
                objSpawned.Spawn();
            }
        }
    }

    private GameObject GetObjectToSpawn()
    {
        GameObject[] itemsList = gameManager.listOfAllObjectLists.ElementAt(Random.Range(0, gameManager.listOfAllObjectLists.Count()-1));
        return itemsList.ElementAt(Random.Range(0, itemsList.Count()));
    }

    public void RestartCoroutine()
    {
        StopCoroutine(waitBeforeSpawn);
        waitBeforeSpawn = SpawnObject();
        StartCoroutine(SpawnObject());
    }

    public void DestroyCollectedObject(GameObject objectToDestroy)
    {
        if (IsServer)
        {
            Debug.Log(objectToDestroy.name);
            NetworkObject.Destroy(objectToDestroy);
            RestartCoroutine();
        }
    }



    public bool ContainsObject
    {
        get { return containsObject; }
        set { containsObject = value; }
    }
}
