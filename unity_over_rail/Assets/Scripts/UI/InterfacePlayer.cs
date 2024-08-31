using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Player's interface
/// TODO : Faire en sorte que l'Interface se mette à jour grâce à des events (flag) et non des appels des script Human et IATrain
/// TODO : Faire en sorte que l'Interface aille chercher les données dans les scripts DataXXX. Les scripts DataXXX vont fournir les abonnements
/// </summary>

public class InterfacePlayer : MonoBehaviour
{
    public int index;
    public GameObject healthBarPlayer;
    public GameObject bulletBarPlayer;
    public GameObject objectSlotPlayer;
}
