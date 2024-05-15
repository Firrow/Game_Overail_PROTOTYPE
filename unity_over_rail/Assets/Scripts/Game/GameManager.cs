using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public void OnQuit(InputAction.CallbackContext context)
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
