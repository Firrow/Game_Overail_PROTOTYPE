using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Interface for object get by player
/// </summary>

public class ObjectSlot : MonoBehaviour
{
    public GameObject displayPoint;



    public void DisplayActualObject(Sprite icon)
    {
        Color c = displayPoint.GetComponent<Image>().color;
        c.a = 1;
        displayPoint.GetComponent<Image>().color = c;
        displayPoint.GetComponent<Image>().sprite = icon;
    }

    public void UndisplayActualObject()
    {
        Color c = displayPoint.GetComponent<Image>().color;
        c.a = 0;
        displayPoint.GetComponent<Image>().color = c;
        displayPoint.GetComponent<Image>().sprite = null;
    }
}
