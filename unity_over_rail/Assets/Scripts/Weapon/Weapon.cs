using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float bulletSpeed;
    public float offset;
    public GameObject weapon;
    public GameObject bullet;
    public Transform firePoint;
    public Transform parentBullet;
    public int test = 2;

    private DateTime _timeShot1;
    private DateTime _timeShot2;
    private Transform train;


    void Start()
    {
        _timeShot1 = DateTime.Now;
        train = this.transform.parent;
    }

    // Update is called once per frame
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
        Vector2 mouseOnScreen = (Vector2)Camera.main.ScreenToViewportPoint(Input.mousePosition);

        float angle = AngleBetweenTwoPoints(positionOnScreen, mouseOnScreen);
        // applique la rotation
        transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));


        // TIR-------------------------------------------------------------------------------------------
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotZ + offset);

        // input du joueur
        if (Input.GetMouseButtonDown(0))
        {
            _timeShot2 = DateTime.Now;

            if (_timeShot2 - _timeShot1 >= new TimeSpan(0, 0, 0,0,150)) 
            {
                Shoot(firePoint, bulletSpeed, bullet, weapon);

                _timeShot1 = _timeShot2;
            }
        }
    }

    // calcul de l'angle entre les 2 points
    float AngleBetweenTwoPoints(Vector3 a, Vector3 b)
    {
        return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
    }

    // fonction de tir
    public void Shoot(Transform firePointWeapon, float speed, GameObject projectile, GameObject weaponOrigin)// Fonction appelée lors de l'input de tir
    {
        GameObject bulletInst = Instantiate(projectile, firePointWeapon.transform.position, weaponOrigin.transform.rotation);

        Rigidbody2D rb = bulletInst.GetComponent<Rigidbody2D>();
        rb.AddForce(firePointWeapon.right * speed, ForceMode2D.Impulse);
    }

}
