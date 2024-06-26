using System.Collections;
using System.Linq;
using UnityEngine;


public class SpawnObjects : MonoBehaviour
{
    // NOTE : Est ce qu'il ne faudrait pas supprimer containsObject et juste check si objectToSpawn est null ou pas ?
    private bool containsObject = false;
    private bool coroutineIsAllowed = false;
    IEnumerator waitBeforeSpawn;
    private GameManager gameManager;
    private GameObject objectToSpawn;


    public void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        coroutineIsAllowed = true;

        objectToSpawn = new GameObject();
        objectToSpawn = null;

        waitBeforeSpawn = SpawnObject();
        StartCoroutine(waitBeforeSpawn);
    }

    IEnumerator SpawnObject()
    {
        while (coroutineIsAllowed)
        {
            if (!containsObject)
                objectToSpawn = null;

            yield return new WaitForSeconds(8f);
            if (!containsObject && Random.Range(0, 3) != 0)
            {
                containsObject = true;
                objectToSpawn = GetObjectToSpawn();
                GetObjectToSpawn();
                Instantiate(objectToSpawn, this.transform.position, Quaternion.Euler(0, 0, 0));
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

    public GameObject ObjectToSpawn
    {
        get { return objectToSpawn; }
    }

}
