using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage;

    void Start()
    {
        damage = 1; //à régler si besoin
        StartCoroutine(EnableCollisionAfterDelay(0.15f));
    }

    void Update()
    {
        
    }

    //Se déclenche quand la balle touche quelque chose
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            //Si la balle entre en collision avec ennemi
            EnemyHealth enemyHealth = collision.transform.GetComponent<EnemyHealth>();
            enemyHealth.TakeDamage(damage);
            Destroy(this.gameObject);
        }
        else if (collision.gameObject.tag == "Player")
        {
            PlayerHealth playerHealth = collision.transform.GetComponent<PlayerHealth>();
            playerHealth.TakeDamage(damage);
            Destroy(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject, 1.5f); //1.5f = temps avant destruction (à régler en fonction de la taille de la map)
        }
    }

    //Donne un delay à l'activation collision balle pour ne pas toucher le joueur qui tir
    IEnumerator EnableCollisionAfterDelay(float delay)
    {
        GetComponent<Collider2D>().enabled = false;
        yield return new WaitForSeconds(delay);
        GetComponent<Collider2D>().enabled = true;
    }

}
