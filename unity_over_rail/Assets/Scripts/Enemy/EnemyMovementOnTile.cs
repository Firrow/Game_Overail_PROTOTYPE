using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyMovementOnTile : MonoBehaviour
{
    private LinkedList<Transform> _road = new LinkedList<Transform>();
    private int _indexRoadToGo;
    //-------------------------

    [SerializeField]
    public string _fromDirection;
    public int startPoint;
    private GameObject _currentTile;
    private string _allDirectionsOfATile;
    private int _choice = 1; //ne pas mettre de choix par défaut fixe
    private string _goDirection = "";
    private int _indexDirection;
    // private int _NORTH_INVERSION = -1; //a gérer quand il y aura l'input system
    private Transform _nextRoad;


    //Flèches (circle actuellement)
    /*public GameObject cGauche;
    public GameObject cDroit;*/

    //déplacements mathématique
    private float _tParam;
    private Vector3 _objectPosition;
    private bool _coroutineAllowed;
    public float speed;
    private bool _reversePoints;
    private Vector3 p0;
    private Vector3 p1;
    private Vector3 p2;
    private Vector3 p3;

    private List<int> choices = new List<int>() { -1, 1 };


    // Start is called before the first frame update
    void Start()
    {
        _indexRoadToGo = 0; //pour test
        _tParam = 0f;
        _coroutineAllowed = true;

        //CODER CHOIX IA ENEMY
        InvokeRepeating("MovementChoice", 0.3f, 0.3f);

        //ArrowColor(cGauche.GetComponent<SpriteRenderer>(), cDroit.GetComponent<SpriteRenderer>(), 1);
    }
    // Update is called once per frame
    void Update()
    {
        if (_coroutineAllowed)
        {
            //StartCoroutine(GoByTheRoute(_indexTileCollection));
            StartCoroutine(GoByTheRoute(_indexRoadToGo)); //pour le test
        }
    }

    //IA ALÉATOIRE A TESTER
    private int MovementChoice()
    {
        int random = UnityEngine.Random.Range(0, 2);
        return _choice = (random == 0) ? -1 : 1;
    }

    //récupère la tuile sur laquelle le joueur est entrain de naviguer
    private void OnTriggerEnter2D(Collider2D collider)
    {
        //DÉTERMINER LA DIRECTION-----------------------------------------

        //Ne pas prendre en compte les balles qui touchent les tuiles
        if (collider.gameObject.CompareTag("Bullet"))//si la balle entre en collision avec le train
            return;//tu ne fais rien

        //récupération de la tuile actuelle
        _currentTile = collider.transform.parent.gameObject;

        _reversePoints = false;

        //Debug.Log("TUILE actuelle : " + _currentTile.name);
        _allDirectionsOfATile = PossibleDirections(_currentTile);
        //Debug.Log("Directions possibles : " + _allDirectionsOfATile);
        _indexDirection = GetIndexDirection(_allDirectionsOfATile, _fromDirection);
        //Debug.Log("ORIGINE : " + _indexDirection);
        _goDirection = GetDirection(_indexDirection, _choice, /*_NORTH_INVERSION,*/ _fromDirection, _allDirectionsOfATile);
        //Debug.Log("Prochaine DIRECTION : " + _goDirection);
        //Debug.Log("--------------------------------------------------------------------------------");

        //DÉTERMINER LA BONNE ROUTE-----------------------------------------
        //récupérer la prochaine route en fonction du nom et l'ajoute à la liste
        string nameNextRoad = _fromDirection + _goDirection;
        if (_currentTile.transform.Find(nameNextRoad) == null)
        {
            nameNextRoad = _goDirection + _fromDirection;
            _reversePoints = true;
        }

        //Debug.Log("Nom prochaine ROUTE : " + nameNextRoad);

        _nextRoad = _currentTile.transform.Find(nameNextRoad);
        _road.AddLast(_nextRoad);

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
        return actualTile.GetComponent<tileManager>().directionOfTile;
    }

    //calcul index de la direction de provenance dans la liste des directions de la tuile
    private int GetIndexDirection(string allDirections, string previousDirection)
    {
        return allDirections.IndexOf(previousDirection);
    }

    //calcul index puis prochaine direction
    private string GetDirection(int index, int playerDirection, /*int INVERSION,*/ string fromD, string allPossibleDirections)
    {
        //_choice (playerDirection) ne marche pas tout le temps ?
        //VERSION AVEC 2 TOUCHES
        int i = index + playerDirection; //* (fromD == "N" ? INVERSION : 1); --> Origine du bug de direction 
        return allPossibleDirections.Substring((i + allPossibleDirections.Length) % allPossibleDirections.Length, 1);
    }

    //Créer une fonction pour le changement de couleur des flèches
    private void ArrowColor(SpriteRenderer actualArrow, SpriteRenderer otherArrow, int choice)
    {
        actualArrow.color = new Color(1, 0, 0, 1);
        otherArrow.color = new Color(1, 0, 0, 0);
    }



    //DÉPLACEMENTS------------------------------------------------------------------------
    IEnumerator GoByTheRoute(int roadNum)
    {
        _coroutineAllowed = false;
        //récupération des positions des points dans bon sens
        if (_reversePoints == true) //sens inverse
        {
            //Debug.Log("Route A inverser !");
            p0 = _road.ElementAt(roadNum).Find("p4").position;
            p1 = _road.ElementAt(roadNum).Find("p3").position;
            p2 = _road.ElementAt(roadNum).Find("p2").position;
            p3 = _road.ElementAt(roadNum).Find("p1").position;
            //Debug.Log("Route inversée !");
        }
        else //sens définit dans éditeur
        {
            p0 = _road.ElementAt(roadNum).Find("p1").position;
            p1 = _road.ElementAt(roadNum).Find("p2").position;
            p2 = _road.ElementAt(roadNum).Find("p3").position;
            p3 = _road.ElementAt(roadNum).Find("p4").position;
        }

        while (_tParam < 1)
        {
            _tParam += Time.deltaTime * speed;

            //la position de la forme prend la valeur de la courbe
            _objectPosition = Mathf.Pow(1 - _tParam, 3) * p0 +
                              3 * Mathf.Pow(1 - _tParam, 2) * _tParam * p1 +
                              3 * (1 - _tParam) * Mathf.Pow(_tParam, 2) * p2 +
                              Mathf.Pow(_tParam, 3) * p3;


            //Rotation de la forme en fonction de la direction de la courbe
            //création vecteur de déplacement (grâce actuelle et nouvelle position) > création angle > rotation de l'angle en z seulement
            Vector3 dir = new Vector3(_objectPosition.x - transform.position.x, _objectPosition.y - transform.position.y, 0.0f);
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);


            //déplacement
            transform.position = _objectPosition;

            yield return new WaitForEndOfFrame();
        }

        //MAJ des paramètres après le déplacement
        _tParam = 0;
        _indexRoadToGo += 1;

        _coroutineAllowed = true;
    }
}
