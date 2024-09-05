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
        dataContainer = GameObject.FindGameObjectWithTag("TEMPDataContainer").GetComponent<DataContainer>();
        dataTrain = dataContainer.GetTheTrain(index);
        RegisterTrain();


    }



    public void RegisterTrain()
    {
        if (dataTrain is not null)
        {
            dataTrain.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == "Health") //TODO : faire un switch
                {
                    healthBarPlayer.GetComponent<HealthBar>().SetHealth(dataTrain.Health);
                }
                else if (e.PropertyName == "BulletQuantity")
                {
                    bulletBarPlayer.GetComponent<BulletBar>().SetBullet(dataTrain.BulletQuantity);
                }
                else if (e.PropertyName == "CurrentObject")
                {
                    if (dataTrain.CurrentObject is null)
                    {
                        objectSlotPlayer.GetComponent<ObjectSlot>().UndisplayActualObject();
                    }
                    else
                    {
                        objectSlotPlayer.GetComponent<ObjectSlot>().DisplayActualObject(dataTrain.CurrentObject.GetComponent<SpriteRenderer>().sprite);
                    }
                }
            };
        }
    }
}
