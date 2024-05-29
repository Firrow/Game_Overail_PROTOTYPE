using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TempRotation : MonoBehaviour
{
    public void ShowRotationValue(Quaternion rotation)
    {
        this.GetComponent<Text>().text = "" + rotation;
    }
}
