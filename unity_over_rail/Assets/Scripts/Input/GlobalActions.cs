using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// API
/// Script that contains all actions that can be done by everyone
/// </summary>

namespace overail.GlobalAction_
{
    public static class GlobalActions
    {
        public static void APIRestartGame()
        {
            GameManager.RestartGame();
        }
        public static void APIQuitGame()
        {
            GameManager.QuitGame();
        }


        public static GameManager GameManager
        {
            get { return GameObject.FindObjectOfType<GameManager>(); }
        }
    }
}

