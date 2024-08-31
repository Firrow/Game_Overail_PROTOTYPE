using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// API
/// All Datas about Spawners on map useful for IA
/// </summary>

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
        // public DataTile CurrentTile // TODO: Convert this Tile into DataTile

        public Tile CurrentTile
        {
            get { return this.train.GetComponent<Train>().CurrentTile.GetComponent<Tile>(); }
        }

        public string FromDirection
        {
            get { return this.train.GetComponent<Train>().fromDirection; }
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
