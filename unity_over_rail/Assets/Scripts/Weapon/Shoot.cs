using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    /*A CODER :
        - Diriger arme
        - Tirer
     */
    // Start is called before the first frame update

    public Transform Firepoint;
    public GameObject bullet;
    //penser à mettre le InputPlayer

    private float bulletSpeed;

    void Awake()
    {
        bulletSpeed = 8f;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //A DÉCOMMENTER QUAND MENU PAUSE CRÉER : Si le jeu est en pause, on ne permet pas au joueur de tirer
        /*if (PauseMenu.gameIsPaused)
        {
            return;
        }*/

        //prend position objet
        Vector2 positionOnScreen = Camera.main.WorldToViewportPoint(transform.position);
        //prend position souris
        Vector2 mouseOnScreen = (Vector2)Camera.main.ScreenToViewportPoint(Input.mousePosition);

        float angle = AngleBetweenTwoPoints(positionOnScreen, mouseOnScreen);
        //applique la rotation
        transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
    }

    //calcul de l'angle entre les 2 points
    float AngleBetweenTwoPoints(Vector3 a, Vector3 b)
    {
        return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
    }

}
