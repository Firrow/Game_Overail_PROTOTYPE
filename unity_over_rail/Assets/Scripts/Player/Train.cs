using System.Collections;
using UnityEngine;

public class Train : MonoBehaviour
{
    public string fromDirection; // permettre de le determiner automatiquement

    protected float speed;
    protected float accelerate = 0f;
    protected bool increaseAcceleration = false;
    protected bool decreaseAcceleration = false;
    protected int choice;


    private GameObject currentTile;
    private Transform nextRoad;
    private bool coroutineAllowed;
    private bool reversePoints;
    private float velocity;
    private bool isStopped = false;
    [SerializeField]
    private GameObject weapon;

    // deplacements mathematique
    private float tParam;
    private Vector3 trainPosition;

    private int maxHealth;
    private int currentHealth;


    protected void Start()
    {
        speed = 1;
        tParam = 0f;
        coroutineAllowed = true;

        maxHealth = 10;
        currentHealth = maxHealth;
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
        velocity = speed + accelerate;
    }


    // DEPLACEMENT --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    private void OnTriggerEnter2D(Collider2D collider) // recupere la tuile sur laquelle le joueur est entrain de naviguer
    {
        if (collider.gameObject.tag == "Tile")
        {
            GetNextRoad(collider);
        }
        else if (collider.gameObject.tag == "Enemy" || collider.gameObject.tag == "Player")
        {
            Destroy(this.gameObject);
        }

        // Detection d'objets
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
}
