using Mirror;
using System;
using System.Collections;
using UnityEngine;
using static GameManager;

public class Train : NetworkBehaviour
{
    protected string fromDirection; // permettre de le determiner automatiquement //public à remplacer par private + SerializeField ?
    public int playerIndex = 0;

    [SerializeField]
    protected GameObject weapon;
    protected float SPEED = 1;
    protected float accelerate = 0f;
    protected bool increaseAcceleration = false;
    protected bool decreaseAcceleration = false;
    protected int choice;
    protected GameObject currentItem;
    protected HealthBar healthBar;
    protected BulletBar bulletBar;
    protected ObjectSlot objectSlot;
    protected float angle = 0f;


    private GameObject currentTile;
    private Transform nextRoad;
    private bool coroutineAllowed = false;
    private bool reversePoints;
    private float velocity;
    private bool isStopped = false;
    private SpawnObjects spawner;

    // deplacements mathematique
    private float tParam = 0f;
    private Vector3 trainPosition;
    private Quaternion rotationMemory;

    private int MAX_HEALTH = 10;
    private int currentHealth;
    private bool shieldIsActivate;
    private GameObject shield;

    protected GameManager gameManager;

    protected void Start()
    {
        //coroutineAllowed = true;
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        StartCoroutine(StartGame()); // temporaire
        currentHealth = MAX_HEALTH;

        shield = this.gameObject.transform.GetChild(1).gameObject;
        shieldIsActivate = shield.activeSelf;
    }

