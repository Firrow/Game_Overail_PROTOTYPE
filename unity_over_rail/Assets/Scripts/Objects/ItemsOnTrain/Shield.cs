using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    public bool isActive;
    private int MAX_SHIELD_HEALTH = 5;
    private int currentShieldHealth;
    private GameObject ownerTrain;


    private void Start()
    {
        currentShieldHealth = MAX_SHIELD_HEALTH;
        ownerTrain = this.gameObject.transform.parent.gameObject;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Bullets"))
        {
            TakeDamage(collision.gameObject.GetComponent<Bullet>().Damage);
            Destroy(collision.gameObject);
        }
    }

    public void TakeDamage(int damage)
    {
        Debug.Log("SHIELD BEFORE : " + currentShieldHealth);
        currentShieldHealth -= damage;
        Debug.Log("SHIELD AFTER : " + currentShieldHealth);

        if (currentShieldHealth == 0)
        {
            gameObject.SetActive(false);
            ownerTrain.GetComponent<Train>().ShieldIsActivate = gameObject.activeSelf;
        }
    }

    public void ResetShield()
    {
        currentShieldHealth = MAX_SHIELD_HEALTH;
        // Remettre sprite bouclier tout propre !
    }
}
