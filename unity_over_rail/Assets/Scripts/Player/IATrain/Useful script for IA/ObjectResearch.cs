using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using overail.DataSpawner_;
using overail.DataContainer_;
using System;

/// <summary>
/// Functions for researching objects in map and find the nearest object to IA
/// </summary>

namespace overail.IAResearchObject_
{
    public class ObjectResearch : MonoBehaviour
    {
        public static List<DataSpawner> ListObjectsInMap(IATrain train, string nameObject)
        {
            List<DataSpawner> objects = new List<DataSpawner>();

            foreach (var spawner in GameObject.FindGameObjectWithTag("TEMPDataContainer").GetComponent<DataContainer>().DataSpawners) //train.GetComponent<DataContainer>().DataSpawners //TODO: quand DataContainer static, utiliser le using DataContainer pour appeler la fonction
            {
                if (spawner.ObjectOnSpawnerName == nameObject)
                {
                    objects.Add(spawner);
                }
            }

            return objects;
        }

        public static bool FindTargetInObjects(List<DataSpawner> objects, IATrain train)
        {
            if (objects.Count > 1)
            {
                train.UpdateTarget(FindNearestObjectInList(train, objects));
                return true;
            }
            else if (objects.Count == 1)
            {
                train.UpdateTarget(objects[0]);
                return true;
            }
            else
            {
                return false;
            }
        }

        private static DataSpawner FindNearestObjectInList(IATrain train, List<DataSpawner> objects)
        {
            float distance;
            float tempDistance = 1000000000;
            GameObject tempObject = null;
            DataSpawner tempspawner = null;
            Vector3 position = new Vector3(0, 0, 0);

            foreach (var item in objects)
            {
                distance = Vector3.Distance(train.TrainPosition, item.Position);
                if (distance < tempDistance)
                {
                    tempDistance = distance;
                    tempspawner = item;
                    position = item.Position;
                }
            }

            return tempspawner;
        }
    }
}

