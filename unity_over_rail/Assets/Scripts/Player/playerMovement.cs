using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

//CE SCRIPT PREND DIRECTEMENT LES ROUTES POUR PERMETTRE LE DÉPLACEMENT

public class playerMovement : MonoBehaviour
{
    [SerializeField]
    private Transform[] _road;

    //déplacements mathématique
    private int _indexRoadToGo;
    private float _tParam;
    private Vector3 _objectPosition;
    private bool _coroutineAllowed;
    public float speed;
    public bool loop;

    //orientation
    private int _NORTH_INVERSION = -1;
    private tileManager _tileCreator;
    private int _actualTileX;
    private int _actualTileY;
    private char _playerInput;
    private string _fromDirection;
    private int _playerDirection = 1; //direction choisie int
    private int _goIndex;
    private string _goDirection = "";

    private GameObject parentTile;


    // Start is called before the first frame update
    void Start()
    {
        _indexRoadToGo = 0;
        _tParam = 0f;
        _coroutineAllowed = true;

        //départ train
        //_actualTileX = 1; //idX Tuile
        //_actualTileY = 1; //idY Tuile
        //script Ulysse : donne position tuile départ
        //script Manon : donne route de départ
        _fromDirection = "O";
    }

    // Update is called once per frame
    void Update()
    {
        if (_coroutineAllowed)
        {
            StartCoroutine(GoByTheRoute(_indexRoadToGo));
        }
        //PLAYER INPUT (à retravailler) --> avant prochain aiguillage
        /*if (Input.GetKeyDown(KeyCode.Q))
            _playerDirection = 1;
        else if (Input.GetKeyDown(KeyCode.D))
            _playerDirection = -1;
        else
            print("erreur mauvaise touche");*/


        //TRAITEMENT CHOIX DU JOUEUR---------------------------------------------------------
        //1) mettre à jour l'id de la tuile actuelle
        parentTile = _road[0].transform.parent.gameObject; //récupération de la tuile parent de la route;
        /*_actualTileX = */

    }










    
    //DÉPLACEMENTS------------------------------------------------------------------------
    IEnumerator GoByTheRoute(int roadNum)
    {
        _coroutineAllowed = false;

        //récupération des positions des points dans bon sens
        Vector3 p0 = _road[roadNum].GetChild(0).position;
        Vector3 p1 = _road[roadNum].GetChild(1).position;
        Vector3 p2 = _road[roadNum].GetChild(2).position;
        Vector3 p3 = _road[roadNum].GetChild(3).position;

        //récupération des positions des points dans sens inverse
        /*Vector3 p3 = _road[roadNum].GetChild(0).position;
        Vector3 p2 = _road[roadNum].GetChild(1).position;
        Vector3 p1 = _road[roadNum].GetChild(2).position;
        Vector3 p0 = _road[roadNum].GetChild(3).position;*/

        while (_tParam < 1)
        {
            _tParam += Time.deltaTime * speed ;

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
        if (loop && (_indexRoadToGo > _road.Length - 1))
        {
            _indexRoadToGo = 0;
        }

        _coroutineAllowed = true;

    }
}
