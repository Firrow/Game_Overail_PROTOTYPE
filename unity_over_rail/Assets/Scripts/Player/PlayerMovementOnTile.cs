using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

//CE SCRIPT UTILE LES TUILES POUR PERMETTRE LE DÉPLACEMENT
public class PlayerMovementOnTile : MonoBehaviour
{
    //juste pour les tests (a changer a terme)
    private LinkedList<Transform> _road = new LinkedList<Transform>();
    private int _indexRoadToGo;
    //-------------------------

    [SerializeField]
    private GameObject _tileCollection;
    [SerializeField]
    private string _fromDirection;

    private tileManager _tileManager;
    private GameObject _object; //objet qui contient le script PlayerMovementOnTile.cs
    private List<GameObject> _allTiles;
    private GameObject _currentTile;
    private string _allDirectionsOfATile;
    private int _playerChoice = 1; //direction choisie int (ancien playerDirection)
    private string _goDirection = "";
    private int _indexDirection;
    private int _NORTH_INVERSION = -1; //plus quand il y aura l'input system
    private Transform _nextRoad;
    private Transform _actualRoad;

    //déplacements mathématique
    private int _indexTileCollection; //toujours = à 0
    private float _tParam;
    private Vector3 _objectPosition;
    private bool _coroutineAllowed;
    public float speed;
    public bool loop;
    private bool reversePoints;

    private Vector3 p0;
    private Vector3 p1;
    private Vector3 p2;
    private Vector3 p3;


    // Start is called before the first frame update
    void Start()
    {
        _indexTileCollection = 0;
        _indexRoadToGo = 0; //pour test
        _tParam = 0f;
        _coroutineAllowed = true;

        //récupère toutes les tuiles de la map
        _allTiles = GetAllTiles(_tileCollection); //utile ?
        _object = this.gameObject;

        /*_firstRoad = GameObject.FindGameObjectWithTag("FirstRoad").transform;
        Debug.Log(_firstRoad);*/
        //_road.AddLast(firstRoad);
        //Debug.Log("route : " + _road.ElementAt(0));
    }

    // Update is called once per frame
    void Update()
    {
        if (_coroutineAllowed)
        {
            //StartCoroutine(GoByTheRoute(_indexTileCollection));
            StartCoroutine(GoByTheRoute(_indexRoadToGo)); //pour le test
        }

        //PLAYER INPUT (à retravailler) --> avant prochain aiguillage
        if (Input.GetKey(KeyCode.Q))
        {
            _playerChoice = 1;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            _playerChoice = -1;
        }
    }




    //récupère la liste de toutes les tuiles de la map //utile ?
    public static List<GameObject> GetAllTiles(GameObject Go)
    {
        List<GameObject> listTile = new List<GameObject>();
        for (int i = 0; i < Go.transform.childCount; i++)
        {
            listTile.Add(Go.transform.GetChild(i).gameObject);
        }
        return listTile;
    }

    //récupère la tuile sur laquelle le joueur est entrain de naviguer
    private void OnTriggerEnter2D(Collider2D collider)
    {
        reversePoints = false;
        //DÉTERMINER LA DIRECTION-----------------------------------------
        _currentTile = collider.transform.parent.gameObject;
        Debug.Log("Tuile : " + _currentTile.name);

        _allDirectionsOfATile = PossibleDirections(_currentTile);
        //Debug.Log("Direction possible actuellement : " + _allDirectionsOfATile);


        //Debug.Log("Choix Joueur : " + _playerChoice);
        //Debug.Log("Direction provenance : " + _fromDirection);
        _indexDirection = GetIndexDirection(_allDirectionsOfATile, _fromDirection);
        //Debug.Log("Index direction demandée : " + _indexDirection);
        _goDirection = GetDirection(_indexDirection, _playerChoice, _NORTH_INVERSION, _fromDirection, _allDirectionsOfATile);
        //Debug.Log("Direction demandée : " + _goDirection);

        //DÉTERMINER LA BONNE ROUTE-----------------------------------------
        //récupérer la prochaine route en fonction du nom
        string nameNextRoad = _fromDirection + _goDirection;
        if (_currentTile.transform.Find(nameNextRoad) == null)
        {
            nameNextRoad = _goDirection + _fromDirection;
            reversePoints = true;
        }
        //Debug.Log("Prochaine route théorique : " + nameNextRoad);
        _nextRoad = _currentTile.transform.Find(nameNextRoad); //ou nameNextRoad2 AFAIRE
        //Debug.Log("Prochaine route : " + _nextRoad.name);

        //assigner la prochaine route
        //_road.RemoveAt(0);

        //_road.AddLast(_nextRoad);
        //_road.RemoveFirst();

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
        Debug.Log(_fromDirection);
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
                            //index_provenance  choix_joueur               direction_provenance     toutes_les_directions_tuiles
    private string GetDirection(int index, int playerDirection, int INVERSION, string fromD, string allPossibleDirections)
    {
        //je sais plus où j'en suis aled
        int i = index + playerDirection * (fromD == "N" ? INVERSION : 1);
        //Debug.Log("i = " + i);
        //Debug.Log("taille tableau direction possibles : " + allPossibleDirections.Length);
        //Debug.Log("calcul : " + ((i + allPossibleDirections.Length) % allPossibleDirections.Length));
        return allPossibleDirections.Substring((i + allPossibleDirections.Length) % allPossibleDirections.Length, 1);
    }



    //DÉPLACEMENTS------------------------------------------------------------------------
    IEnumerator GoByTheRoute(int roadNum)
    {
        _coroutineAllowed = false;
        //Debug.Log("numéro route : " + roadNum);
        //Debug.Log("route : " + _road.ElementAt(roadNum));
        //récupération des positions des points dans bon sens
        if (reversePoints == true) //sens inverse
        {
            p0 = _road.ElementAt(roadNum).Find("p4").position;
            p1 = _road.ElementAt(roadNum).Find("p3").position;
            p2 = _road.ElementAt(roadNum).Find("p2").position;
            p3 = _road.ElementAt(roadNum).Find("p1").position;
        }
        else //sens définit dans éditeur
        {
            p0 = _road.ElementAt(roadNum).Find("p1").position;
            p1 = _road.ElementAt(roadNum).Find("p2").position;
            p2 = _road.ElementAt(roadNum).Find("p3").position;
            p3 = _road.ElementAt(roadNum).Find("p4").position;
        }

        //récupération des positions des points dans sens inverse
        /*Vector3 p3 = _road[roadNum].GetChild(0).position;
        Vector3 p2 = _road[roadNum].GetChild(1).position;
        Vector3 p1 = _road[roadNum].GetChild(2).position;
        Vector3 p0 = _road[roadNum].GetChild(3).position;*/

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

        //loop du chemin
        /*if (loop && _indexRoadToGo > _road.Length - 1)
        {
            _indexRoadToGo = 0;
        }*/

        _coroutineAllowed = true;
    }
}
