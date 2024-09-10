using System.Collections;
using UnityEngine;
using overail.DataTrain_;
using overail.DataTile_;
using overail.DataSpawner_;
using overail.DataMap_;
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
    private DataContainer dataContainer;

    public DataTile nextSwitch;

    void Start()
    {
        base.Start();

        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        dataContainer = gameManager.GetComponent<DataContainer>();
        myData = dataContainer.GetTheTrain(PlayerIndex);
        CurrentState = new Attack(this);
    }

    void Update()
    {
        base.Update();
    }

    private void FixedUpdate()
    {
        base.FixedUpdate();

        CurrentState.MainExecution();
        NeedToChangeDirectionToTarget();

        Debug.Log("current tile DataTile : " + myData.CurrentTile.Tile);
        nextSwitch = dataContainer.DataNetworkMap.GetNextSwitchOnMap(myData.CurrentTile, fromDirection);
        Debug.Log("next switch : " + nextSwitch.Tile);
        Debug.Log("------------------------------------------------------------------------------");
    }



    public void ChangeState(IStateTrain newState)
    {
        CurrentState = newState;
    }

    public void UpdateTarget(ITargetToMove target)
    {
        targetChanged = true;
        targetToMove = target;
        targetPosition = target.Position;
        //Debug.Log("CHANGE CIBLE");
        //Debug.Log(targetPosition);
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

    public IStateTrain CurrentState
    {
        get { return currentState; }
        set { currentState = value; }
    }

    public Vector3 TargetPosition
    {
        get { return targetPosition; }
        set { targetPosition = value; }
    }
}

