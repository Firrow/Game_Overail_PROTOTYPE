using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// API
/// All Datas about Spawners on map useful for IA
/// </summary>

namespace overail.DataTrain_
{
    public class DataTrain //: ISubject
    {
        public GameObject train;
        public Weapon trainWeapon;



        public DataTrain(GameObject train)
        {
            this.train = train;
            RegisterTrain(train.GetComponent<Train>());
            //trainWeapon = train.GetComponent<Train>().weapon.GetComponent<Weapon>();
        }



        public static void RegisterTrain(Train train)
        {
            train.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == nameof(train.CurrentHealth))
                {
                    Debug.Log("DATATRAIN santé");
                    //train.GetComponent<Train>().healthBar.GetComponent<HealthBar>().SetHealth(train.CurrentHealth);
                }
                else if (e.PropertyName == nameof(train.CurrentItem))
                {
                    Debug.Log("DATATRAIN object");
                }
            };
            train.Weapon.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == nameof(train.Weapon.CurrentBulletQuantity))
                {
                    Debug.Log("DATATRAIN bullet");
                }
            };
        }


        public int Index
        {
            get { return this.train.GetComponent<Train>().PlayerIndex; }
        }

        public Vector2 Position
        {
            get { return this.train.GetComponent<Train>().TrainPosition; }
        }

        public Tile CurrentTile // public DataTile CurrentTile // TODO: Convert this Tile into DataTile
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

        public GameObject CurrentObject
        {
            get { return this.train.GetComponent<Train>().CurrentItem; }
        }
    }
}
