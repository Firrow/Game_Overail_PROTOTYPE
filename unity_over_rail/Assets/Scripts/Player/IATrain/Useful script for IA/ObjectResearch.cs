using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using overail.DataSpawner_;
using System;


namespace overail.IAResearchObject
{
    public class ObjectResearch : MonoBehaviour
    {
        private static GameObject objectTarget;


        public static bool FindObject(IATrain train, string nameObject)
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
                train.UpdateTarget(FindNearestObjectInList(train, objects));
                return true;
            }
            else if (objects.Count == 1)
            {
                train.UpdateTarget(objects[0].SpawnerPosition);
                return true;
                //objectTarget = objects[0].ObjectOnSpawner;
            }
            else
            {
                return false;
            }
        }

        private static Vector3 FindNearestObjectInList(IATrain train, List<DataSpawner> objects)
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

            //objectTarget = tempObject;

            Debug.Log(objectTarget.name);

            return position;
        }

    }
}

