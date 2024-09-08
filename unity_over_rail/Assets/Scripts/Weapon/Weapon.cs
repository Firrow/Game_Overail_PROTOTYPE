using System;
using UnityEngine;
using UnityEngine.InputSystem;
using System.ComponentModel;
using System.Runtime.CompilerServices;

/// <summary>
/// All functions of players' weapon
/// </summary>

public class Weapon : MonoBehaviour, INotifyPropertyChanged
{
    private int MAX_BULLET_QUANTITY = 30;
    private float BULLET_SPEED = 20;
    private float WEAPON_SPEED = 2000;

    public GameObject bullet;
    public Transform firePoint;
    public event PropertyChangedEventHandler PropertyChanged;

    private int currentBulletQuantity;
    private TimeSpan FIRE_RATE = new TimeSpan(0, 0, 0, 0, 150);
    private DateTime lastTimeShot;
    private bool isHumanPlayer = true;
    private bool inputIsKeyboard = false;
    private Vector2 positionTargetOnScreen = new Vector2();
    private Vector2 positionWeaponOnScreen = new Vector2();
    private float targetAngle = 0f;
    private float angleMemory = 0f;
    private Quaternion targetRotation;


    private void Awake()
    {
        // StartValue
        CurrentBulletQuantity = 2; //15
    }

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



    /// <summary>
    /// Move the weapon. Function is divided depending on the player's device.
    /// </summary>
    /// <param name="moveValue"></param>
    /// <param name="isKeyboard"></param>
    public void moveWeapon(Vector2 moveValue, bool isKeyboard) //TODO : trouver le moyen d'avoir une fonction générique et avoir la prise en compte des devices dans PlayerInputHandler
    {
        if (isKeyboard)
        {
            if (!inputIsKeyboard)
            {
                inputIsKeyboard = true;
            }

            positionTargetOnScreen = Camera.main.ScreenToViewportPoint(moveValue);
        }
        else
        {
            // Apply a smoothing factor to joystick values
            Vector2 smoothedMoveValue = Vector2.Lerp(new Vector2(transform.rotation.x, transform.rotation.y), moveValue, Time.deltaTime * 360f);

            // If the player holds his joystick
            if (smoothedMoveValue.magnitude >= 0.1f)
            {
                targetAngle = Mathf.Atan2(smoothedMoveValue.y, smoothedMoveValue.x) * Mathf.Rad2Deg;
                angleMemory = targetAngle;
            }
        }
    }

    /// <summary>
    /// Function using by IA to move weapon
    /// </summary>
    /// <param name="targetPosition"></param>
    public void AIMoveWeapon(Vector2 targetPosition)
    {
        if (isHumanPlayer)
            isHumanPlayer = false;

        positionTargetOnScreen = Camera.main.ScreenToViewportPoint(targetPosition); //TODO : JE NE SAIS PAS SI ÇA MARCHE

    }

    /// <summary>
    /// Calculate angle between two points : Vector a - Vector b
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    private float AngleBetweenTwoPoints(Vector3 a, Vector3 b)
    {
        return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
    }

    public void CalculWeaponRotationWithMouse()
    {
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

    /// <summary>
    /// Update weapon rotation during train's moving : Allows weapon rotation to be decoupled from gear rotation
    /// </summary>
    /// <param name="deltaAngleTrain"></param>
    public void FixWeaponRotation(float deltaAngleTrain)
    {
        transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, this.transform.rotation.eulerAngles.z - deltaAngleTrain));
    }

    /// <summary>
    /// authorize player to shoot depending on time last shoot and bullet quantity
    /// </summary>
    public void PressShootButton()
    {
        if (CurrentBulletQuantity > 0 && DateTime.Now - lastTimeShot >= FIRE_RATE)
        {
            Shoot(firePoint, BULLET_SPEED, bullet, this.gameObject);
            CurrentBulletQuantity--;
            this.GetComponentInParent<HumanTrain>().UpdateBulletBar(CurrentBulletQuantity);
        }
    }

    /// <summary>
    /// Create a bullet and add forces to throw it
    /// </summary>
    /// <param name="firePointWeapon"></param>
    /// <param name="speed"></param>
    /// <param name="projectile"></param>
    /// <param name="weaponOrigin"></param>
    public void Shoot(Transform firePointWeapon, float speed, GameObject projectile, GameObject weaponOrigin) // Fonction appelée lors de l'input de tir
    {
        GameObject bulletInst = Instantiate(projectile, firePointWeapon.transform.position, weaponOrigin.transform.rotation);
        Rigidbody2D rb = bulletInst.GetComponent<Rigidbody2D>();
        rb.AddForce(firePointWeapon.right * speed, ForceMode2D.Impulse);

        lastTimeShot = DateTime.Now;
    }





    protected void OnPropertyChanged([CallerMemberName] string name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    public int CurrentBulletQuantity
    {
        get { return currentBulletQuantity; }
        set { 
            currentBulletQuantity = value;
            OnPropertyChanged("CurrentBulletQuantity");
        }
    }

    public int MaxBulletQuantity
    {
        get { return MAX_BULLET_QUANTITY; }
    }
}
