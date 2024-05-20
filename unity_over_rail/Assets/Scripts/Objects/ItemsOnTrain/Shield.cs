using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    public bool isActive;
    private int MAX_SHIELD_HEALTH = 5;
    private int currentShieldHealth;
    private GameObject ownerTrain;

    private void Awake()
    {
        isActive = this.gameObject.activeSelf;
    }
    private void Start()
    {
        currentShieldHealth = MAX_SHIELD_HEALTH;
        ownerTrain = this.gameObject.transform.parent.gameObject;
    }

    public void TakeDamage()
    {
        Debug.Log("SHIELD BEFORE : " + currentShieldHealth);
        currentShieldHealth--;
        Debug.Log("SHIELD AFTER : " + currentShieldHealth);
        Debug.Log("-----------------------------------------------------------------");

        if (currentShieldHealth == 0)
        {
            gameObject.SetActive(false);
        }
    }
}
