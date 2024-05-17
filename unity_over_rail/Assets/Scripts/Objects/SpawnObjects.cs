using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObjects : MonoBehaviour
{
    private bool containsObject = false;
    private bool coroutineIsAllowed;
    [SerializeField]
    private GameObject[] items;
    private GameObject objectToSpawn;

    // fonction instancier un objet
    // Détection prise objet par un train


    public void Start()
    {
        coroutineIsAllowed = true;
        StartCoroutine(SpawnObject());
    }


    IEnumerator SpawnObject()
    {
        while (coroutineIsAllowed)
        {
            Debug.Log("BEFORE SPAWN");
            if (!containsObject && Random.Range(0, 3) != 0)
            {
                //containsObject = true;
                objectToSpawn = GetObjectToSpawn();
                //call spawn function
                Instantiate(objectToSpawn, this.transform.position, this.transform.rotation);
            }
            yield return new WaitForSeconds(5f);
        }
    }

    private GameObject GetObjectToSpawn()
    {
        // Choose a random value inside the total probability
        float random = UnityEngine.Random.value * CalculateSomeProbability();

        // Go through the elements again, until the chosen value is in the element's probability range
        float current = 0f;
        for (int i = 0; i < items.Length; i++)
        {
            if (current <= random && random < current + items[i].GetComponent<Objects>().objectsProbabilities[items[i].GetComponent<Objects>().objectList[i]])
            {
                return items[i];
            }
            current += items[i].GetComponent<Objects>().objectsProbabilities[items[i].GetComponent<Objects>().objectList[i]];
        }

        return null;
    }

    private float CalculateSomeProbability()
    {
        // Calculate sum probabilities of all elements
        float total = 0f;
        for (int i = 0; i < items.Length; i++)
        {
            total += items[i].GetComponent<Objects>().objectsProbabilities[items[i].GetComponent<Objects>().objectList[i]];
        }

        return total;
    }
}
