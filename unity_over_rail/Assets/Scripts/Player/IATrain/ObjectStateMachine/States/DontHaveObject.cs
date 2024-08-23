using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using overail.DataSpawner;
using System;
using Random = UnityEngine.Random;
using overail.IAPath;

public class DontHaveObject : IStateObject
{
    private IATrain train;
    private int BULLET_LIMIT_DIVIDED = 3;
    private Vector3 positionToTarget;
    private GameObject objectTarget;

    private int chanceToWantToGetObject = 6; // niveau "normal"
    private string objectToGet = "";

    private TimeSpan SEARCH_RATE = new TimeSpan(0, 0, 0, 4, 0);
    private DateTime lastTimeSearch = DateTime.Now;


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
            objectToGet = "";
            train.ChangeState2(new HaveObject(train));
        }
    }








    private void FindObjectThatIANeed()
    {
        if (train.CurrentState1 is Defense)
        {
            objectToGet = "HeartObject";
            //Debug.Log("coeur position : " + positionToTarget);
        }
        else if (train.myData.BulletQuantity <= (train.GetComponentInChildren<Weapon>().MaxBulletQuantity / BULLET_LIMIT_DIVIDED))
        {
            objectToGet = "BulletObject";
            //Debug.Log("balle position : " + positionToTarget);
        }
        else // Avoir une certaine chance d'aller chercher un objet au hasard (?)
        {
            // Cherche un objet aléatoire tous les x temps aléatoire aussi --> TROUVER MIEUX
            /*if (objectToGet == "" && (DateTime.Now - lastTimeSearch >= SEARCH_RATE))
            {
                lastTimeSearch = DateTime.Now;
                if (Random.Range(0, chanceToWantToGetObject) == 0)
                {
                    int indexElement = Random.Range(0, train.GameManager.allObjectNames.Count);
                    objectToGet = train.GameManager.allObjectNames[indexElement];
                    Debug.Log(objectToGet);
                }
            }*/
        }

        FindObject(objectToGet);
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
            //positionToTarget = FindNearestObjectInList(objects);

            train.UpdateTarget(FindNearestObjectInList(objects));
        }
        else if (objects.Count == 1)
        {
            //positionToTarget = objects[0].SpawnerPosition;
            train.UpdateTarget(objects[0].SpawnerPosition);

            objectTarget = objects[0].ObjectOnSpawner;
        }
    }

    private Vector3 FindNearestObjectInList(List<DataSpawner> objects)
    {
        float distance;
        float tempDistance = 1000000000;
        GameObject tempObject = null;
        Vector3 position = new Vector3(0, 0, 0);

        foreach (var item in objects)
        {
            distance = Vector3.Distance(train.TrainPosition, item.SpawnerPosition);
            if (distance < tempDistance)
            {
                tempDistance = distance;
                tempObject = item.ObjectOnSpawner;
                position = item.SpawnerPosition;
            }
        }

        objectTarget = tempObject;
        return position;
    }




    //------------------------------------------------------------------------------------------------------------------



    public int ChanceToWantToGetObject
    {
        get { return chanceToWantToGetObject; }
        set { chanceToWantToGetObject = value; }
    }
}