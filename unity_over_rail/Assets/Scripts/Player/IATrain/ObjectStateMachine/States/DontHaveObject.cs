using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using overail.DataSpawner;
using System;

public class DontHaveObject : IStateObject
{
    private IATrain train;
    private Vector3 positionToTarget;
    private GameObject objectTarget;

    
    public DontHaveObject(IATrain IATrain)
    {
        train = IATrain;
    }


    public void MainExecution()
    {
        UpdateState();

        FindObjectThatIANeed();
    }

    public void UpdateState()
    {
        if (train.myData.CurrentObject)
        {
            train.ChangeState2(new HaveObject(train));
        }
    }

    private void FindObjectThatIANeed()
    {
        if (train.CurrentState1 is Defense)
        {
            FindObject("HeartObject"); // Trouve la position de l'objet demandé le plus proche
        }
        // TODO : faire avec les autres situations
        // IDÉE : Ajouter un type aux objets (défense ou attaque) et chercher un objet de ce type en fonction état de l'IA
    }

    private void FindObject(string nameObject)
    {
        List<DataSpawner> objects = new List<DataSpawner>();

        foreach (var spawner in train.GetComponent<DataContainer>().DataSpawners)
        {
            if (spawner.ObjectOnSpawnerName == nameObject)
            {
                objects.Add(spawner);
            }
        }

        if (objects.Count > 1)
        {
            positionToTarget = FindNearestObjectInList(objects);
            //Debug.Log(positionToTarget);
            //Debug.Log(objectTarget.name);
        }
        else if (objects.Count == 1)
        {
            positionToTarget = objects[0].SpawnerPosition;
            objectTarget = objects[0].ObjectOnSpawner;
            //Debug.Log(positionToTarget);
            //Debug.Log(objectTarget.name);
        }
    }

    private Vector3 FindNearestObjectInList(List<DataSpawner> objects)
    {
        float distance;
        float tempDistance = 1000000000;
        GameObject tempObject = null;
        Vector3 position = new Vector3(0, 0, 0);
        //int i = 0;

        foreach (var item in objects)
        {
            distance = Vector3.Distance(train.TrainPosition, item.SpawnerPosition);
            // Debug.Log("Item " + i + " position : " + item.SpawnerPosition);
            // Debug.Log("Item " + i + " distance : " + distance);
            if (distance < tempDistance)
            {
                tempDistance = distance;
                tempObject = item.ObjectOnSpawner;
                position = item.SpawnerPosition;
            }
            //i++;
        }

        objectTarget = tempObject;
        return position;
    }
}