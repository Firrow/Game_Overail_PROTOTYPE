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
    private float WEAPON_SPEED = 2000;
    private TimeSpan FIRE_RATE = new TimeSpan(0, 0, 0, 0, 150);
    private DateTime lastTimeShot;

    private bool isHumanPlayer = true;
    private bool inputIsKeyboard = false;

    private Vector2 positionTargetOnScreen = new Vector2();
    private Vector2 positionWeaponOnScreen = new Vector2();
    private float targetAngle = 0f;
    private float angleMemory = 0f;
    private Quaternion targetRotation;


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

    private void FixedUpdate()
    {
        if (isHumanPlayer)
        {
            if (inputIsKeyboard)
            {
                CalculWeaponRotationWithMouse();
            }
            else
            {
                CalculWeaponRotationWithJoystick();
            }
        }
        UpdateWeaponRotation(targetRotation);
    }

    public void moveWeapon(Vector2 moveValue, bool isKeyboard)
    {
        if (isKeyboard)
        {
            if (!inputIsKeyboard)
            {
                inputIsKeyboard = true;
            }
            // ORIENTATION DE L'ARME--------------------------------------------------------------------------
            // prend position souris
            positionTargetOnScreen = Camera.main.ScreenToViewportPoint(moveValue);
        }
        else
        {
            // Appliquer un facteur de lissage aux valeurs du joystick
            Vector2 smoothedMoveValue = Vector2.Lerp(new Vector2(transform.rotation.x, transform.rotation.y), moveValue, Time.deltaTime * 360f);

            // Si je joueur maintient son joystick
            if (smoothedMoveValue.magnitude >= 0.1f)
            {
                targetAngle = Mathf.Atan2(smoothedMoveValue.y, smoothedMoveValue.x) * Mathf.Rad2Deg;
                angleMemory = targetAngle;
            }
        }
    }

    public void AIMoveWeapon(Vector2 targetPosition)
    {
        if (isHumanPlayer)
            isHumanPlayer = false;

        positionTargetOnScreen = Camera.main.ScreenToViewportPoint(targetPosition); // JE NE SAIS PAS SI ÇA MARCHE

    }

    // calcul de l'angle entre les 2 points
    private float AngleBetweenTwoPoints(Vector3 a, Vector3 b)
    {
        return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
    }

    public void CalculWeaponRotationWithMouse()
    {
        // prend position objet
        positionWeaponOnScreen = Camera.main.WorldToViewportPoint(transform.position);

        targetAngle = AngleBetweenTwoPoints(positionTargetOnScreen, positionWeaponOnScreen);
        targetRotation = Quaternion.Euler(new Vector3(0f, 0f, targetAngle));
    }

    public void CalculWeaponRotationWithJoystick()
    {
        targetRotation = Quaternion.Euler(new Vector3(0, 0, angleMemory));
    }

    public void UpdateWeaponRotation(Quaternion targetRotationValue)
    {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotationValue, Time.deltaTime * WEAPON_SPEED);
    }

    public void FixWeaponRotation(float deltaAngleTrain)
    {
        // Permet d'avoir la rotation de l'arme decorrelee de la rotation du train
        transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, this.transform.rotation.eulerAngles.z - deltaAngleTrain));
    }


    public void PressShootButton()
    {
        if (currentBulletQuantity > 0 && DateTime.Now - lastTimeShot >= FIRE_RATE)
        {
            Shoot(firePoint, BULLET_SPEED, bullet, this.gameObject);
            currentBulletQuantity--;
            this.GetComponentInParent<HumanTrain>().UpdateBulletBar(currentBulletQuantity);
        }
    }

    public void Shoot(Transform firePointWeapon, float speed, GameObject projectile, GameObject weaponOrigin) // Fonction appelée lors de l'input de tir
    {
        GameObject bulletInst = Instantiate(projectile, firePointWeapon.transform.position, weaponOrigin.transform.rotation);
        Rigidbody2D rb = bulletInst.GetComponent<Rigidbody2D>();
        rb.AddForce(firePointWeapon.right * speed, ForceMode2D.Impulse);

        lastTimeShot = DateTime.Now;
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
