using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;


public class IATrain : Train
{
    //Flèches (circle actuellement)
    /*public GameObject cGauche;
    public GameObject cDroit;*/


    void Start()
    {
        base.Start();
        this.choice = 1;

        //CODER CHOIX IA ENEMY
        InvokeRepeating("MovementChoice", 0.3f, 0.3f);

        //ArrowColor(cGauche.GetComponent<SpriteRenderer>(), cDroit.GetComponent<SpriteRenderer>(), 1);
    }

    void Update()
    {
        //MovementChoice();
        base.Update();
    }

    //IA ALÉATOIRE A TESTER
    private void MovementChoice()
    {
        int random = Random.Range(0, 2);
        this.choice = (random == 0) ? -1 : 1;
    }
}
