using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using overail.TrainActions_;

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
        int index = playerInput.playerIndex;
    }

    public void OnMoveTrain(InputAction.CallbackContext context) //TODO changer le nom en ChangeDirection
    {
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
        if (humanTrain != null && context.action.ReadValue<float>() == 0)
            humanTrain.PlayerShoot(context);
    }

    public void OnUseObject(InputAction.CallbackContext context)
    {
        if (humanTrain != null && humanTrain.GetComponent<HumanTrain>().CurrentItem != null && context.action.ReadValue<float>() == 0)
            humanTrain.UsePickObject();
    }

    public void RestartGame(InputAction.CallbackContext context)
    {
        SceneManager.LoadScene(0);
    }

    public void OnQuit(InputAction.CallbackContext context)
    {
        Application.Quit();
    }
}
