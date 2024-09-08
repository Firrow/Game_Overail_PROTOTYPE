using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using overail.DataContainer_;
using overail.DataTrain_;

/// <summary>
/// Player's interface
/// TODO : Faire en sorte que l'Interface se mette à jour grâce à des events (flag) et non des appels des script Human et IATrain
/// TODO : Faire en sorte que l'Interface aille chercher les données dans les scripts DataXXX. Les scripts DataXXX vont fournir les abonnements
/// </summary>

public class InterfacePlayer : MonoBehaviour
{
    public int index;
    public GameObject healthBarPlayer;
    public GameObject bulletBarPlayer;
    public GameObject objectSlotPlayer;

    private DataContainer dataContainer;
    private DataTrain dataTrain;



    private void Start()
    {
        dataContainer = GameObject.FindGameObjectWithTag("DataContainer").GetComponent<DataContainer>();
        dataTrain = dataContainer.GetTheTrain(index);
        RegisterTrain();

        SetInterface();
    }



    public void RegisterTrain()
    {
        if (dataTrain is not null)
        {
            dataTrain.PropertyChanged += (sender, e) =>
            {
                switch (e.PropertyName)
                {
                    case "Health":
                        healthBarPlayer.GetComponent<HealthBar>().SetHealth(dataTrain.Health);
                        break;
                    case "BulletQuantity":
                        bulletBarPlayer.GetComponent<BulletBar>().SetBullet(dataTrain.BulletQuantity);
                        break;
                    case "CurrentObject":
                        if (dataTrain.CurrentObject is null)
                        {
                            objectSlotPlayer.GetComponent<ObjectSlot>().UndisplayActualObject();
                        }
                        else
                        {
                            objectSlotPlayer.GetComponent<ObjectSlot>().DisplayActualObject(dataTrain.CurrentObject.GetComponent<SpriteRenderer>().sprite);
                        }
                        break;
                    case "IsDead":
                        ResetInterface();
                        break;
                    default:
                        break;
                }
            };
        }
    }

    private void SetInterface()
    {
        bulletBarPlayer.GetComponent<BulletBar>().SetMaxBullet(dataTrain.MaxBulletQuantity);
        bulletBarPlayer.GetComponent<BulletBar>().SetBullet(dataTrain.BulletQuantity);
        healthBarPlayer.GetComponent<HealthBar>().SetMaxHealth(dataTrain.MaxHealth);
        healthBarPlayer.GetComponent<HealthBar>().SetHealth(dataTrain.Health);
    }

    private void ResetInterface()
    {
        bulletBarPlayer.GetComponent<BulletBar>().SetBullet(0);
        healthBarPlayer.GetComponent<HealthBar>().SetHealth(0);
        objectSlotPlayer.GetComponent<ObjectSlot>().UndisplayActualObject();
    }
}
