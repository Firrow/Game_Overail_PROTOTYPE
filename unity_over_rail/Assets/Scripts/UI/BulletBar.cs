using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Interface for bullet quantity
/// </summary>

public class BulletBar : MonoBehaviour
{
    public Slider slider;



    public void SetMaxBullet(int maxBullet)
    {
        slider.maxValue = maxBullet;
        slider.value = maxBullet;
    }

    public void SetBullet(int bullet)
    {
        slider.value = bullet;
    }
}
