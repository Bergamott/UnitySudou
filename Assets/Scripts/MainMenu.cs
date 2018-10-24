using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class MainMenu : MonoBehaviour, ButtonHolder {

    public ViewHolder owner;

	public void ButtonPressed(int b)
    {
        if (b == 0) // Start button
        {
            owner.GoToOfflineSkillSetting();
        }
        else if (b == 1) // Compete button
        {
            owner.JoinGameSession();
        }
        else if (b == 2) // Stats button
        {

        }
        else if (b == 3) // Settings button
        {
            owner.GoToOnlineSkillSetting();
        }
    }

    public bool IsResponsive()
    {
        // Temp
        return true;
    }

}
