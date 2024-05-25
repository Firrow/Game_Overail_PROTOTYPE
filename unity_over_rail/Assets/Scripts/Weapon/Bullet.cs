using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private int damage = 1;
    private float AUTODESTRUCTION_TIME = 1f;
    private float DELAY_COLLISION = 0.175f;


    void Start()
    {
        StartCoroutine(EnableCollisionAfterDelay(DELAY_COLLISION));
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