    protected void Update()
    {
        
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



    // DEPLACEMENT --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Tile")
        {
            GetNextRoad(collider);
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

            //Undisplay values in interface player
            ResetInterface();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Spawner")
        {
            spawner = collision.gameObject.GetComponent<SpawnObjects>();
        }
    }

    private void GetNextRoad(Collider2D collider)
    {
        // DETERMINER LA DIRECTION-----------------------------------------
        // recuperation de la tuile actuelle
        currentTile = collider.transform.parent.gameObject;

        reversePoints = false;

        string _allDirectionsOfATile = GetPossibleDirections(currentTile);
        int _indexDirection = GetIndexDirection(_allDirectionsOfATile, fromDirection);
        string _goDirection = GetDirection(_indexDirection, choice, _allDirectionsOfATile);


        // DETERMINER LA BONNE ROUTE-----------------------------------------
        // recuperer la prochaine route en fonction du nom et l'ajoute a la liste
        string nameNextRoad = fromDirection + _goDirection;
        if (currentTile.transform.Find(nameNextRoad) == null)
        {
            nameNextRoad = _goDirection + fromDirection;
            reversePoints = true;
        }

        nextRoad = currentTile.transform.Find(nameNextRoad);

        switch (_goDirection)
        {
            case "N":
                fromDirection = "S";
                break;
            case "E":
                fromDirection = "O";
                break;
            case "S":
                fromDirection = "N";
                break;
            case "O":
                fromDirection = "E";
                break;
        }
    }

    // recupere les directions possibles
    private string GetPossibleDirections(GameObject actualTile)
    {
        return actualTile.GetComponent<Tile>().directionOfTile;
    }


    // calcul index de la direction de provenance du joueur dans la liste des directions de la tuile
    private int GetIndexDirection(string allDirections, string previousDirection)
    {
        return allDirections.IndexOf(previousDirection);
    }


    // determine la prochaine direction
    private string GetDirection(int indexOriginDirection, int playerDirection, string allPossibleDirections)
    {
        int i = indexOriginDirection + playerDirection;
        return allPossibleDirections.Substring((i + allPossibleDirections.Length) % allPossibleDirections.Length, 1);
    }


    /// <summary>
    /// Coroutine permettant au train de naviguer sur la courbe de bezier
    /// </summary>
    /// <returns>Delay avant le prochain appel de la coroutine</returns>
    public IEnumerator GoByTheRoute()
    {
        Vector3 p0;
        Vector3 p1;
        Vector3 p2;
        Vector3 p3;

        coroutineAllowed = false;

        // recuperation des positions des points dans bon sens
        if (reversePoints == true) // sens inverse
        {
            p0 = nextRoad.Find("p4").position;
            p1 = nextRoad.Find("p3").position;
            p2 = nextRoad.Find("p2").position;
            p3 = nextRoad.Find("p1").position;
        }
        else // sens definit dans editeur
        {
            p0 = nextRoad.Find("p1").position;
            p1 = nextRoad.Find("p2").position;
            p2 = nextRoad.Find("p3").position;
            p3 = nextRoad.Find("p4").position;
        }

        while (tParam < 1)
        {
            tParam += Time.deltaTime * velocity;

            if (!isStopped)
            {
                rotationMemory = this.transform.rotation;

                // la position de la forme prend la valeur de la courbe
                trainPosition = Mathf.Pow(1 - tParam, 3) * p0 +
                                  3 * Mathf.Pow(1 - tParam, 2) * tParam * p1 +
                                  3 * (1 - tParam) * Mathf.Pow(tParam, 2) * p2 +
                                  Mathf.Pow(tParam, 3) * p3;



                // Rotation de la forme en fonction de la direction de la courbe
                // creation vecteur de deplacement (grace actuelle et nouvelle position) > creation angle > rotation de l'angle en z seulement
                Vector3 dir = new Vector3(trainPosition.x - transform.position.x, trainPosition.y - transform.position.y, 0.0f);
                angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

                // Stockage de la rotation correcte
                rotationMemory = Quaternion.Euler(0, 0, Mathf.Round(angle));

                // Décorreler la rotation de l'arme et du train
                float delta = (angle - transform.rotation.eulerAngles.z) % 360;
                weapon.GetComponent<Weapon>().FixWeaponRotation(delta);
            }

            // changement de position du train
            transform.position = trainPosition;
            // Appliquer la rotation au train
            transform.rotation = rotationMemory;

            yield return new WaitForEndOfFrame();
        }

        // MAJ des parametres apres le deplacement
        tParam = 0;
        coroutineAllowed = true;
    }





    // HEALTH --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    public void TakeDamage(int damage)
    {
        currentHealth -= damage; //a voir pour la valeur des degats

        if (currentHealth <= 0)
        {
            ResetInterface();
            Destroy(this.gameObject);
        }
        healthBar.GetComponent<HealthBar>().SetHealth(currentHealth);
    }



    // SPEED MANAGER ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
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




    // MANAGE OBJECTS ---------------------------------------------------------------------------------------------------------------
    private void ManageObjects(GameObject itemCollided)
    {
        if (!(shieldIsActivate && itemCollided.GetComponent<ShieldObject>())) // case of player take shield item when he already have shield on him
        {
            currentItem = GameObject.FindGameObjectWithTag(itemCollided.tag);

            if (currentItem.TryGetComponent(out IObjects pickedObject))
                pickedObject.GetTrain(this.gameObject);

            objectSlot.GetComponent<ObjectSlot>().DisplayActualObject(currentItem.GetComponent<SpriteRenderer>().sprite);
        }
        else
        {
            shield.GetComponent<Shield>().ResetShield();
        }

        spawner.ContainsObject = false;
        spawner.RestartCoroutine();
        Destroy(itemCollided.gameObject);
    }

    // INTERFACE -------------------------------------------------------------------------------------------------------------------------------------
    private void ResetInterface()
    {
        bulletBar.GetComponent<BulletBar>().SetBullet(0);
        objectSlot.GetComponent<ObjectSlot>().UndisplayActualObject();
    }


    // TEMPORAIRE START PARTIE ---------------------------------------------------------------------------------------------------------------
    // temporaire car fonction décompte quand jeu plus avancé
    private IEnumerator StartGame()
    {
        //yield return new WaitForSeconds(0.1f);
        while (gameManager.currentState != GameState.Playing)
        {
            yield return new WaitForSeconds(0.1f);
        }
        coroutineAllowed = true;
    }




    public GameObject CurrentItem
    {
        get { return currentItem; }
        set { currentItem = value; }
    }

    public int CurrentHealth
    {
        get { return currentHealth; }
        set { currentHealth = value; }
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
