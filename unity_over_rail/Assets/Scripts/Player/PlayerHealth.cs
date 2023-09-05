using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
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
        Debug.Log("PV player : " + _currentHealth);
        //détruire joueur quand plus de PV
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        /*if (collider.gameObject.CompareTag("Bullet"))//si la balle entre en collision avec le train
            print("TOUCH !!"); 
        else if (collider.gameObject.CompareTag("Enemy") || collider.gameObject.CompareTag("Player"))
        {
            //animation à mettre
            //A VOIR S'IL SUFFIT D'UNE COLLISION POUR DETRUIRE LES TRAINS
            Destroy(this.gameObject);
        }
        //si autre chose entre en contact avec le train (attention, à faire évoluer pour la suite du jeu :
        //penser à créer un prefab pour les tuile, mettre un tag et vérifier que le parent du collider possède le tag "tile"*/
    }
}
