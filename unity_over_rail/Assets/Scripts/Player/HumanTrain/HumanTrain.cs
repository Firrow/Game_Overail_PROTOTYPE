using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Script with all function for human players only.
/// </summary>

public class HumanTrain : Train
{
    private float movementInput;
    private float trainAngle;



    void Start()
    {
        base.Start();

        // Get the right interface
        foreach (GameObject Element in GameObject.FindGameObjectsWithTag("InterfacePlayer"))
        {
            if (Element.GetComponent<InterfacePlayer>().index == playerIndex)
            {
                this.healthBar = Element.GetComponent<InterfacePlayer>().healthBarPlayer.GetComponent<HealthBar>();
                this.bulletBar = Element.GetComponent<InterfacePlayer>().bulletBarPlayer.GetComponent<BulletBar>();
                this.objectSlot = Element.GetComponent<InterfacePlayer>().objectSlotPlayer.GetComponent<ObjectSlot>();
            }
        }

        // Set this interface
        bulletBar.SetMaxBullet(weapon.GetComponent<Weapon>().MaxBulletQuantity);
        bulletBar.SetBullet(weapon.GetComponent<Weapon>().CurrentBulletQuantity);
        healthBar.SetMaxHealth(MaxHealth);
        //healthBar.SetHealth(currentHealth); //Lequel choisir ??? (garder SetMaxHealth pour les tests)
        // TODO : voir pour mettre à jour automatiquement la barre de vie lorsque la valeur est changée dans IATrain

        // Set the default arrow
        //ChangeArrowColor(leftArrow.GetComponent<SpriteRenderer>(), rightArrow.GetComponent<SpriteRenderer>());
    }

    void Update()
    {
        base.Update();
        trainAngle = angle;
    }



    public void UpdateBulletBar(int updateValue)
    {
        bulletBar.SetBullet(updateValue);
    }
}
