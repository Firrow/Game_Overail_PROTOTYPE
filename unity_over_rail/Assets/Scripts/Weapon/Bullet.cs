using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)//Se déclenche quand la balle touche quelque chose
    {
        if (collision.gameObject.name == "Enemy")
        /*Si la balle entre en collision avec un obstacle, un ennemi ou un allié effectue l'action*/
            Debug.Log("Ennemi touché !");
        else
        {
            Physics2D.IgnoreLayerCollision(0, 1);
        }
    }
}
