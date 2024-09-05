using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.ComponentModel;
using System.Runtime.CompilerServices;

/// <summary>
/// API
/// All Datas about Spawners on map useful for IA
/// </summary>

namespace overail.DataTrain_
{
    public class DataTrain : INotifyPropertyChanged
    {
        public GameObject train;
        public Weapon trainWeapon;
        public event PropertyChangedEventHandler PropertyChanged;



        public DataTrain(GameObject train)
        {
            this.train = train;
            RegisterTrain(train.GetComponent<Train>());
        }



        public void RegisterTrain(Train train)
        {
            train.PropertyChanged += (sender, e) =>
            {
                //Debug.Log("Index : " + train.PlayerIndex + " : abonnement DataTrain - PROPERTY : " + e.PropertyName);
                if (e.PropertyName == nameof(train.CurrentHealth))
                {
                    OnPropertyChanged(nameof(Health));
                }
                else if (e.PropertyName == nameof(train.CurrentItem))
                {
                    OnPropertyChanged(nameof(CurrentObject));
                }
            };
            train.Weapon.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == nameof(train.Weapon.CurrentBulletQuantity))
                {
                    OnPropertyChanged(nameof(train.Weapon.CurrentBulletQuantity));
                }
            };
        }

        public void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
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
