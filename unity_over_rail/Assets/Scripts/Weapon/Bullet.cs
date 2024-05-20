using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private int damage;

    void Start()
    {
        damage = 1; 
        StartCoroutine(EnableCollisionAfterDelay(0.4f)); //0.15f
    }


    // Donne un delay à l'activation collision balle pour ne pas toucher le joueur qui tir
    IEnumerator EnableCollisionAfterDelay(float delay)
    {
        GetComponent<Collider2D>().enabled = false;
        yield return new WaitForSeconds(delay);
        GetComponent<Collider2D>().enabled = true;
    }



    public int Damage
    {
        get { return damage; }
    }

}
