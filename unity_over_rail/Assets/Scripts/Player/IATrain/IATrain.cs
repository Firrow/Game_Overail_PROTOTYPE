using System.Collections;
using UnityEngine;
using overail.DataTain;
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

    private DataTrain myData;
    private enum State1
    {
        Attack,
        Defense,
    }
    private enum State2
    {
        HaveObject,
        DontHaveObject
    }
    private State1 currentState1 = State1.Attack;
    private State2 currentState2 = State2.DontHaveObject;
    private int HEALTH_LIMIT_DIVIDED = 2;
    private int BULLET_LIMIT_DIVIDED = 6;

    private void Awake()
    {
        trainIndex = playerIndex;
    }

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

        myData = this.GetComponent<DataContainer>().MyDataTrain;

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
        StartCoroutine(CheckState1());
        StartCoroutine(CheckState2());
        Debug.Log("State 1 : " + currentState1);
        Debug.Log("State 2 : " + currentState2);
    }


    IEnumerator CheckState1()
    {
        yield return new WaitForSeconds(0.5f);
        if (myData.Health >= MaxHealth / HEALTH_LIMIT_DIVIDED && myData.BulletQuantity >= this.GetComponentInChildren<Weapon>().MaxBulletQuantity/BULLET_LIMIT_DIVIDED)
        {
            currentState1 = State1.Attack;
        }
        else
        {
            currentState1 = State1.Defense;
        }
    }
    IEnumerator CheckState2()
    {
        yield return new WaitForSeconds(0.5f);
        if (myData.CurrentObject == null)
        {
            currentState2 = State2.DontHaveObject;
        }
        else
        {
            currentState2 = State2.HaveObject;
        }
    }





    // --------------------------------------------------------------------------------------------------------------------------------------------------
    // ------------------------------------------------------------- ACTION IN GAME (traitement du choix) --------------------------------------------------------------------------------
    // --------------------------------------------------------------------------------------------------------------------------------------------------
    public void PlayerChoiceDirection(int movementInput) //0 ; 1 ; -1
    {
        /*if (movementInput == 0)
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
        lastChoice = this.choice;*/

        Debug.Log("CHOIX DIRECTION");
    }


    public void PlayerIncreaseVelocity(bool isAccelerate) // true ; false
    {
        /*if (isAccelerate)
            increaseAcceleration = true;
        else
            increaseAcceleration = false;*/
        Debug.Log("AUGMENTE VELOCITY");
    }
    public void PlayerDecreaseVelocity(bool isDecelerate) // true ; false
    {
        /*if (isDecelerate)
            decreaseAcceleration = true;
        else
            decreaseAcceleration = false;*/
        Debug.Log("DIMINUE VELOCITY");
    }


    public void PlayerMoveWeapon(Vector2 target) // position de la cible à atteindre
    {
        /*// calculer position target finale (prendre en compte la vitesse de la cible)
        this.gameObject.GetComponentInChildren<Weapon>().AIMoveWeapon(target);*/
        Debug.Log("BOUGE L'ARME");
    }

    public void PlayerShoot()
    {
        //this.gameObject.GetComponentInChildren<Weapon>().PressShootButton();
        Debug.Log("TIR");
    }

    public void UsePickObject()
    {
        /*if (currentItem.TryGetComponent(out IObjects pickedObject))
        {
            pickedObject.UseObject();
            currentItem = null;
            objectSlot.UndisplayActualObject();
        }*/
        Debug.Log("UTILISE OBJET");
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


    public int PlayerIndex
    {
        get { return playerIndex; }
    }

}
