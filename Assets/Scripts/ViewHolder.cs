using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewHolder : MonoBehaviour {

    public MainMenu mainMenu;
    public SkillSetting skillSetting;
    public TeamSetting teamSetting;
    public Networker networker;
    public MultiplayerScreen multiplayerScreen;
    public GameObject waitScreen;

    public Player localPlayer;
    Match activeMatch = null;

    public void Start()
    {
        mainMenu.gameObject.SetActive(true);
        skillSetting.gameObject.SetActive(false);
        teamSetting.gameObject.SetActive(false);
        multiplayerScreen.gameObject.SetActive(false);
        HideWaitScreen();
    }

    public void GoToOfflineSkillSetting()
    {
        mainMenu.gameObject.SetActive(false);
        skillSetting.SetUpForOffline();
    }

    public void GoToOnlineSkillSetting()
    {
        mainMenu.gameObject.SetActive(false);
        skillSetting.SetUpForOnline();
    }

    public void BackToMainMenuFromSetting()
    {
        mainMenu.gameObject.SetActive(true);
        skillSetting.gameObject.SetActive(false);
    }

    public void ShowTeamSetting(bool host)
    {
        teamSetting.ShowScreen(host);
        skillSetting.gameObject.SetActive(false);
        mainMenu.gameObject.SetActive(false);
        HideWaitScreen();
    }

    // Wait screen
    public void ShowWaitScreen()
    {
        waitScreen.SetActive(true);
    }

    public void HideWaitScreen()
    {
        waitScreen.SetActive(false);
    }

    // Team setting screen

    public void HostGameSession()
    {
        networker.StartAsHost();
    }

    public void JoinGameSession()
    {
        networker.StartClient();
    }

    public void RegisterLocalPlayer(Player lp)
    {
        localPlayer = lp;
        ShowTeamSetting(lp.isServer);
    }

    public void RegisterActiveMatch(Match m)
    {
        activeMatch = m;
    }

    public void UpdateTeamSettingPlayers()
    {
        teamSetting.UpdatePlayerCounters();
    }

    // Match management

    public void InitiateMultiplayerMatch(bool[] teams)
    {
        networker.SpawnMatch(skillSetting.GetSkillLevel(), teams);

    }

    public Match GetMatch()
    {
        return activeMatch;
    }

    public void ShowMultiplayerScreenWithMatch(Match m)
    {
        teamSetting.gameObject.SetActive(false);
        multiplayerScreen.SetupWithMatch(m, localPlayer);
    }

    public void TryToShowMultiplayerScreen()
    {
        if (activeMatch != null && localPlayer != null)
        {
            multiplayerScreen.SetupWithMatch(activeMatch, localPlayer);
            teamSetting.gameObject.SetActive(false);
        }
    }

    public void AskToSetDigit(int d, int h, int v)
    {
        localPlayer.CmdAskToSetDigit(d, h, v);
    }

    public void ConfirmDigit(int d, int h, int v)
    {
        multiplayerScreen.ManuallySetDigit(d, h, v);
    }

    public void WrongDigit(int d, int h, int v)
    {
        multiplayerScreen.WrongDigit(d, h, v);
    }

    public void AskToDisconnect()
    {
        if (localPlayer.isServer)
        {
            localPlayer.HostShutDown();
        }
        else
            DisconnectFromMatch();
    }

    public void DisconnectFromMatch()
    {
        print("DisconnectFromMatch called");
        if (localPlayer.isServer)
        {
            networker.DisconnectHostFromMatch();
        }
        else
        {
            networker.DisconnectClientFromMatch();
        }
        multiplayerScreen.Leave();
        teamSetting.gameObject.SetActive(false);
        localPlayer = null;
        activeMatch = null;
        mainMenu.gameObject.SetActive(true);
    }

    // Test
    public void BackButtonPressed()
    {
        print("ViewHolder.BackButtonPressed");
        if (localPlayer.isServer)
        {
            localPlayer.BackButtonPressed();
        }
        else
        {
            DisconnectFromMatch();
        }
    }

    public void FadeInNewBoard()
    {
        multiplayerScreen.FadeInNewBoard();
    }
}
