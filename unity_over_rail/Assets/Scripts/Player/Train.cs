using System.Collections;
using UnityEngine;
using overail.DataContainer_;
using overail.DataTrain_;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using overail.DataTile_;
using overail.GlobalComponent_;

/// <summary>
/// Main script to managing trains (players and IA)
/// TODO : ranger le script et ses deux enfants !
/// </summary>

public class Train : MonoBehaviour, INotifyPropertyChanged
{
    protected float SPEED = 1;
    private int MAX_HEALTH = 10;

    public GameObject leftArrow;
    public GameObject rightArrow;
    public event PropertyChangedEventHandler PropertyChanged;

    protected GameManager gameManager;
    protected DataContainer dataContainer;
    [SerializeField]
    protected GameObject weapon;
    [SerializeField]
    protected int playerIndex;
    protected float accelerate = 0f;
    protected bool increaseAcceleration = false;
    protected bool decreaseAcceleration = false;
    protected int choice;
    protected int currentHealth;
    protected GameObject currentItem;
    protected HealthBar healthBar;
    protected BulletBar bulletBar;
    protected ObjectSlot objectSlot;
    protected float angle = 0f;

    public string fromDirection; //TODO : créer une valeur publique "firstFromDirection" et mettre fromDirection en private et le set dans le Awake ou Start fromDirection = firstFromDirection
    private bool isDead = false;
    private GameObject currentTile;
    private Transform nextRoad;
    private bool coroutineAllowed = false;
    private bool reversePoints;
    private float velocity;
    private bool isStopped = false;
    private SpawnObjects spawner;
    private bool shieldIsActivate;
    private GameObject shield;
    private int lastChoiceDirection;
    // deplacements mathematique
    private float tParam = 0f;
    private Vector3 trainPosition;
    private Quaternion rotationMemory;



    private void Awake()
    {
        // StartValue
        CurrentHealth = MaxHealth;
    }

    protected void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        dataContainer = gameManager.GetComponent<DataContainer>();

        CurrentHealth = MAX_HEALTH;
        StartCoroutine(StartGame()); // temporaire

        this.choice = 1;
        lastChoiceDirection = 1;
        // Set the default arrow
        ChangeArrowColor(leftArrow.GetComponent<SpriteRenderer>(), rightArrow.GetComponent<SpriteRenderer>());

        shield = this.gameObject.transform.GetChild(1).gameObject;
        shieldIsActivate = shield.activeSelf;

