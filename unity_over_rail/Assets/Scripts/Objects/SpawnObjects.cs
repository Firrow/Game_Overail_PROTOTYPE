using System.Collections;
using System.Linq;
using UnityEngine;


public class SpawnObjects : MonoBehaviour
{
    private bool containsObject = false;
    private bool coroutineIsAllowed;
    private GameManager gameManager;
    private GameObject objectToSpawn;

    // Détection prise objet par un train


    public void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        coroutineIsAllowed = true;
        StartCoroutine(SpawnObject());
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





    public bool ContainsObject
    {
        get { return containsObject; }
        set { containsObject = value; }
    }

}
