using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Interface for health quantity
/// </summary>

public class HealthBar : MonoBehaviour, IObserver
{
    public Slider slider;



    public void Update(ISubject subject)
    {
        throw new System.NotImplementedException();
    }

    public void SetMaxHealth(int maxHealth) //TODO : utile ?
    {
        slider.maxValue = maxHealth;
        slider.value = maxHealth;
    }

    public void SetHealth(int health)
    {
        slider.value = health;
    }
}
