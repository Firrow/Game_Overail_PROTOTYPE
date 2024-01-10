using System;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject bullet;
    public Transform firePoint;

    private float bulletSpeed;
    private float offset;
    private DateTime lastTimeShot;
    private DateTime actualTimeShot;


    void Start()
    {
        lastTimeShot = DateTime.Now;
        bulletSpeed = 15;
    }


    void Update()
    {
        // A DÉCOMMENTER QUAND MENU PAUSE CRÉER : Si le jeu est en pause, on ne permet pas au joueur de tirer
        /*if (PauseMenu.gameIsPaused)
        {
            return;
        }*/

        // ORIENTATION DE L'ARME--------------------------------------------------------------------------
        // prend position objet
        Vector2 positionOnScreen = Camera.main.WorldToViewportPoint(transform.position);
        // prend position souris
        Vector2 mouseOnScreen = Camera.main.ScreenToViewportPoint(Input.mousePosition);

        float angle = AngleBetweenTwoPoints(positionOnScreen, mouseOnScreen);
        // applique la rotation
        transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));


        // TIR-------------------------------------------------------------------------------------------
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotZ + offset);
    }

    // calcul de l'angle entre les 2 points
    private float AngleBetweenTwoPoints(Vector3 a, Vector3 b)
    {
        return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
    }

    public void PressShootButton()
    {
        actualTimeShot = DateTime.Now;

        if (actualTimeShot - lastTimeShot >= new TimeSpan(0, 0, 0, 0, 150))
        {
            Shoot(firePoint, bulletSpeed, bullet, this.gameObject);
            lastTimeShot = actualTimeShot;
        }
    }

    // fonction de tir
    public void Shoot(Transform firePointWeapon, float speed, GameObject projectile, GameObject weaponOrigin) // Fonction appelée lors de l'input de tir
    {
        GameObject bulletInst = Instantiate(projectile, firePointWeapon.transform.position, weaponOrigin.transform.rotation);

        Rigidbody2D rb = bulletInst.GetComponent<Rigidbody2D>();
        rb.AddForce(firePointWeapon.right * speed, ForceMode2D.Impulse);
    }

}
