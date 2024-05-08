using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    private PlayerInput playerInput;
    private HumanTrain humanTrain;


    private InputActionAsset inputAsset;
    private InputActionMap playerActionMap;
    private float movementInput;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        var humanTrains = FindObjectsOfType<HumanTrain>();
        int index = playerInput.playerIndex;
        humanTrain = humanTrains.FirstOrDefault(h => h.GetPlayerIndex() == index);
    }

    /*private void OnEnable()
    {
        playerActionMap.FindAction("Move").started += humanTrain.playerChoiceDirection;
        playerActionMap.FindAction("Shoot").started += humanTrain.playerShoot;
        playerActionMap.FindAction("PointerMouse").performed += humanTrain.playerMoveWeapon;
        playerActionMap.FindAction("PointerStick").performed += humanTrain.playerMoveWeapon;
        playerActionMap.Enable();
    }

    private void OnDisable()
    {
        playerActionMap.FindAction("Move").started -= humanTrain.playerChoiceDirection;
        playerActionMap.FindAction("Shoot").started -= humanTrain.playerShoot;
        playerActionMap.FindAction("PointerMouse").performed -= humanTrain.playerMoveWeapon;
        playerActionMap.FindAction("PointerStick").performed -= humanTrain.playerMoveWeapon;
        playerActionMap.Disable();
    }*/

    public void OnMoveTrain(InputAction.CallbackContext context)
    {
        if (humanTrain != null)
            humanTrain.playerChoiceDirection(context);
    }

    public void OnMoveWeapon(InputAction.CallbackContext context)
    {
        if (humanTrain != null)
            humanTrain.playerMoveWeapon(context);
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (humanTrain != null)
            humanTrain.playerShoot(context);
    }
}
