using System.Collections;
using UnityEngine;

//CE SCRIPT UTILE LES TUILES POUR PERMETTRE LE DÉPLACEMENT
public class HumanTrain : MonoBehaviour
{
    public string _fromDirection;
    public float speed;
    //Flèches (circle actuellement)
    public GameObject cLeft;
    public GameObject cRight;

    private GameObject _currentTile;
    private int _choice;
    private Transform _nextRoad;

    //déplacements mathématique
    private float _tParam;
    private Vector3 trainPosition;
    private bool _coroutineAllowed;
    private bool _reversePoints;
    private Vector3 p0;
    private Vector3 p1;
    private Vector3 p2;
    private Vector3 p3;


    void Start()
    {
        _tParam = 0f;
        _coroutineAllowed = true;
        ChangeArrowColor(cLeft.GetComponent<SpriteRenderer>(), cRight.GetComponent<SpriteRenderer>());
        _choice = 1;
    }


    void Update()
    {
        if (_coroutineAllowed)
        {
            StartCoroutine(GoByTheRoute());
        }
        //base sans inversion
        if (Input.GetKeyDown(KeyCode.Q))
        {
            _choice = 1;
            ChangeArrowColor(cLeft.GetComponent<SpriteRenderer>(), cRight.GetComponent<SpriteRenderer>());
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            _choice = -1;
            ChangeArrowColor(cRight.GetComponent<SpriteRenderer>(), cLeft.GetComponent<SpriteRenderer>());
        }
    }



    //récupère la tuile sur laquelle le joueur est entrain de naviguer
    private void OnTriggerEnter2D(Collider2D collider)
    {
        //DÉTERMINER LA DIRECTION-----------------------------------------
        //Ne pas prendre en compte les balles qui touchent les tuiles
        if (collider.gameObject.CompareTag("Bullet"))
            return;//tu ne fais rien

        //récupération de la tuile actuelle
        _currentTile = collider.transform.parent.gameObject;


        //Vérifie si le train est sur le réseau
        if (_currentTile.transform.GetChild(1).tag == "Untagged")
            _currentTile.GetComponent<Tile>().onNetwork = true;

        _reversePoints = false;

        string _allDirectionsOfATile = PossibleDirections(_currentTile);
        int _indexDirection = GetIndexDirection(_allDirectionsOfATile, _fromDirection);
        string _goDirection = GetDirection(_indexDirection, _choice, _allDirectionsOfATile);

        //DÉTERMINER LA BONNE ROUTE-----------------------------------------
        //récupérer la prochaine route en fonction du nom et l'ajoute à la liste
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


    //récupère les directions possibles
    private string PossibleDirections(GameObject actualTile)
    {
        return actualTile.GetComponent<Tile>().directionOfTile;
    }



    //calcul index de la direction de provenance dans la liste des directions de la tuile
    private int GetIndexDirection(string allDirections, string previousDirection)
    {
        return allDirections.IndexOf(previousDirection);
    }



    //calcul index puis prochaine direction
    private string GetDirection(int index, int playerDirection, string allPossibleDirections)
    {
        int i = index + playerDirection;
        return allPossibleDirections.Substring((i + allPossibleDirections.Length) % allPossibleDirections.Length, 1);
    }

    //Créer une fonction pour le changement de couleur des flèches
    private void ChangeArrowColor(SpriteRenderer actualArrow, SpriteRenderer otherArrow)
    {
        actualArrow.color = new Color(1, 0, 0, 1);
        otherArrow.color = new Color(1, 0, 0, 0);
    }



    //DÉPLACEMENTS------------------------------------------------------------------------
    IEnumerator GoByTheRoute()
    {
        _coroutineAllowed = false;
        //récupération des positions des points dans bon sens
        if (_reversePoints == true) //sens inverse
        {
            p0 = _nextRoad.Find("p4").position;
            p1 = _nextRoad.Find("p3").position;
            p2 = _nextRoad.Find("p2").position;
            p3 = _nextRoad.Find("p1").position;
        }
        else //sens définit dans éditeur
        {
            p0 = _nextRoad.Find("p1").position;
            p1 = _nextRoad.Find("p2").position;
            p2 = _nextRoad.Find("p3").position;
            p3 = _nextRoad.Find("p4").position;
        }

        while (_tParam < 1)
        {
            _tParam += Time.deltaTime * speed;

            //la position de la forme prend la valeur de la courbe
            trainPosition = Mathf.Pow(1 - _tParam, 3) * p0 +
                              3 * Mathf.Pow(1 - _tParam, 2) * _tParam * p1 +
                              3 * (1 - _tParam) * Mathf.Pow(_tParam, 2) * p2 +
                              Mathf.Pow(_tParam, 3) * p3;


            //Rotation de la forme en fonction de la direction de la courbe
            //création vecteur de déplacement (grâce actuelle et nouvelle position) > création angle > rotation de l'angle en z seulement
            Vector3 dir = new Vector3(trainPosition.x - transform.position.x, trainPosition.y - transform.position.y, 0.0f);
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);


            //déplacement
            transform.position = trainPosition;

            yield return new WaitForEndOfFrame();
        }

        //MAJ des paramètres après le déplacement
        _tParam = 0;

        _coroutineAllowed = true;
    }
}
