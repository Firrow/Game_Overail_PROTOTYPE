using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObjects : MonoBehaviour
{
    // Est ce qu'un objet est déjà présent ?
    private bool containsObject = false;
    private bool coroutineIsAllowed;

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
                Debug.Log("yolo");
                //containsObject = true;
            }
            yield return new WaitForSeconds(7f);
        }
    }
}
