using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Weapon : MonoBehaviour
{
    public GameObject bullet;
    public Transform firePoint;

    private int MAX_BULLET_QUANTITY;
    private int currentBulletQuantity;

    private float bulletSpeed;
    private float offset;
    private DateTime lastTimeShot;
    private DateTime actualTimeShot;


    void Start()
    {
        lastTimeShot = DateTime.Now;
        bulletSpeed = 15;

        MAX_BULLET_QUANTITY = 30;
        currentBulletQuantity = 15;      // A METTRE PLUS TARD
    }


    void Update()
    {
        // A DÉCOMMENTER QUAND MENU PAUSE CRÉER : Si le jeu est en pause, on ne permet pas au joueur de tirer
        /*if (PauseMenu.gameIsPaused)
        {
            return;
        }*/
    }

    public void moveWeapon(Vector2 moveValue, bool isKeyboard)
    {
        if (isKeyboard)
        {
            // ORIENTATION DE L'ARME--------------------------------------------------------------------------
            // prend position objet
            Vector2 positionOnScreen = Camera.main.WorldToViewportPoint(transform.position);
            // prend position souris
            Vector2 mouseOnScreen = Camera.main.ScreenToViewportPoint(moveValue); //Input.mousePosition

            float angle = AngleBetweenTwoPoints(mouseOnScreen, positionOnScreen);
            // applique la rotation
            transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        }
        else
        {
            float angle = Mathf.Atan2(moveValue.y, moveValue.x) * Mathf.Rad2Deg;
            this.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
    }

    // calcul de l'angle entre les 2 points
    private float AngleBetweenTwoPoints(Vector3 a, Vector3 b)
    {
        return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
    }

    public void UpdateWeaponRotation(float deltaAngleTrain)
    {
        // Permet d'avoir la rotation de l'arme décorrélé de la rotation du train
        this.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, this.transform.rotation.eulerAngles.z - deltaAngleTrain));
    }

    public void PressShootButton()
    {
        actualTimeShot = DateTime.Now;

        if (currentBulletQuantity > 0 && actualTimeShot - lastTimeShot >= new TimeSpan(0, 0, 0, 0, 150))
        {
            Shoot(firePoint, bulletSpeed, bullet, this.gameObject);
            currentBulletQuantity--;
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

    public int CurrentBulletQuantity
    {
        get { return currentBulletQuantity; }
        set { currentBulletQuantity = value; }
    }

    public int MaxBulletQuantity
    {
        get { return MAX_BULLET_QUANTITY; }
    }
}
