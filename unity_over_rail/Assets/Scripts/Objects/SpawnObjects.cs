using System.Collections;
using System.Linq;
using UnityEngine;
using Mirror;


public class SpawnObjects : NetworkBehaviour
{
    private bool containsObject = false;
    private bool coroutineIsAllowed = false;
    IEnumerator waitBeforeSpawn;
    private GameManager gameManager;
    private GameObject objectToSpawn;


    public void StartSpawnObject()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        coroutineIsAllowed = true;
        waitBeforeSpawn = SpawnObject();
        StartCoroutine(waitBeforeSpawn);
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

                // On instancie l’objet côté serveur
                GameObject spawned = Instantiate(objectToSpawn, this.transform.position, Quaternion.Euler(0, 0, 0));

                // On le fait apparaître sur tous les clients
                NetworkServer.Spawn(spawned);
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
        StartCoroutine(waitBeforeSpawn);
    }


    public bool ContainsObject
    {
        get { return containsObject; }
        set { containsObject = value; }
    }

}
