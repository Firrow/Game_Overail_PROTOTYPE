using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth;
    private int _currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        _currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TakeDamage(int damage)
    {
        _currentHealth -= damage; //a voir pour la valeur des dégats
        Debug.Log("PV enemy : " + _currentHealth);
        //Détruire ennemi quand plus de PV

        if (_currentHealth <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            //Si le joueur entre en collision avec ennemi
            Destroy(this.gameObject);
        }
        else if (collision.gameObject.tag == "Player")
        {
            //Si le joueur entre en collision avec autre joueur
            Destroy(this.gameObject);
        }
    }
}
