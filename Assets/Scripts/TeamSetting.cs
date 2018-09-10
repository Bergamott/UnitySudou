using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamSetting : MonoBehaviour, ButtonHolder {
    
    public ViewHolder owner;

    public FruitHolder bananaHolder;
    public FruitHolder peachHolder;
    public FruitHolder pearHolder;
    public FruitHolder cherryHolder;

    public Button startButton;

    FruitHolder[] fruitHolders;

    int team = 0;
    int[] teamPlayers = { 0, 0, 0, 0, 0 };

    public void Start()
    {
        fruitHolders = new FruitHolder[] {bananaHolder,peachHolder,pearHolder,cherryHolder };
        for (int i = 0; i < 4;i++)
        {
            fruitHolders[i].Clear();
        }
    }

    public void ShowScreen(bool isHost)
    {
        startButton.gameObject.SetActive(isHost);
        transform.gameObject.SetActive(true);
        team = 0;
        UpdateCircles();
        UpdatePlayerCounters();
    }

    public void ButtonPressed(int b)
    {
        if (b >= 2 && b <= 5)
        {
            if (owner.localPlayer.team != b-1)
            {
                team = b-1;
                owner.localPlayer.SetTeam(b-1);
                UpdateCircles();
            }
        }
        else if (b==0) // Start match
        {
            print("Initiating multiplayer match");
            bool[] activeTeams = new bool[4];
            for (int i = 0; i < 4;i++)
            {
                activeTeams[i] = (teamPlayers[i + 1] > 0);
            }
            owner.InitiateMultiplayerMatch(activeTeams);
        }
        else if (b==1) // Back
        {
            owner.BackButtonPressed();
        }
    }

    public void UpdatePlayerCounters()
    {
        for (int i = 0; i < 5; i++)             teamPlayers[i] = 0;         GameObject[] players = GameObject.FindGameObjectsWithTag("Player");         foreach (GameObject gob in players)         {             Player pl = gob.GetComponent<Player>();             teamPlayers[pl.team]++;         }         int fullTeams = 0;         for (int i = 1; i < 5; i++)         {             if (teamPlayers[i] > 0)                 fullTeams++;         }         for (int i = 0; i < 4; i++)
        {
            fruitHolders[i].UpdateTeamSize(teamPlayers[i + 1]);
        }
    }

    public void UpdateCircles()
    {
        for (int i = 0; i < 4; i++)
        {
            fruitHolders[i].JoinTeam(team == i + 1);
        }
    }
}
