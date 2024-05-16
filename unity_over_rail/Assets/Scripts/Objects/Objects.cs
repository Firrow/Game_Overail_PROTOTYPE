using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objects : MonoBehaviour
{
    protected Dictionary<string, float> objectsProbabilities = new Dictionary<string, float>();
    // fonction apparition
    // fonction disparition
    // utilisation

    private void Awake()
    {
        objectsProbabilities.Add("heart", 0.3f);
        objectsProbabilities.Add("bullet", 0.5f);
        objectsProbabilities.Add("shield", 0.2f);
    }
}
