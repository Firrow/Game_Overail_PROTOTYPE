using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace overail.DataTain
{
    public class DataTrain
    {
        public GameObject train;
        public int index;
        public Vector2 position;
        public bool shieldIsActivate;
        public GameObject currentObject;
        public float speed;
        public int health;
        public int bulletQuantity;



        public DataTrain(GameObject train, int index, Vector2 position, bool shieldIsActivate, GameObject currentObject, float speed, int health, int bulletQuantity)
        {
            this.train = train;
            this.index = index;
            this.position = position;
            this.shieldIsActivate = shieldIsActivate;
            this.currentObject = currentObject;
            this.speed = speed;
            this.health = health;
            this.bulletQuantity = bulletQuantity;
        }
    }
}
