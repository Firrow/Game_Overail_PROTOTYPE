using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObjects : MonoBehaviour
{
    // Est ce qu'un objet est déjà présent ?
    private bool containsObject = false;
    private bool coroutineIsAllowed;
    [SerializeField]
    private Objects[] objects;

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
            if (!containsObject)
            {
                // faire spawn objet
                //containsObject = true;
            }
            yield return new WaitForSeconds(7f);
        }
    }

    /*private void getObject()
    {
        // Calculate sum probabilities of all elements
        var total = 0f;
        foreach (var o in objects)
        {
            total += o.
        }

        // Choose a random value inside the total probability
        var random = UnityEngine.Random.value * total;

        // Go through the elements again, until the chosen value is in the element's probability range
        var current = 0f;
        foreach (var el in collection)
        {
            if (current <= random && random < current + el.Probability)
            {
                return el;
            }
            current += el.Probability;
        }

        return default(T);
    }*/
}
