using System.Collections;
using UnityEngine;


public class HumanTrain : Train
{
    // Flèches (circle actuellement)
    public GameObject cLeft;
    public GameObject cRight;


    void Start()
    {
        base.Start();
        ChangeArrowColor(cLeft.GetComponent<SpriteRenderer>(), cRight.GetComponent<SpriteRenderer>());
        this.choice = 1;
    }


    void Update()
    {
        // base sans inversion
        if (Input.GetKeyDown(KeyCode.Q))
        {
            this.choice = 1;
            ChangeArrowColor(cLeft.GetComponent<SpriteRenderer>(), cRight.GetComponent<SpriteRenderer>());
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            this.choice = -1;
            ChangeArrowColor(cRight.GetComponent<SpriteRenderer>(), cLeft.GetComponent<SpriteRenderer>());
        }
        else if (Input.GetMouseButtonDown(0))
        {
            this.gameObject.GetComponentInChildren<Weapon>().PressTrigger();
        }

        base.Update();
    }


    // Créer une fonction pour le changement de couleur des flèches
    private void ChangeArrowColor(SpriteRenderer actualArrow, SpriteRenderer otherArrow)
    {
        actualArrow.color = new Color(1, 0, 0, 1);
        otherArrow.color = new Color(1, 0, 0, 0);
    }

    public void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
    }
}
