using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using overail.DataTrain_;
using overail.DataTile_;
using overail.DataSpawner_;
using overail.DataMap_;
using overail.IAPathResearch_;
using overail.DataContainer_;
using overail.TrainActions_;
using System;

/// <summary>
/// Script with all function for IA players only.
/// </summary>

public class IATrain : Train
{
    private int DEPTH_THRESHOLD = 7;

    public ITargetToMove targetToMove = null;
    public DataTrain myData;

    private IStateTrain currentState;
    private float trainAngle;
    private Vector3 targetPosition;
    private bool targetChanged = false;
    private bool enterOnSwitch = false;



    void Start()
    {
        base.Start();

        myData = dataContainer.GetTheTrain(PlayerIndex);
        CurrentState = new Attack(this);
    }

    private void FixedUpdate()
    {
        base.FixedUpdate();

        CurrentState.MainExecution();
        NeedToChangeDirectionToTarget();
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
        Debug.Log(targetPosition);
    }

    public void NeedToChangeDirectionToTarget()
    {
        if (targetChanged || enterOnSwitch)
        {
            //DANS PATH : 
            // recalculer la prochaine direction à prendre
            // récupérer le prochain aiguillage + ajouter sécurité au cas ou il n'y a pas d'aiguillage

            DataContainer.DirectionChoice directionChoice = GetNextChoiceDirection();

            switch (directionChoice)
            {
                //TODO : remplacer les valeurs brutes par les valeurs dans l'enum créer (voir TODO du Train.cs ligne 101)
                case DataContainer.DirectionChoice.LEFT:
                    TrainActions.APIChangeDirection(myData.Index, -1);
                    break;
                case DataContainer.DirectionChoice.RIGHT:
                    TrainActions.APIChangeDirection(myData.Index, 1);
                    break;
                case DataContainer.DirectionChoice.RANDOM:
                    TrainActions.APIChangeDirection(myData.Index, (int)Math.Pow(-1, UnityEngine.Random.Range(1, 3)));
                    break;
                default:
                    break;
            }

            // remet les flags à false
            targetChanged = false;
            enterOnSwitch = false;


            //TODO : Pour l'entrée du réseau, faire un choix de direction au hasard
        }
    }

    /// <summary>
    /// Determine the direction choice to do for IA to get the target
    /// </summary>
    /// <returns></returns>
    public DataContainer.DirectionChoice GetNextChoiceDirection()
    {
        // If the target is on the same road than the train
        if (dataContainer.DataNetworkMap.ThereIsTargetOnRoad(myData.CurrentTile, FromDirection, targetToMove))
        {
            return DataContainer.DirectionChoice.NO_DIRECTION;
        }

        KeyValuePair<string, DataTile> nextSwitch = dataContainer.DataNetworkMap.GetNextSwitchOnMap(myData.CurrentTile, FromDirection);
        Dictionary<string, DataTile> nextTilesOfSwitch = dataContainer.DataNetworkMap.GetNextTiles(nextSwitch.Value, FromDirection);

        //TODO : Do an aggregate with random for default value and conditional (ternary) operator for return
        DataContainer.DirectionChoice choice = DataContainer.DirectionChoice.RANDOM;
        int? minDepth = null;

        foreach (var nextTile in nextTilesOfSwitch)
        {
            int? currentDepth = IsTargetHere(nextTile.Value, nextTile.Key);
            if ((minDepth is null && currentDepth is not null) || currentDepth < minDepth)
            {
                minDepth = currentDepth;
                choice = dataContainer.DataNetworkMap.WhichChoiceIsNextDirection(nextSwitch.Value, nextSwitch.Key, DataContainer.OppositeDirections[nextTile.Key]);
            }
        }

        return choice;
        //--------
    }

    /// <summary>
    /// Check if the target is reachable from this tile in a minimum number of switch
    /// </summary>
    /// <param name="currentTile"></param>
    /// <param name="fromDirection"></param>
    /// <param name="depth"></param>
    /// <returns></returns>
    private int? IsTargetHere(DataTile tile, string fromDirection, int depth = 1)
    {
        if (depth > DEPTH_THRESHOLD)
        {
            return null; // no target was found
        }
        else if (dataContainer.DataNetworkMap.ThereIsTargetOnRoad(tile, fromDirection, targetToMove))
        {
            return depth; // the target was found
        }
        else
        {
            var nextSwitch = dataContainer.DataNetworkMap.GetNextSwitchOnMap(tile, fromDirection);

            Dictionary<string, DataTile> nextTilesOfSwitch = dataContainer.DataNetworkMap.GetNextTiles(nextSwitch.Value, nextSwitch.Key);

            return nextTilesOfSwitch.Aggregate<KeyValuePair<string, DataTile>, int?>(null, (minDepth, nextTile) => {
                if (minDepth == 1)
                {
                    return minDepth;
                }

                int? nextTileDepth = IsTargetHere(nextTile.Value, nextTile.Key, ++depth);

                return nextTileDepth < minDepth ? nextTileDepth : minDepth ?? nextTileDepth; //TODO : voir screen conversation Ulysse pour faire doc sur Notion (null coalescing operator)
            });
        }
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

