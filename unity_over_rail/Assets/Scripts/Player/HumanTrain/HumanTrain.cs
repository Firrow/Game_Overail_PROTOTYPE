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



    void Update()
    {
        trainAngle = angle;
    }



    public void UpdateBulletBar(int updateValue)
    {
        bulletBar.SetBullet(updateValue);
    }
}
