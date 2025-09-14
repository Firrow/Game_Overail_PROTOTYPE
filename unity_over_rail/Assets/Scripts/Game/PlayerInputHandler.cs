using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerInputHandler : MonoBehaviour
{
    [SerializeField]
    private PlayerInput playerInput;
    [SerializeField]
    private HumanTrain humanTrain;


    //MULTI LOCAL
    /*private void Awake()
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
    }*/



    // MULTI ONLINE
    public void OnStartAuthority()
    {
        playerInput.enabled = true;
    }

    public void OnStopAuthority()
    {
        playerInput.enabled = false;
    }

    public void OnMoveTrain(InputAction.CallbackContext context)
    {
        Debug.Log("OnMoveTrain");
        //if (!isLocalPlayer) return;

        Debug.Log("humanTrain : " + humanTrain);

        if (humanTrain != null)
            humanTrain.PlayerChoiceDirection(context);

        Debug.Log("-----------------");
    }

    public void OnAccelerateTrain(InputAction.CallbackContext context)
    {
        //if (!isLocalPlayer) return;

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
        //if (!isLocalPlayer) return;

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
        //if (!isLocalPlayer) return;

        humanTrain?.PlayerMoveWeapon(context);
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        //if (!isLocalPlayer) return;

        if (humanTrain != null && context.action.ReadValue<float>() == 0)
            humanTrain.PlayerShoot(context);
    }

    public void OnUseObject(InputAction.CallbackContext context)
    {
        //if (!isLocalPlayer) return;

        if (humanTrain != null && humanTrain.CurrentItem != null && context.action.ReadValue<float>() == 0)
            humanTrain.UsePickObject();
    }
}
