using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Interface for health quantity
/// </summary>

public class HealthBar : MonoBehaviour
{
    public Slider slider;



    public void SetMaxHealth(int maxHealth) //TODO : utile ?
    {
        slider.maxValue = maxHealth;
        slider.value = maxHealth;
    }

    public void SetHealth(int health)
    {
        Debug.Log("MAJ Barre vie");
        slider.value = health;
    }
}
