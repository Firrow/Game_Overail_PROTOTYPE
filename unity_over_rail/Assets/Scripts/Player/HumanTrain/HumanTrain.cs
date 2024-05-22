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
        ChangeArrowColor(leftArrow.GetComponent<SpriteRenderer>(), rightArrow.GetComponent<SpriteRenderer>());
        this.choice = 1;
        lastChoice = 1;

        foreach (GameObject HB in GameObject.FindGameObjectsWithTag("HealthBar"))
        {
            if (HB.GetComponent<HealthBar>().index == playerIndex)
            {
                this.healthBar = HB.GetComponent<HealthBar>();
            }
        }
        healthBar.SetMaxHealth(MaxHealth);

        Debug.Log("player : " + this.gameObject.name + " HB : " + healthBar.name);
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



    public void PickObject()
    {
        if (actualItem.TryGetComponent(out IObjects pickedObject))
        {
            pickedObject.UseObject();
            actualItem = null;
        }
    }


    // Créer une fonction pour le changement de couleur des flèches
    private void ChangeArrowColor(SpriteRenderer actualArrow, SpriteRenderer otherArrow)
    {
        actualArrow.color = new Color(1, 0, 0, 1);
        otherArrow.color = new Color(1, 0, 0, 0);
    }

    public void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
    }









    public int GetPlayerIndex()
    {
        return playerIndex;
    }
}