        GetInterface();
    }

    protected void FixedUpdate()
    {
        if (coroutineAllowed)
        {
            StartCoroutine(GoByTheRoute());
        }
        ManageAcceleration();
        velocity = SPEED + accelerate;
    }

    // --------------------------------------------------------------------------------------------------------------------------------------------------
    // ------------------------------------------------------------- ACTIONS -----------------------------------------------------------------------
    // --------------------------------------------------------------------------------------------------------------------------------------------------

    public void PlayerChoiceDirection(int directionValue)
    {
        //TODO : faire un enum avec 0, 1 et -1 (à voir)
        //TODO : faire un switch
        if (directionValue == 0)
        {
            this.choice = lastChoiceDirection;
        }
        else if (directionValue == -1)
        {
            this.choice = 1;
            ChangeArrowColor(leftArrow.GetComponent<SpriteRenderer>(), rightArrow.GetComponent<SpriteRenderer>());
        }
        else if (directionValue == 1)
        {
            this.choice = -1;
            ChangeArrowColor(rightArrow.GetComponent<SpriteRenderer>(), leftArrow.GetComponent<SpriteRenderer>());
        }

        lastChoiceDirection = this.choice;
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

    public void PlayerMoveWeapon(Vector2 valueMovement, bool isKeyboard)
    {
        this.gameObject.GetComponentInChildren<Weapon>().moveWeapon(valueMovement, isKeyboard);
    }

    public void PlayerShoot()
    {
        this.gameObject.GetComponentInChildren<Weapon>().PressShootButton();
    }

    public void UsePickObject()
    {
        if (CurrentItem != null && CurrentItem.TryGetComponent(out IObjects pickedObject))
        {
            pickedObject.UseObject();
            CurrentItem = null;
        }
    }

    // --------------------------------------------------------------------------------------------------------------------------------------------------
    // ------------------------------------------------------------- DEPLACEMENTS -----------------------------------------------------------------------
    // --------------------------------------------------------------------------------------------------------------------------------------------------

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "TileTrigger")
        {
            //# (Prop) Mis à jour ici / Utilisé par le trigger des collisions, les next roads et le train IA et humain
            CurrentTile = collider.transform.parent.gameObject;

            string nextDirection = GetDirection(CurrentTile.GetComponent<Tile>(), FromDirection, choice);

            //# (Prop, Var) Mis à jour ci-dessus / Met à jour la NextRoad (route sur laquelle ont vient de rentrer)
            SetCurrentRoad(CurrentTile, nextDirection);

            //# (Att) Mis à jour dans le Start / Condition de "détection" d'un aiguillage, principalement pour les IA
            if (dataContainer.DataNetworkMap.FindDataTile(CurrentTile).Neighbors[nextDirection].IsSwitch) //donne l'info au train qu'il est arrivé à un aiguillage
            {
                //# (Meth) Déclenche l'évènement "SwitchEnter"
                this.OnSwitchDetected(dataContainer.DataNetworkMap.FindDataTile(CurrentTile).Neighbors[nextDirection], GlobalComponent.OPPOSITE_DIRECTIONS[nextDirection]);
            }

            //# (Var) Mis à jour ci-dessus / Met à jour le FromDirection
            SetFromDirection(nextDirection);            
        }
        else if (collider.gameObject.layer == LayerMask.NameToLayer("Bullets"))
        {
            if (!this.gameObject.transform.GetChild(1).gameObject.activeSelf) // if shield is disable
            {
                TakeDamage(collider.gameObject.GetComponent<Bullet>().Damage);
                Destroy(collider.gameObject);
            }
        }
        else if (collider.gameObject.layer == LayerMask.NameToLayer("Objects") && currentItem == null)
        {
            ManageObjects(collider.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Trains"))
        {
            TakeDamage(MAX_HEALTH);
            isDead = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Spawner")
        {
            spawner = collision.gameObject.GetComponent<SpawnObjects>();
        }
    }

    private void SetCurrentRoad(GameObject tile, string nextDir)
    {
        // DETERMINER LA DIRECTION-----------------------------------------
        //# (Att) Mis à jour ici / Détermine le sens de lecture de la route (courbe de Bézier)
        //#   # Pourquoi un attribut ?
        reversePoints = false;

        // DETERMINER LA BONNE ROUTE-----------------------------------------
        // get next route by name and add it to list
        //# (Var) Mis à jour ici / Détermine le nom de la prochaine route
        string nameNextRoad = FromDirection + nextDir;
        //# (Var) Mis à jour ci-dessus / Cas de route inversé
        if (tile.transform.Find(nameNextRoad) == null)
        {
            //# (Var) Mis à jour ici / Détermine le nom de la prochaine route
            nameNextRoad = nextDir + FromDirection;
            //# (Att) Mis à jour ici / Détermine le sens de lecture de la route (courbe de Bézier)
            //#   # Pourquoi un attribut ?
            reversePoints = true;
        }

        //# (Att) Mis à jour ici / La prochaine route (courbe de Bézier) à parcourir
        //#   # => Remplaçable par un simple appel à *.IndexOf(previousDirection) (?????)
        nextRoad = CurrentTile.transform.Find(nameNextRoad);
    }

    private void SetFromDirection(string goDirection) //TODO : utiliser oppositeDirection
    {
        switch (goDirection)
        {
            case "N":
                FromDirection = "S";
                break;
            case "E":
                FromDirection = "O";
                break;
            case "S":
                FromDirection = "N";
                break;
            case "O":
                FromDirection = "E";
                break;
        }
    }

    /// <summary>
    /// Determine the next direction
    /// </summary>
    /// <param name="tile"></param>
    /// <param name="fromDir"></param>
    /// <param name="choice"></param>
    /// <returns> Directions the train can take to get out of the tile </returns>
    private string GetDirection(Tile tile, string fromDir, int choice)
    {
        int i = CurrentTile.GetComponent<Tile>().directionOfTile.IndexOf(fromDir) + choice;
        return CurrentTile.GetComponent<Tile>().directionOfTile.Substring((i + CurrentTile.GetComponent<Tile>().directionOfTile.Length) % CurrentTile.GetComponent<Tile>().directionOfTile.Length, 1);
    }

    /// <summary>
    /// Coroutine allowing the train to navigate on the bezier curve
    /// </summary>
    /// <returns> Delay before the next coroutine call </returns>
    public IEnumerator GoByTheRoute()
    {
        Vector3 p0;
        Vector3 p1;
        Vector3 p2;
        Vector3 p3;

        coroutineAllowed = false;

        // recovery of point positions in the right direction
        if (reversePoints == true) // reverse direction
        {
            p0 = nextRoad.Find("p4").position;
            p1 = nextRoad.Find("p3").position;
            p2 = nextRoad.Find("p2").position;
            p3 = nextRoad.Find("p1").position;
        }
        else // meaning defined in editor
        {
            p0 = nextRoad.Find("p1").position;
            p1 = nextRoad.Find("p2").position;
            p2 = nextRoad.Find("p3").position;
            p3 = nextRoad.Find("p4").position;
        }

        //Debug.Log("Je t'aime mon amour !");

        while (tParam < 1)
        {
            tParam += Time.deltaTime * velocity;

            if (!isStopped)
            {
                rotationMemory = this.transform.rotation;

                // the train position takes the value of the curve
                trainPosition = Mathf.Pow(1 - tParam, 3) * p0 +
                                  3 * Mathf.Pow(1 - tParam, 2) * tParam * p1 +
                                  3 * (1 - tParam) * Mathf.Pow(tParam, 2) * p2 +
                                  Mathf.Pow(tParam, 3) * p3;

                // Shape rotation according to curve direction : create displacement vector(current and new position) > create angle
                Vector3 dir = new Vector3(trainPosition.x - transform.position.x, trainPosition.y - transform.position.y, 0.0f);
                angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg; 

                // Stockage de la rotation correcte
                rotationMemory = Quaternion.Euler(0, 0, Mathf.Round(angle)); // rotate angle in z only

                // Decorrect gun and train rotation
                float delta = (angle - transform.rotation.eulerAngles.z) % 360;
                Weapon.FixWeaponRotation(delta);
            }

            // Update train position and rotation
            transform.position = trainPosition;
            transform.rotation = rotationMemory;

            yield return new WaitForEndOfFrame();
        }

        tParam = 0;
        coroutineAllowed = true;
    }

    // --------------------------------------------------------------------------------------------------------------------------------------------------
    // ------------------------------------------------------------- HEALTH -----------------------------------------------------------------------------
    // --------------------------------------------------------------------------------------------------------------------------------------------------

    public void TakeDamage(int damage)
    {
        CurrentHealth -= damage;

        if (CurrentHealth <= 0)
        {
            IsDead = true;
            Destroy(this.gameObject);
        }
    }

    // --------------------------------------------------------------------------------------------------------------------------------------------------
    // ------------------------------------------------------------- SPEED MANAGER ----------------------------------------------------------------------
    // --------------------------------------------------------------------------------------------------------------------------------------------------

    private void ManageAcceleration()
    {
        if (increaseAcceleration)
        {
            IncreaseAccelerate();
        }
        else if (decreaseAcceleration)
        {
            DecreaseAccelerate();
        }
    }

    private void IncreaseAccelerate()
    {
        if (velocity < 2.5f)
        {
            if (isStopped)
                isStopped = false;
            else
                accelerate += 0.01f;
        }
    }

    private void DecreaseAccelerate()
    {
        if (velocity > 0.02f)
            accelerate -= 0.015f;
        else
            isStopped = true;
    }

    // --------------------------------------------------------------------------------------------------------------------------------------------------
    // ------------------------------------------------------------- MANAGE OBJECTS ---------------------------------------------------------------------
    // --------------------------------------------------------------------------------------------------------------------------------------------------

    private void ManageObjects(GameObject itemCollided)
    {
        if (!(shieldIsActivate && itemCollided.GetComponent<ShieldObject>())) // case of player take shield item when he already have shield on him
        {
            CurrentItem = GameObject.FindGameObjectWithTag(itemCollided.tag);

            if (CurrentItem.TryGetComponent(out IObjects pickedObject))
                pickedObject.GetTrain(this.gameObject);
        }
        else
        {
            shield.GetComponent<Shield>().ResetShield();
        }

        spawner.ContainsObject = false;
        spawner.RestartCoroutine();
        Destroy(itemCollided.gameObject);
    }

    // --------------------------------------------------------------------------------------------------------------------------------------------------
    // ------------------------------------------------------------- INTERFACE --------------------------------------------------------------------------
    // --------------------------------------------------------------------------------------------------------------------------------------------------
    
    private void GetInterface()
    {
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
    }

    private void ChangeArrowColor(SpriteRenderer actualArrow, SpriteRenderer otherArrow)
    {
        actualArrow.color = new Color(1, 0, 0, 1);
        otherArrow.color = new Color(1, 0, 0, 0);
    }

    // TEMPORAIRE START PARTIE ---------------------------------------------------------------------------------------------------------------
    private IEnumerator StartGame() //TODO : temporary because countdown function when game is more advanced
    {
        yield return new WaitForSeconds(0.1f);
        coroutineAllowed = true;
    }

    /// <summary>
    /// Entering on a switch (by IA)
    /// </summary>
    public virtual void OnSwitchDetected(DataTile detectedSwitch, string fromDirection) { }

    protected void OnPropertyChanged([CallerMemberName] string name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }



    public int PlayerIndex
    {
        get { return playerIndex; }
    }

    public bool IsDead
    {
        get { return isDead; }
        set { 
            isDead = value;
            OnPropertyChanged("IsDead");
        }
    }

    public Weapon Weapon
    {
        get { return weapon.GetComponent<Weapon>(); }
    }

    public Vector3 TrainPosition
    {
        get { return trainPosition; }
    }

    public string FromDirection
    {
        get { return fromDirection; }
        set { fromDirection = value; }
    }

    public GameObject CurrentTile
    {
        get { return currentTile; }
        set { currentTile = value; }
    }

    public float Velocity
    {
        get { return velocity; }
    }

    public int CurrentHealth
    {
        get { return currentHealth; }
        set { 
            currentHealth = value;
            OnPropertyChanged("CurrentHealth");
        }
    }

    public GameObject CurrentItem
    {
        get { return currentItem; }
        set { 
            currentItem = value;
            OnPropertyChanged("CurrentItem");
        }
    }

    public int MaxHealth
    {
        get { return MAX_HEALTH; }
    }

    public bool ShieldIsActivate
    {
        get { return shieldIsActivate; }
        set { shieldIsActivate = value; }
    }
}
