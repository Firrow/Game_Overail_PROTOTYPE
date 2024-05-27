using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Weapon : MonoBehaviour
{
    public GameObject bullet;
    public Transform firePoint;

    private int MAX_BULLET_QUANTITY = 30;
    private int currentBulletQuantity = 15;
    private float BULLET_SPEED = 20;
    private float WEAPON_SPEED = 360; 
    private DateTime lastTimeShot;
    private DateTime actualTimeShot;

    private float angleMemory = 0f;
    private Vector2 smoothedMoveValue;

    void Start()
    {
        lastTimeShot = DateTime.Now;
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
            Vector2 mouseOnScreen = Camera.main.ScreenToViewportPoint(moveValue);

            float angle = AngleBetweenTwoPoints(mouseOnScreen, positionOnScreen);
            // applique la rotation
            transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        }
        else
        {
            // Appliquer un facteur de lissage aux valeurs du joystick
            smoothedMoveValue = Vector2.Lerp(smoothedMoveValue, moveValue, Time.deltaTime * 360f);

            // Si je joueur maintient son joystick
            if (smoothedMoveValue.magnitude >= 0.1f)
            {
                float targetAngle = Mathf.Atan2(smoothedMoveValue.y, smoothedMoveValue.x) * Mathf.Rad2Deg;
                angleMemory = targetAngle;
            }

            Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angleMemory));
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * WEAPON_SPEED);
        }
    }

    // calcul de l'angle entre les 2 points
    private float AngleBetweenTwoPoints(Vector3 a, Vector3 b)
    {
        return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
    }

    public void UpdateWeaponRotation(float deltaAngleTrain)
    {
        // Permet d'avoir la rotation de l'arme decorrelee de la rotation du train
        transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, this.transform.rotation.eulerAngles.z - deltaAngleTrain));
    }

    public void PressShootButton()
    {
        actualTimeShot = DateTime.Now;

        if (currentBulletQuantity > 0 && actualTimeShot - lastTimeShot >= new TimeSpan(0, 0, 0, 0, 150))
        {
            Shoot(firePoint, BULLET_SPEED, bullet, this.gameObject);
            currentBulletQuantity--;
            this.GetComponentInParent<HumanTrain>().UpdateBulletBar(currentBulletQuantity);
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
