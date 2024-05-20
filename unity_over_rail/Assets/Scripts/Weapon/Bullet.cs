using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private int damage;
    private float AUTODESTRUCTION_TIME = 1.4f;

    void Start()
    {
        damage = 1; 
        StartCoroutine(EnableCollisionAfterDelay(0.4f)); //0.15f
        StartCoroutine(DestroyBulletWhenNoCollisions(AUTODESTRUCTION_TIME));
    }


    // Donne un delay à l'activation collision balle pour ne pas toucher le joueur qui tir
    IEnumerator EnableCollisionAfterDelay(float delay)
    {
        GetComponent<Collider2D>().enabled = false;
        yield return new WaitForSeconds(delay);
        GetComponent<Collider2D>().enabled = true;
    }

    IEnumerator DestroyBulletWhenNoCollisions(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(this.gameObject);
    }


    public int Damage
    {
        get { return damage; }
    }

}
