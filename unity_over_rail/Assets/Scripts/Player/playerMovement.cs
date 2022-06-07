using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class playerMovement : MonoBehaviour
{
    [SerializeField]
    private Transform[] _road;

    private int _indexRoadToGo;
    private float _tParam;
    private Vector3 _objectPosition;
    private bool _coroutineAllowed;

    public float speed;
    public bool loop;

    // Start is called before the first frame update
    void Start()
    {
        _indexRoadToGo = 0;
        _tParam = 0f;
        _coroutineAllowed = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (_coroutineAllowed)
        {
            StartCoroutine(GoByTheRoute(_indexRoadToGo));
        }
    }

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
