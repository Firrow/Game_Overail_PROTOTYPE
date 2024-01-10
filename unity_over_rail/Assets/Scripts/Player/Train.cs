using System.Collections;
using UnityEngine;

public class Train : MonoBehaviour
{
    public string fromDirection; // permettre de le déterminer automatiquement

    private float speed;
    private GameObject currentTile;
    private Transform nextRoad;
    private bool coroutineAllowed;
    private bool reversePoints;
    protected int choice;

    // déplacements mathématique
    private float tParam;
    private Vector3 trainPosition;
    private Vector3 p0;
    private Vector3 p1;
    private Vector3 p2;
    private Vector3 p3;


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


    // DEPLACEMENT --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    private void OnTriggerEnter2D(Collider2D collider) // récupère la tuile sur laquelle le joueur est entrain de naviguer
    {
        if (collider.gameObject.tag == "Tile")
        {
            GetNextRoad(collider);
        }
        else if (collider.gameObject.tag == "Enemy" || collider.gameObject.tag == "Player")
        {
            Destroy(this.gameObject);
        }
        // Détection d'objets
    }

    private void GetNextRoad(Collider2D collider)
    {
        // DÉTERMINER LA DIRECTION-----------------------------------------
        // récupération de la tuile actuelle
        currentTile = collider.transform.parent.gameObject;


        // Vérifie si le train est sur le réseau
        if (currentTile.transform.GetChild(1).tag == "Untagged")
            currentTile.GetComponent<Tile>().trainOnNetwork = true;

        reversePoints = false;

        string _allDirectionsOfATile = GetPossibleDirections(currentTile);
        int _indexDirection = GetIndexDirection(_allDirectionsOfATile, fromDirection);
        string _goDirection = GetDirection(_indexDirection, choice, _allDirectionsOfATile);


        // DÉTERMINER LA BONNE ROUTE-----------------------------------------
        // récupérer la prochaine route en fonction du nom et l'ajoute à la liste
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

    // récupère les directions possibles
    private string GetPossibleDirections(GameObject actualTile)
    {
        return actualTile.GetComponent<Tile>().directionOfTile;
    }


    // calcul index de la direction de provenance du joueur dans la liste des directions de la tuile
    private int GetIndexDirection(string allDirections, string previousDirection)
    {
        return allDirections.IndexOf(previousDirection);
    }


    // détermine la prochaine direction
    private string GetDirection(int indexOriginDirection, int playerDirection, string allPossibleDirections)
    {
        int i = indexOriginDirection + playerDirection;
        return allPossibleDirections.Substring((i + allPossibleDirections.Length) % allPossibleDirections.Length, 1);
    }


    // Mouvement
    public IEnumerator GoByTheRoute(GameObject train)
    {
        coroutineAllowed = false;

        // récupération des positions des points dans bon sens
        if (reversePoints == true) // sens inverse
        {
            p0 = nextRoad.Find("p4").position;
            p1 = nextRoad.Find("p3").position;
            p2 = nextRoad.Find("p2").position;
            p3 = nextRoad.Find("p1").position;
        }
        else // sens définit dans éditeur
        {
            p0 = nextRoad.Find("p1").position;
            p1 = nextRoad.Find("p2").position;
            p2 = nextRoad.Find("p3").position;
            p3 = nextRoad.Find("p4").position;
        }

        while (tParam < 1)
        {
            tParam += Time.deltaTime * speed;

            // la position de la forme prend la valeur de la courbe
            trainPosition = Mathf.Pow(1 - tParam, 3) * p0 +
                              3 * Mathf.Pow(1 - tParam, 2) * tParam * p1 +
                              3 * (1 - tParam) * Mathf.Pow(tParam, 2) * p2 +
                              Mathf.Pow(tParam, 3) * p3;

            // Rotation de la forme en fonction de la direction de la courbe
            // création vecteur de déplacement (grâce actuelle et nouvelle position) > création angle > rotation de l'angle en z seulement
            Vector3 dir = new Vector3(trainPosition.x - transform.position.x, trainPosition.y - transform.position.y, 0.0f);
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);

            // changement de position du train
            train.transform.position = trainPosition;
            yield return new WaitForEndOfFrame();
        }

        // MAJ des paramètres après le déplacement
        tParam = 0;
        coroutineAllowed = true;
    }





    // HEALTH --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    public void TakeDamage(int damage)
    {
        currentHealth -= damage; //a voir pour la valeur des dégats

        if (currentHealth <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
