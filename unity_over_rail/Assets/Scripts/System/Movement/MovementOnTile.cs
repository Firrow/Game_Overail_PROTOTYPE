using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//CE SCRIPT UTILE LES TUILES POUR PERMETTRE LE DÉPLACEMENT
public class MovementOnTile : MonoBehaviour
{
    private LinkedList<Transform> _road = new LinkedList<Transform>();
    private int _indexRoadToGo;
    //-------------------------
    
    [SerializeField]
    private string _fromDirection;
    private GameObject _currentTile;
    private string _allDirectionsOfATile;
    private int _choice = 1;
    private string _goDirection = "";
    private int _indexDirection;
    private int _NORTH_INVERSION = -1; //a gérer quand il y aura l'input system
    private Transform _nextRoad;

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


    // Start is called before the first frame update
    void Start()
    {
        _indexRoadToGo = 0; //pour test
        _tParam = 0f;
        _coroutineAllowed = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (_coroutineAllowed)
        {
            //StartCoroutine(GoByTheRoute(_indexTileCollection));
            StartCoroutine(GoByTheRoute(_indexRoadToGo)); //pour le test
        }

        //Si l'objet possédant le script possède le tag joueur
        if (this.gameObject.GetComponent<MovementOnTile>().tag == "Player")
        {
            //PLAYER INPUT (à retravailler) --> avant prochain aiguillage
            if (Input.GetKey(KeyCode.Q))
            {
                _choice = 1;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                _choice = -1;
            }
        }

        if (this.gameObject.GetComponent<MovementOnTile>().tag == "Enemy")
        {
            //CODER CHOIX IA ENEMY
            _choice = 1;
        }

    }

    //récupère la tuile sur laquelle le joueur est entrain de naviguer
    private void OnTriggerEnter2D(Collider2D collider)
    {
        //DÉTERMINER LA DIRECTION-----------------------------------------
        //récupération de la tuile actuelle
        if (collider.gameObject.CompareTag("Bullet"))//si la balle entre en collision avec le train
            return;//tu ne fais rien
        //si autre chose entre en contact avec le train (attention, à faire évoluer pour la suite du jeu :
        //penser à créer un prefab pour les tuile, mettre un tag et vérifier que le parent du collider possède le tag "tile"
        _currentTile = collider.transform.parent.gameObject;

        _reversePoints = false;

        _allDirectionsOfATile = PossibleDirections(_currentTile);
        _indexDirection = GetIndexDirection(_allDirectionsOfATile, _fromDirection);
        _goDirection = GetDirection(_indexDirection, _choice, _NORTH_INVERSION, _fromDirection, _allDirectionsOfATile);

        //DÉTERMINER LA BONNE ROUTE-----------------------------------------
        //récupérer la prochaine route en fonction du nom et l'ajoute à la liste
        string nameNextRoad = _fromDirection + _goDirection;
        if (_currentTile.transform.Find(nameNextRoad) == null)
        {
            nameNextRoad = _goDirection + _fromDirection;
            _reversePoints = true;
        }
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
    private string GetDirection(int index, int playerDirection, int INVERSION, string fromD, string allPossibleDirections)
    {
        int i = index + playerDirection * (fromD == "N" ? INVERSION : 1);
        return allPossibleDirections.Substring((i + allPossibleDirections.Length) % allPossibleDirections.Length, 1);
    }



    //DÉPLACEMENTS------------------------------------------------------------------------
    IEnumerator GoByTheRoute(int roadNum)
    {
        _coroutineAllowed = false;
        //récupération des positions des points dans bon sens
        if (_reversePoints == true) //sens inverse
        {
            Debug.Log("Route A inverser !");
            p0 = _road.ElementAt(roadNum).Find("p4").position;
            p1 = _road.ElementAt(roadNum).Find("p3").position;
            p2 = _road.ElementAt(roadNum).Find("p2").position;
            p3 = _road.ElementAt(roadNum).Find("p1").position;
            Debug.Log("Route inversée !");
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

            //déplacement
            transform.position = _objectPosition;
            yield return new WaitForEndOfFrame();
        }

        //MAJ des paramètres après le déplacement
        _tParam = 0;
        speed = speed * 0.90f;
        _indexRoadToGo += 1;

        _coroutineAllowed = true;
    }
}
