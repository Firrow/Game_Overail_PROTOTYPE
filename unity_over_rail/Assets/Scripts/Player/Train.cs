using System.Collections;
using UnityEngine;

public class Train : MonoBehaviour
{
    public string fromDirection; // permettre de le determiner automatiquement //public à remplacer par private + SerializeField ?

    [SerializeField]
    protected GameObject weapon;
    protected float SPEED;
    protected float accelerate = 0f;
    protected bool increaseAcceleration = false;
    protected bool decreaseAcceleration = false;
    protected int choice;
    public GameObject actualItem;
    protected HealthBar healthBar;
    protected BulletBar bulletBar;
    protected ObjectSlot objectSlot;


    private GameObject currentTile;
    private Transform nextRoad;
    private bool coroutineAllowed;
    private bool reversePoints;
    private float velocity;
    private bool isStopped = false;
    private SpawnObjects spawner;

    // deplacements mathematique
    private float tParam;
    private Vector3 trainPosition;

    private int MAX_HEALTH;
    private int currentHealth;
    private bool shieldIsActivate;
    private GameObject shield;



    protected void Start()
    {
        SPEED = 1;
        tParam = 0f;
        coroutineAllowed = true;

        MAX_HEALTH = 10;
        currentHealth = MAX_HEALTH;

        shield = this.gameObject.transform.GetChild(1).gameObject;
        shieldIsActivate = shield.activeSelf;
    }

    protected void Update()
    {
        if (coroutineAllowed)
        {
            StartCoroutine(GoByTheRoute(this.gameObject));
        }
    }

    protected void FixedUpdate()
    {
        ManageAcceleration();
        velocity = SPEED + accelerate;
    }


    // DEPLACEMENT --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Tile")
        {
            // recupere la tuile sur laquelle le joueur est entrain de naviguer
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
        else if (collider.gameObject.layer == LayerMask.NameToLayer("Objects") && actualItem == null)
        {
            ManageObjects(collider.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Trains"))
        {
            TakeDamage(MAX_HEALTH);
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


    // Mouvement
    public IEnumerator GoByTheRoute(GameObject train)
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
                // la position de la forme prend la valeur de la courbe
                trainPosition = Mathf.Pow(1 - tParam, 3) * p0 +
                                  3 * Mathf.Pow(1 - tParam, 2) * tParam * p1 +
                                  3 * (1 - tParam) * Mathf.Pow(tParam, 2) * p2 +
                                  Mathf.Pow(tParam, 3) * p3;

                // Rotation de la forme en fonction de la direction de la courbe
                // creation vecteur de deplacement (grace actuelle et nouvelle position) > creation angle > rotation de l'angle en z seulement
                Vector3 dir = new Vector3(trainPosition.x - transform.position.x, trainPosition.y - transform.position.y, 0.0f);
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

                // Décorréler la rotation de l'arme et du train
                float delta = (angle - transform.rotation.eulerAngles.z) % 360;
                weapon.GetComponent<Weapon>().UpdateWeaponRotation(delta);

                transform.rotation = Quaternion.Euler(0, 0, angle);
            }
            // changement de position du train
            train.transform.position = trainPosition;

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
        if (velocity > 0.015f)
            accelerate -= 0.015f;
        else
            isStopped = true;
    }



    // MANAGE OBJECTS ---------------------------------------------------------------------------------------------------------------
    private void ManageObjects(GameObject itemCollided)
    {
        if (!(shieldIsActivate && itemCollided.GetComponent<ShieldObject>())) // case of player take shield item when he already have shield on him
        {
            actualItem = GameObject.FindGameObjectWithTag(itemCollided.tag);

            if (actualItem.TryGetComponent(out IObjects pickedObject))
                pickedObject.GetTrain(this.gameObject);
        }
        else
        {
            shield.GetComponent<Shield>().ResetShield();
        }

        objectSlot.GetComponent<ObjectSlot>().DisplayActualObject(actualItem.GetComponent<SpriteRenderer>().sprite);
        spawner.ContainsObject = false;
        spawner.RestartCoroutine();
        Destroy(itemCollided.gameObject);
    }









    public GameObject ActualItem
    {
        get { return actualItem; }
        set { actualItem = value; }
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
