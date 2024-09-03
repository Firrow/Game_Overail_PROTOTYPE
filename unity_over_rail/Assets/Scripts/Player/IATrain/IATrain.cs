using System.Collections;
using UnityEngine;
using overail.DataTrain_;
using overail.DataTile_;
using overail.DataSpawner_;
using overail.IAPathResearch_;
using overail.DataContainer_;

/// <summary>
/// Script with all function for IA players only.
/// </summary>

public class IATrain : Train
{
    public ITargetToMove targetToMove = null;
    public DataTrain myData;

    private IStateTrain currentState;
    private int lastChoice;
    private float trainAngle;
    private Vector3 targetPosition;
    private bool targetChanged = false;
    private bool enterOnSwitch = false;
    private GameManager gameManager;



    void Start()
    {
        base.Start();

        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        myData = GameObject.FindGameObjectWithTag("TEMPDataContainer").GetComponent<DataContainer>().GetTheTrain(PlayerIndex); //ligne temporaire car DataContainer pas static
        //TODO: quand DataContainer static, utiliser le using DataContainer pour appeler la fonction
        currentState = new Attack(this);
    }

    void Update()
    {
        base.Update();
    }

    private void FixedUpdate()
    {
        base.FixedUpdate();

        currentState.MainExecution();
        NeedToChangeDirectionToTarget();

        GetNextSwitchOnMap();
    }



    public void ChangeState(IStateTrain newState)
    {
        currentState = newState;
    }

    public void UpdateTarget(ITargetToMove target)
    {
        targetChanged = true;
        targetToMove = target;
        targetPosition = target.Position;
        Debug.Log("CHANGE CIBLE");
        Debug.Log(targetPosition);
    }

    public void NeedToChangeDirectionToTarget()
    {
        if (targetChanged || enterOnSwitch)
        {
            //DANS PATH : 
            // recalculer la prochaine direction à prendre
            // récupérer le prochain aiguillage + ajouter sécurité au cas ou il n'y a pas d'aiguillage

            //PathResearch.GetNextDirection(this); //return numéro de la direction

            // remet les flags à false
            targetChanged = false;
            enterOnSwitch = false;
        }
    }

    public static void GetNextDirection()
    {
        Debug.Log("NEXTDIRECTION");
        //GetNextDirectionRecursive(train);
    }

    private void GetNextDirectionRecursive()
    {

    }

    private void GetNextSwitchOnMap()
    {
        GridLayout grid = GameObject.FindObjectOfType<GridLayout>();
        Vector3 tmp = grid.CellToWorld(grid.WorldToCell(this.myData.Position));
        Debug.Log("Positions : " + tmp);
        /*Tile currentTile = this.myData.CurrentTile;
        Tile nextTile;
        string fromDirectionTrain = this.myData.FromDirection;

        if (currentTile.isSwitch) //utile ?
        {
            return currentTile;
        }
        else
        {
            currentTile
        }*/

    }

    public override void OnSwitchEnter()
    {
        enterOnSwitch = true;
    }



    public int PlayerIndex
    {
        get { return playerIndex; }
    }

    public GameManager GameManager
    {
        get { return gameManager; }
    }

    public IStateTrain CurrentState1
    {
        get { return currentState; }
    }

    public Vector3 TargetPosition
    {
        get { return targetPosition; }
        set { targetPosition = value; }
    }
}

