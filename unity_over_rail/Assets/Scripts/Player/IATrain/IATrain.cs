using System.Collections;
using UnityEngine;
//using Random = UnityEngine.Random;
using overail.DataTain;



public class IATrain : Train
{
    // Flèches (circle actuellement)
    public GameObject leftArrow;
    public GameObject rightArrow;

    [SerializeField]
    private int playerIndex = 0;
    private int lastChoice;
    private float trainAngle;

    private Vector3 targetPosition;
    private bool targetChanged = false;
    private bool enterOnSwitch = false;



    public DataTrain myData;
    private IStateTrain currentState1;
    private IStateObject currentState2;
    private GameManager gameManager;




    private void Awake()
    {
        trainIndex = playerIndex;
    }

    void Start()
    {
        base.Start();

        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

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
        // healthBar.SetHealth(currentHealth); //Lequel choisir ??? (garder SetMaxHealth pour les tests)

        //ChangeArrowColor(leftArrow.GetComponent<SpriteRenderer>(), rightArrow.GetComponent<SpriteRenderer>());
        this.choice = 1;
        lastChoice = 1;


        myData = this.GetComponent<DataContainer>().MyDataTrain;
        currentState1 = new Attack(this); // État par défaut
        currentState2 = new DontHaveObject(this); // État par défaut

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

        currentState1.MainExecution();
        currentState2.MainExecution();

        Debug.Log("position objectif : " + TargetPosition);
    }







    public void ChangeState1(IStateTrain newState)
    {
        currentState1 = newState;
    }
    public void ChangeState2(IStateObject newState)
    {
        currentState2 = newState;
    }

    public void UpdateTarget(Vector3 newPosition)
    {
        targetChanged = true;
        targetPosition = newPosition;
    }

    public void NeedToChangeDirectionToTarget()
    {
        if (targetChanged || enterOnSwitch)
        {
            // recalculer la prochaine direction à prendre
            // récupérer le prochain aiguillage + ajouter sécurité au cas ou il n'y a pas d'aiguillage

            // remet les flags à false
            targetChanged = false;
            enterOnSwitch = false;
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









    public override void OnSwitchEnter()
    {
        enterOnSwitch = true;
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

    public GameManager GameManager
    {
        get { return gameManager; }
    }

    public IStateTrain CurrentState1
    {
        get { return currentState1; }
    }
    public IStateObject CurrentState2
    {
        get { return currentState2; }
    }

    public Vector3 TargetPosition
    {
        get { return targetPosition; }
        set { targetPosition = value; }
    }
}

