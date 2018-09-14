using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking; 

public class Player : NetworkBehaviour {

    [SyncVar(hook = "OnChangeTeam")]
    public int team = 0;

    ViewHolder viewHolder;

	// Use this for initialization
	void Start () {
        print("Player started");

        GameObject viewHolderObject = GameObject.FindGameObjectWithTag("ViewHolder");
        viewHolder = viewHolderObject.GetComponent<ViewHolder>();

        if (isLocalPlayer)
        {
            viewHolder.RegisterLocalPlayer(this);
            viewHolder.TryToShowMultiplayerScreen();
        }

	}

    public void SetTeam(int t)
    {
        CmdSetTeam(t);
    }
	
    [Command]     void CmdSetTeam(int t)     {         team = t;     }

    void OnChangeTeam(int t)     {         team = t;         viewHolder.UpdateTeamSettingPlayers();     } 
    [Command]
    public void CmdAskToSetDigit(int d, int h, int v)
    {
        Match m = viewHolder.GetMatch();
        int dig = m.GetValueAt(h, v);
        if (dig < 10)
        {
            if (dig == d%10) // Correct
            {
                RpcSetDigit(d, h, v);
                m.SetDigitAt(d, h, v);
                if (m.IsFull())
                {
                    print("Board complete");
                }
            }
            else // Incorrect
            {
                RpcWrongDigit(d, h, v);
                m.WrongDigit(d);
            }
        }
    }

    public void HostShutDown()
    {
        CmdHostShutDown();
    }

    [Command]
    void CmdHostShutDown()
    {
        print("Host shut down");
        RpcHostShutDown();
        //Invoke("RpcHostShutDown",0.2f);
    }

    [ClientRpc]
    void RpcHostShutDown()
    {
        print("RpcHostShutDown");
        //viewHolder.DisconnectFromMatch();
    }

    [ClientRpc]     void RpcSetDigit(int d, int h, int v)     {         viewHolder.ConfirmDigit(d, h, v);     } 
    [ClientRpc]
    void RpcWrongDigit(int d, int h, int v)
    {
        viewHolder.WrongDigit(d, h, v);
    }

    // Test
    public void BackButtonPressed()
    {
        print("Player.BackButtonPressed");
        CmdBackButtonPressed();
    }
    [Command]
    void CmdBackButtonPressed()
    {
        print("Player.CmdBackButtonPressed");
        RpcBackButtonPressed();
    }
    [ClientRpc]
    void RpcBackButtonPressed()
    {
        print("Player.RpcBackButtonPressed");
        Invoke("DelayedDisconnect", 0.5f);
    }

    void DelayedDisconnect()
    {
        viewHolder.DisconnectFromMatch();
    }
}
