using UnityEngine;
using UnityEngine.InputSystem;


public class HumanTrain : Train
{
    // Flèches (circle actuellement)
    public GameObject leftArrow;
    public GameObject rightArrow;

    //Controle gauche
    //Controle droit
    //Controle tir
    //sprite image

    private Vector2 movementInput; //test
    [SerializeField]
    private InputActionReference movement, shoot, pointerPosition;
    private int lastChoice;


    void Start()
    {
        base.Start();
        ChangeArrowColor(leftArrow.GetComponent<SpriteRenderer>(), rightArrow.GetComponent<SpriteRenderer>());
        this.choice = 1;
        lastChoice = 1;
    }


    void Update()
    {
        movementInput = movement.action.ReadValue<Vector2>();

        // base sans inversion
        if (movementInput == Vector2.zero)
        {
            this.choice = lastChoice;
        }
        else if (movementInput.x < 0) //Input.GetKeyDown(KeyCode.Q)
        {
            this.choice = 1;
            ChangeArrowColor(leftArrow.GetComponent<SpriteRenderer>(), rightArrow.GetComponent<SpriteRenderer>());
        }
        else if (movementInput.x > 0) //Input.GetKeyDown(KeyCode.D)
        {
            this.choice = -1;
            ChangeArrowColor(rightArrow.GetComponent<SpriteRenderer>(), leftArrow.GetComponent<SpriteRenderer>());
        }
        else if (Input.GetMouseButtonDown(0))
        {
            this.gameObject.GetComponentInChildren<Weapon>().PressShootButton();
        }

        lastChoice = this.choice;
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
