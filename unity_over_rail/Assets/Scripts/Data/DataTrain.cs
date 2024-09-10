using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using overail.DataTile_;
using overail.DataContainer_;

/// <summary>
/// API
/// All Datas about Spawners on map useful for IA
/// </summary>

namespace overail.DataTrain_
{
    public class DataTrain : INotifyPropertyChanged
    {
        public GameObject train;
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
                if (e.PropertyName == nameof(train.CurrentHealth))
                {
                    OnPropertyChanged(nameof(Health));
                }
                else if (e.PropertyName == nameof(train.CurrentItem))
                {
                    OnPropertyChanged(nameof(CurrentObject));
                }
                else if (e.PropertyName == nameof(train.IsDead))
                {
                    OnPropertyChanged(nameof(IsDead));
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

        public bool IsDead
        {
            get { return this.train.GetComponent<Train>().IsDead; }
        }

        public Vector2 Position
        {
            get { return this.train.GetComponent<Train>().TrainPosition; }
        }

        public DataTile CurrentTile // TODO: Convert this Tile into DataTile
        {
            get { return GameObject.FindGameObjectWithTag("DataContainer").GetComponent<DataContainer>().DataNetworkMap.FindDataTile(this.train.GetComponent<Train>().CurrentTile); } 
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
        public int MaxHealth
        {
            get { return this.train.GetComponent<Train>().MaxHealth; }
        }

        public int BulletQuantity
        {
            get { return this.train.GetComponentInChildren<Weapon>().CurrentBulletQuantity; }
        }
        public int MaxBulletQuantity
        {
            get { return this.train.GetComponentInChildren<Weapon>().MaxBulletQuantity; }
        }

        public GameObject CurrentObject
        {
            get { return this.train.GetComponent<Train>().CurrentItem; }
        }
    }
}
