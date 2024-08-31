using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Script with all function for human players only.
/// </summary>

public class HumanTrain : Train
{
    public GameObject leftArrow;
    public GameObject rightArrow;

    [SerializeField]
    private int playerIndex = 0;
    private float movementInput;
    private int lastChoice;
    private float trainAngle;



    private void Awake()
    {
        trainIndex = playerIndex;
    }

    void Start()
    {
        base.Start();

        // Get the right interface
        foreach (GameObject Element in GameObject.FindGameObjectsWithTag("InterfacePlayer"))
        {
            if (Element.GetComponent<InterfacePlayer>().index == playerIndex)
            {
                this.healthBar = Element.GetComponent<InterfacePlayer>().healthBarPlayer.GetComponent<HealthBar>();
                this.bulletBar = Element.GetComponent<InterfacePlayer>().bulletBarPlayer.GetComponent<BulletBar>();
                this.objectSlot = Element.GetComponent<InterfacePlayer>().objectSlotPlayer.GetComponent<ObjectSlot>();
            }
        }

        // Set this interface
        bulletBar.SetMaxBullet(weapon.GetComponent<Weapon>().MaxBulletQuantity);
        bulletBar.SetBullet(weapon.GetComponent<Weapon>().CurrentBulletQuantity);
        healthBar.SetMaxHealth(MaxHealth);
        //healthBar.SetHealth(currentHealth); //Lequel choisir ??? (garder SetMaxHealth pour les tests)
        // TODO : voir pour mettre à jour automatiquement la barre de vie lorsque la valeur est changée dans IATrain

        // Set the default arrow
        ChangeArrowColor(leftArrow.GetComponent<SpriteRenderer>(), rightArrow.GetComponent<SpriteRenderer>());
        this.choice = 1;
        lastChoice = 1;
    }

    void Update()
    {
        base.Update();
        trainAngle = angle;
    }



    public void PlayerChoiceDirection(InputAction.CallbackContext obj)
    {
        movementInput = obj.ReadValue<Vector2>().x;

        if (movementInput == 0)
        {
            this.choice = lastChoice;
        }
        else if (movementInput == -1)
        {
            this.choice = 1;
            ChangeArrowColor(leftArrow.GetComponent<SpriteRenderer>(), rightArrow.GetComponent<SpriteRenderer>());
        }
        else if (movementInput == 1)
        {
            this.choice = -1;
            ChangeArrowColor(rightArrow.GetComponent<SpriteRenderer>(), leftArrow.GetComponent<SpriteRenderer>());
        }
        lastChoice = this.choice;
    }

    public void PlayerIncreaseVelocity(bool isAccelerate)
    {
        if (isAccelerate)
            increaseAcceleration = true;
        else
            increaseAcceleration = false;
    }

    public void PlayerDecreaseVelocity(bool isDecelerate)
    {
        if (isDecelerate)
            decreaseAcceleration = true;
        else
            decreaseAcceleration = false;
    }

    public void PlayerMoveWeapon(InputAction.CallbackContext obj)
    {
        this.gameObject.GetComponentInChildren<Weapon>().moveWeapon(obj.ReadValue<Vector2>(), obj.control.device is Mouse);
    }

    public void PlayerShoot(InputAction.CallbackContext obj)
    {
        this.gameObject.GetComponentInChildren<Weapon>().PressShootButton();
    }

    public void UsePickObject() //TODO : In Train.cs (?)
    {
        if (currentItem.TryGetComponent(out IObjects pickedObject))
        {
            pickedObject.UseObject();
            currentItem = null;
            objectSlot.UndisplayActualObject();
        }
    }

    private void ChangeArrowColor(SpriteRenderer actualArrow, SpriteRenderer otherArrow)
    {
        actualArrow.color = new Color(1, 0, 0, 1);
        otherArrow.color = new Color(1, 0, 0, 0);
    }

    public void UpdateBulletBar(int updateValue)
    {
        bulletBar.SetBullet(updateValue);
    }



    public int GetPlayerIndex()
    {
        return playerIndex;
    }
}
