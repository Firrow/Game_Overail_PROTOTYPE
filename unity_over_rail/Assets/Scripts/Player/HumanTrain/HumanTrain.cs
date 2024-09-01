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

        //healthBar.SetHealth(currentHealth); //Lequel choisir ??? (garder SetMaxHealth pour les tests)
        // TODO : voir pour mettre à jour automatiquement la barre de vie lorsque la valeur est changée dans IATrain
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
