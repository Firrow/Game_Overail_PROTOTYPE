using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class SetupPlayerForOnline : NetworkBehaviour
{
    private PlayerInputHandler playerInputHandler;

    private void Start()
    {
        playerInputHandler = this.transform.GetChild(4).GetComponent<PlayerInputHandler>();
    }

    public override void OnStartAuthority()
    {
        playerInputHandler?.OnStartAuthority();
    }

    public override void OnStopAuthority()
    {
        playerInputHandler?.OnStopAuthority();
    }
}
