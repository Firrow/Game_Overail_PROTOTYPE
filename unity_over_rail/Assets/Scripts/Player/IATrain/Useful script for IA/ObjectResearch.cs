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
        /// <summary>
        /// Get all objets in map
        /// </summary>
        /// <param name="train"></param>
        /// <param name="nameObject"></param>
        /// <returns>list of all DataSpawner which contains an object in map</returns>
        public static List<DataSpawner> ListObjectsInMap(IATrain train, string nameObject)
        {
            List<DataSpawner> objects = new List<DataSpawner>();

            foreach (var spawner in GameObject.FindGameObjectWithTag("GameManager").GetComponent<DataContainer>().DataNetworkMap.DataSpawners) 
            {
                if (spawner.ObjectOnSpawnerName == nameObject)
                {
                    objects.Add(spawner);
                }
            }

            return objects;
        }

        /// <summary>
        /// Use the object the train is looking for and try to find if there is an instance of it on map
        /// </summary>
        /// <param name="objects"></param>
        /// <param name="train"></param>
        /// <returns>true or false</returns>
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

        /// <summary>
        /// Take the list of DataSpawners and find the object closest to the train
        /// </summary>
        /// <param name="train"></param>
        /// <param name="objects"></param>
        /// <returns></returns>
        private static DataSpawner FindNearestObjectInList(IATrain train, List<DataSpawner> objects)
        {
            float distance;
            float tempDistance = 1000000000;
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

