using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    [SerializeField]
    private Transform[] _road;

    private int _roadToGo;
    private float _tParam;
    private Vector3 _objectPosition;
    public float speed;
    private bool _coroutineAllowed;

    // Start is called before the first frame update
    void Start()
    {
        _roadToGo = 0;
        _tParam = 0f;
        _coroutineAllowed = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (_coroutineAllowed)
        {
            StartCoroutine(GoByTheRoute(_roadToGo));
        }
    }

    IEnumerator GoByTheRoute(int roadNum)
    {
        _coroutineAllowed = false;

        //récupération des positions des points
        Vector3 p0 = _road[roadNum].GetChild(0).position;
        Vector3 p1 = _road[roadNum].GetChild(1).position;
        Vector3 p2 = _road[roadNum].GetChild(2).position;
        Vector3 p3 = _road[roadNum].GetChild(3).position;

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
        _roadToGo += 1;

        //loop du chemin
        if (_roadToGo > _road.Length - 1)
        {
            _roadToGo = 0;
        }

        _coroutineAllowed = true;

    }
}
