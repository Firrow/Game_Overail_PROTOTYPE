using System.Collections;
using UnityEngine;

/// <summary>
/// Script of projectile create by players' weapon
/// </summary>

public class Bullet : MonoBehaviour
{
    private float AUTODESTRUCTION_TIME = 1f;
    private float DELAY_COLLISION = 0.175f;

    private int damage = 1;



    void Start()
    {
        StartCoroutine(EnableCollisionAfterDelay(DELAY_COLLISION));
        StartCoroutine(DestroyBulletWhenNoCollisions(AUTODESTRUCTION_TIME));
    }



    /// <summary>
    /// Delays ball collision activation to avoid hitting the shooting player
    /// </summary>
    /// <param name="delay"></param>
    ///TODO : améliorer le système pour ne plus utiliser de delay
    IEnumerator EnableCollisionAfterDelay(float delay)
    {
        GetComponent<Collider2D>().enabled = false;
        yield return new WaitForSeconds(delay);
        GetComponent<Collider2D>().enabled = true;
    }

    /// <summary>
    /// Destroy bullet when there has been no collision with another train
    /// </summary>
    /// <param name="delay"></param>
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
