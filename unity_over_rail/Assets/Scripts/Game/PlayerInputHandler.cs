using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Mirror;

public class PlayerInputHandler : /*MonoBehaviour*/ NetworkBehaviour
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




    private void Awake()
    {
        //playerInput = GetComponent<PlayerInput>();
        //humanTrain = GetComponent<HumanTrain>();
        // Ton prefab de joueur DOIT contenir HumanTrain
        // ou au minimum avoir un champ [SerializeField] vers son train.
    }

    public override void OnStartAuthority()
    {
        // On active les inputs uniquement pour le joueur local
        playerInput.enabled = true;
        Debug.Log("OnStartAuthority -> Input activé");
    }

    public override void OnStopAuthority()
    {
        // Si jamais l’autorité est retirée (rare, mais possible), on coupe les inputs
        playerInput.enabled = false;
        Debug.Log("OnStopAuthority -> Input désactivé");
    }

    private void Start()
    {
        /*if (!isLocalPlayer)
        {
            // Désactive les inputs pour les autres joueurs
            playerInput.enabled = false;
            Debug.Log("Disable playerInput quand pas local");
        }*/
    }

    public void OnMoveTrain(InputAction.CallbackContext context)
    {
        Debug.Log("OnMoveTrain");
        if (!isLocalPlayer) return;

        Debug.Log("humanTrain : " + humanTrain);

        if (humanTrain != null)
            humanTrain.PlayerChoiceDirection(context);

        Debug.Log("-----------------");
    }

    public void OnAccelerateTrain(InputAction.CallbackContext context)
    {
        if (!isLocalPlayer) return;

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
        if (!isLocalPlayer) return;

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
        if (!isLocalPlayer) return;

        humanTrain?.PlayerMoveWeapon(context);
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (!isLocalPlayer) return;

        if (humanTrain != null && context.action.ReadValue<float>() == 0)
            humanTrain.PlayerShoot(context);
    }

    public void OnUseObject(InputAction.CallbackContext context)
    {
        if (!isLocalPlayer) return;

        if (humanTrain != null && humanTrain.CurrentItem != null && context.action.ReadValue<float>() == 0)
            humanTrain.UsePickObject();
    }
}
