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

    //OLD
    /*private Vector2 movementInput; //test
    [SerializeField]
    private InputActionReference movement, shoot; //, pointerPosition;*/

    //NEW
    private InputActionAsset inputAsset;
    private InputActionMap playerActionMap;
    private InputActionReference movement, shoot;
    private float movementInput;

    private int lastChoice;


    private void Awake()
    {
        inputAsset = this.GetComponent<PlayerInput>().actions;
        playerActionMap = inputAsset.FindActionMap("PlayerInput");
        //shoot.action.performed += _ => this.gameObject.GetComponentInChildren<Weapon>().PressShootButton();
    }

    void Start()
    {
        base.Start();
        ChangeArrowColor(leftArrow.GetComponent<SpriteRenderer>(), rightArrow.GetComponent<SpriteRenderer>());
        this.choice = 1;
        lastChoice = 1;
    }

    private void OnEnable()
    {
        playerActionMap.FindAction("Move").started += playerChoiceDirection;
        //playerActionMap.FindAction("Attack").started += 
    }


    void Update()
    {
        base.Update();
    }

    private void playerChoiceDirection(InputAction.CallbackContext obj)
    {
        movementInput = obj.ReadValue<Vector2>().x;

        if (movementInput == 0)
        {
            this.choice = lastChoice;
        }
        else if (movementInput == 1) //Input.GetKeyDown(KeyCode.Q)
        {
            this.choice = 1;
            ChangeArrowColor(leftArrow.GetComponent<SpriteRenderer>(), rightArrow.GetComponent<SpriteRenderer>());
        }
        else if (movementInput == -1) //Input.GetKeyDown(KeyCode.D)
        {
            this.choice = -1;
            ChangeArrowColor(rightArrow.GetComponent<SpriteRenderer>(), leftArrow.GetComponent<SpriteRenderer>());
        }
        lastChoice = this.choice;
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
