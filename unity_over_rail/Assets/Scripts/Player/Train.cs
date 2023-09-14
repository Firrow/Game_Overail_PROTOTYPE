using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Train : MonoBehaviour
{
    //déplacements mathématique
    private float _tParam;
    private Vector3 trainPosition;
    private bool _coroutineAllowed;
    private bool _reversePoints;
    private Vector3 p0;
    private Vector3 p1;
    private Vector3 p2;
    private Vector3 p3;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //DÉPLACEMENTS------------------------------------------------------------------------
    /*IEnumerator GoByTheRoute()
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
    }*/
}
