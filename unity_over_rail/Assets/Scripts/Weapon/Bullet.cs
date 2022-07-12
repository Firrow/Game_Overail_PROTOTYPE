using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage;

    // Start is called before the first frame update
    void Start()
    {
        damage = 1; //à régler si besoin
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)//Se déclenche quand la balle touche quelque chose
    {
        if (collision.gameObject.name == "Enemy")
        {
            //Si la balle entre en collision avec ennemi
            EnemyHealth enemyHealth = collision.transform.GetComponent<EnemyHealth>();
            enemyHealth.TakeDamage(damage);
            Destroy(this.gameObject);
        }
        else if (collision.gameObject.name == "Player")
        {
            PlayerHealth playerHealth = collision.transform.GetComponent<PlayerHealth>();
            playerHealth.TakeDamage(damage);
            Destroy(this.gameObject);
        }
        else
        {
            Physics2D.IgnoreLayerCollision(0, 1);
            Destroy(this.gameObject, 5f); //5f = temps avant destruction (à régler en fonction de la taille de la map)
        }
    }
}
