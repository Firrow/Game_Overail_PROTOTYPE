using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using overail.TrainActions_;
using overail.GlobalAction_;

/// <summary>
/// Functions about player's actions in game
/// </summary>

public class PlayerInputHandler : MonoBehaviour
{
    private PlayerInput playerInput;
    private HumanTrain humanTrain;
    private int index;



    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        index = playerInput.playerIndex;
    }

    public void OnMoveTrain(InputAction.CallbackContext context) //TODO changer le nom en ChangeDirection
    {
        Debug.Log("INDEX : " + index);
        TrainActions.APIChangeDirection(index, (int)context.ReadValue<Vector2>().x);
    }

    public void OnAccelerateTrain(InputAction.CallbackContext context)
    {
        if (context.started)
            TrainActions.APIOnAccelerateTrain(index, true);
        else if (context.canceled)
            TrainActions.APIOnAccelerateTrain(index, false);
    }

    public void OnDecelerateTrain(InputAction.CallbackContext context)
    {
        if (context.started)
            TrainActions.APIOnDecelerateTrain(index, true);
        else if (context.canceled)
            TrainActions.APIOnDecelerateTrain(index, false);
    }

    public void OnMoveWeapon(InputAction.CallbackContext context)
    {
        TrainActions.APIOnMoveWeapon(index, context.ReadValue<Vector2>(), context.control.device is Mouse);
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        TrainActions.APIOnShoot(index, context.action.ReadValue<float>());
    }

    public void OnUseObject(InputAction.CallbackContext context)
    {
       TrainActions.APIUsePickObject(index, context.action.ReadValue<float>());
    }

    public void RestartGame(InputAction.CallbackContext context)
    {
        GlobalActions.APIRestartGame();
    }

    public void OnQuit(InputAction.CallbackContext context)
    {
        GlobalActions.APIQuitGame();
    }
}
