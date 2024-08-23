using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace overail.DataTain_
{
    public class DataTrain
    {
        public GameObject train;
        public int index;



        public DataTrain(GameObject train, int index)
        {
            this.train = train;
            this.index = index;
        }




        public Vector2 Position
        {
            get { return this.train.GetComponent<Train>().TrainPosition; }
        }

        public bool ShieldIsActivate
        {
            get { return this.train.GetComponent<Train>().ShieldIsActivate; }
        }

        public GameObject CurrentObject
        {
            get { return this.train.GetComponent<Train>().CurrentItem; }
        }

        public float Speed
        {
            get { return this.train.GetComponent<Train>().Velocity; }
        }
        public int Health
        {
            get { return this.train.GetComponent<Train>().CurrentHealth; }
        }
        public int BulletQuantity
        {
            get { return this.train.GetComponentInChildren<Weapon>().CurrentBulletQuantity; }
        }

    }
}
