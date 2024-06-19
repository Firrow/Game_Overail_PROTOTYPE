using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;


public class IATrain : Train
{
    // Flèches (circle actuellement)
    public GameObject leftArrow;
    public GameObject rightArrow;


    [SerializeField]
    private int playerIndex = 0;
    private int lastChoice;
    private float trainAngle;


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

        //ChangeArrowColor(leftArrow.GetComponent<SpriteRenderer>(), rightArrow.GetComponent<SpriteRenderer>());
        this.choice = 1;
        lastChoice = 1;


        trainIndex = playerIndex;

        // CODER CHOIX IA ENEMY
        //InvokeRepeating("MovementChoice", 0.3f, 0.3f);

        //ArrowColor(cGauche.GetComponent<SpriteRenderer>(), cDroit.GetComponent<SpriteRenderer>(), 1);
    }

    void Update()
    {
        base.Update();
    }

    private void FixedUpdate()
    {
        base.FixedUpdate();
    }




    public void PlayerChoiceDirection(int movementInput) //0 ; 1 ; -1
    {
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

    public void PlayerIncreaseVelocity(bool isAccelerate) // true ; false
    {
        if (isAccelerate)
            increaseAcceleration = true;
        else
            increaseAcceleration = false;
    }
    public void PlayerDecreaseVelocity(bool isDecelerate) // true ; false
    {
        if (isDecelerate)
            decreaseAcceleration = true;
        else
            decreaseAcceleration = false;
    }


    public void PlayerMoveWeapon(Vector2 target) // position de la cible à atteindre
    {
        // calculer position target finale (prendre en compte la vitesse de la cible)
        this.gameObject.GetComponentInChildren<Weapon>().AIMoveWeapon(target);
    }

    public void PlayerShoot()
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







    private void ChangeArrowColor(SpriteRenderer actualArrow, SpriteRenderer otherArrow)
    {
        actualArrow.color = new Color(1, 0, 0, 1);
        otherArrow.color = new Color(1, 0, 0, 0);
    }


    // IA ALÉATOIRE A TESTER
    /*private void MovementChoice()
    {
        int random = Random.Range(0, 2);
        this.choice = (random == 0) ? -1 : 1;
    }

    public new void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
    }*/
}
