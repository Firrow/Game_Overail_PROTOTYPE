using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Train : MonoBehaviour
{
    public float speed;
    public string _fromDirection;
    public int choice;

    private GameObject _currentTile;
    private Transform _nextRoad;
    private bool _coroutineAllowed;
    private bool _reversePoints;

    // déplacements mathématique
    private float _tParam;
    private Vector3 trainPosition;
    private Vector3 p0;
    private Vector3 p1;
    private Vector3 p2;
    private Vector3 p3;



    private int maxHealth;
    private int _currentHealth;


    protected void Start()
    {
        _tParam = 0f;
        _coroutineAllowed = true;

        maxHealth = 10;
        _currentHealth = maxHealth;
    }

    protected void Update()
    {
        if (_coroutineAllowed)
        {
            StartCoroutine(GoByTheRoute(this.gameObject));
        }
    }


    // DEPLACEMENT --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    private void OnTriggerEnter2D(Collider2D collider) // récupère la tuile sur laquelle le joueur est entrain de naviguer
    {
        if (collider.gameObject.tag == "Enemy" || collider.gameObject.tag == "Player")
        {
            Destroy(this.gameObject);
        }
        else if (collider.gameObject.tag == "Tile")
        {
            GetNextRoad(collider);
        }
        // Détection d'objets
    }

    private void GetNextRoad(Collider2D collider)
    {
        // DÉTERMINER LA DIRECTION-----------------------------------------
        // récupération de la tuile actuelle
        _currentTile = collider.transform.parent.gameObject;


        // Vérifie si le train est sur le réseau
        if (_currentTile.transform.GetChild(1).tag == "Untagged")
            _currentTile.GetComponent<Tile>().onNetwork = true;

        _reversePoints = false;

        string _allDirectionsOfATile = PossibleDirections(_currentTile);
        int _indexDirection = GetIndexDirection(_allDirectionsOfATile, _fromDirection);
        string _goDirection = GetDirection(_indexDirection, choice, _allDirectionsOfATile);


        // DÉTERMINER LA BONNE ROUTE-----------------------------------------
        // récupérer la prochaine route en fonction du nom et l'ajoute à la liste
        string nameNextRoad = _fromDirection + _goDirection;
        if (_currentTile.transform.Find(nameNextRoad) == null)
        {
            nameNextRoad = _goDirection + _fromDirection;
            _reversePoints = true;
        }

        _nextRoad = _currentTile.transform.Find(nameNextRoad);

        switch (_goDirection)
        {
            case "N":
                _fromDirection = "S";
                break;
            case "E":
                _fromDirection = "O";
                break;
            case "S":
                _fromDirection = "N";
                break;
            case "O":
                _fromDirection = "E";
                break;
        }
    }

    // récupère les directions possibles
    private string PossibleDirections(GameObject actualTile)
    {
        return actualTile.GetComponent<Tile>().directionOfTile;
    }


    // calcul index de la direction de provenance dans la liste des directions de la tuile
    private int GetIndexDirection(string allDirections, string previousDirection)
    {
        return allDirections.IndexOf(previousDirection);
    }


    // calcul index puis prochaine direction
    private string GetDirection(int index, int playerDirection, string allPossibleDirections)
    {
        int i = index + playerDirection;
        return allPossibleDirections.Substring((i + allPossibleDirections.Length) % allPossibleDirections.Length, 1);
    }


    // Mouvement
    public IEnumerator GoByTheRoute(GameObject train)
    {
        _coroutineAllowed = false;

        // récupération des positions des points dans bon sens
        if (_reversePoints == true) // sens inverse
        {
            p0 = _nextRoad.Find("p4").position;
            p1 = _nextRoad.Find("p3").position;
            p2 = _nextRoad.Find("p2").position;
            p3 = _nextRoad.Find("p1").position;
        }
        else // sens définit dans éditeur
        {
            p0 = _nextRoad.Find("p1").position;
            p1 = _nextRoad.Find("p2").position;
            p2 = _nextRoad.Find("p3").position;
            p3 = _nextRoad.Find("p4").position;
        }

        while (_tParam < 1)
        {
            _tParam += Time.deltaTime * speed;

            // la position de la forme prend la valeur de la courbe
            trainPosition = Mathf.Pow(1 - _tParam, 3) * p0 +
                              3 * Mathf.Pow(1 - _tParam, 2) * _tParam * p1 +
                              3 * (1 - _tParam) * Mathf.Pow(_tParam, 2) * p2 +
                              Mathf.Pow(_tParam, 3) * p3;

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
        _tParam = 0;
        _coroutineAllowed = true;
    }





    // HEALTH --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    public void TakeDamage(int damage)
    {
        _currentHealth -= damage; //a voir pour la valeur des dégats

        //détruire joueur quand plus de PV
        if (_currentHealth <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
