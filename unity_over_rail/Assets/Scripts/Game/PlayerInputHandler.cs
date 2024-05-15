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

    public void OnAccelerateTrain(InputAction.CallbackContext context)
    {
        if (humanTrain != null)
        {
            if (context.started)
                humanTrain.playerIncreaseVelocity(true);
            else if (context.canceled)
                humanTrain.playerIncreaseVelocity(false);
        }
    }

    public void OnDecelerateTrain(InputAction.CallbackContext context)
    {
        if (humanTrain != null)
        {
            if (context.started)
                humanTrain.playerDecreaseVelocity(true);
            else if (context.canceled)
                humanTrain.playerDecreaseVelocity(false);
        }
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



    //TEMPORAIRE POUR TESTS
    public void OnQuit(InputAction.CallbackContext context)
    {
        Application.Quit();
    }
}
