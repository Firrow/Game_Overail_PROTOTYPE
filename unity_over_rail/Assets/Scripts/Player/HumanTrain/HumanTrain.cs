using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;


public class HumanTrain : Train
{
    // Flèches (circle actuellement)
    public GameObject leftArrow;
    public GameObject rightArrow;


    [SerializeField]
    private int playerIndex = 0;
    private float movementInput;
    private int lastChoice;


    void Start()
    {
        base.Start();

        foreach (GameObject Element in GameObject.FindGameObjectsWithTag("InterfacePlayer"))
        {
            if (Element.GetComponent<InterfacePlayer>().index == playerIndex)
            {
                this.healthBar = Element.GetComponent<InterfacePlayer>().healthBarPlayer.GetComponent<HealthBar>();
                this.bulletBar = Element.GetComponent<InterfacePlayer>().bulletBarPlayer.GetComponent<BulletBar>();
                this.objectSlot = Element.GetComponent<InterfacePlayer>().objectSlotPlayer.GetComponent<ObjectSlot>();
            }
        }
        bulletBar.SetMaxBullet(weapon.GetComponent<Weapon>().MaxBulletQuantity);
        bulletBar.SetBullet(weapon.GetComponent<Weapon>().CurrentBulletQuantity);
        healthBar.SetMaxHealth(MaxHealth);

        ChangeArrowColor(leftArrow.GetComponent<SpriteRenderer>(), rightArrow.GetComponent<SpriteRenderer>());
        this.choice = 1;
        lastChoice = 1;
    }

    void Update()
    {
        base.Update();
    }

    private void FixedUpdate()
    {
        base.FixedUpdate();
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



    public void UsePickObject()
    {
        if (currentItem.TryGetComponent(out IObjects pickedObject))
        {
            pickedObject.UseObject();
            currentItem = null;
            objectSlot.UndisplayActualObject();
        }
    }


    // Créer une fonction pour le changement de couleur des flèches
    private void ChangeArrowColor(SpriteRenderer actualArrow, SpriteRenderer otherArrow)
    {
        actualArrow.color = new Color(1, 0, 0, 1);
        otherArrow.color = new Color(1, 0, 0, 0);
    }

    public new void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
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
