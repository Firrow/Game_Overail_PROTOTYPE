using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    private PlayerInput playerInput;
    private HumanTrain humanTrain;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        var humanTrains = FindObjectsOfType<HumanTrain>();
        int index = playerInput.playerIndex;
        humanTrain = humanTrains.FirstOrDefault(h => h.GetPlayerIndex() == index);
    }

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
