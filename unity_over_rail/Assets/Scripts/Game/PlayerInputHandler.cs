using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

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
            humanTrain.PlayerChoiceDirection(context);
    }

    public void OnAccelerateTrain(InputAction.CallbackContext context)
    {
        if (humanTrain != null)
        {
            if (context.started)
                humanTrain.PlayerIncreaseVelocity(true);
            else if (context.canceled)
                humanTrain.PlayerIncreaseVelocity(false);
        }
    }

    public void OnDecelerateTrain(InputAction.CallbackContext context)
    {
        if (humanTrain != null)
        {
            if (context.started)
                humanTrain.PlayerDecreaseVelocity(true);
            else if (context.canceled)
                humanTrain.PlayerDecreaseVelocity(false);
        }
    }

    public void OnMoveWeapon(InputAction.CallbackContext context)
    {
        if (humanTrain != null)
            humanTrain.PlayerMoveWeapon(context);
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (humanTrain != null && context.action.ReadValue<float>() == 0)
            humanTrain.PlayerShoot(context);
    }

    public void OnUseObject(InputAction.CallbackContext context)
    {
        if (humanTrain != null && context.action.ReadValue<float>() == 0)
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
