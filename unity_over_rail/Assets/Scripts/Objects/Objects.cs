using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objects : MonoBehaviour
{
    public Dictionary<string, float> objectsProbabilities = new Dictionary<string, float>
    {
        {"heart", 0.3f},
        {"bullet", 0.5f},
        {"shield", 0.2f}
    };
    public string[] objectList = new string[] { "heart", "bullet", "shield" };

}
