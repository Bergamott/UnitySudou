using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class Match : NetworkBehaviour
{
    [SyncVar]
    int skillLevel = 0;
    [SyncVar]
    public int secondsLeft = 300;
    readonly int[] widths = { 4, 4, 4, 6, 6, 6, 9, 9, 9 };

    SyncListInt boardData = new SyncListInt();
    SyncListInt scores = new SyncListInt();
    SyncListBool teams = new SyncListBool();

    [SyncVar]
    int filledNum;

    // Use this for initialization
    void Start()
    {
        print("Match started");
        GameObject[] gob = GameObject.FindGameObjectsWithTag("ViewHolder");
        ViewHolder vh = gob[0].GetComponent<ViewHolder>();
        vh.RegisterActiveMatch(this);
        vh.TryToShowMultiplayerScreen();
    }

    public void SetSkillLevel(int s, int[] digits)
    {
        skillLevel = s;
        filledNum = 0;
        // Set board data
        boardData.Clear();
        for (int i = 0; i < widths[skillLevel]*widths[skillLevel];i++)
        {
            boardData.Add(digits[i]);
            if (digits[i] > 50)
            {
                filledNum++;
            }
        }
    }

    public bool IsFull()
    {
        return filledNum == widths[skillLevel] * widths[skillLevel];
    }

    public void SetActiveTeams(bool[] activeTeams)
    {
        teams.Clear();
        for (int i = 0; i < 4;i++)
        {
            teams.Add(activeTeams[i]);
        }
    }

    public bool GetActiveTeam(int t)
    {
        return teams[t];
    }

    public void ResetScores()
    {
        scores.Clear();
        for (int i = 0; i < 4;i++)
        {
            scores.Add(0);
        }
    }

    public int GetSkillLevel()
    {
        return skillLevel;
    }

    public int GetSize()
    {
        return widths[skillLevel];
    }

    public int GetValueAt(int h, int v)
    {
        return boardData[v * widths[skillLevel] + h];   
    }

    public void SetDigitAt(int d, int h, int v)
    {
        boardData[v * widths[skillLevel] + h] = d;
        scores[d / 10 - 1]++;
        filledNum++;
    }

    public void WrongDigit(int d)
    {
        scores[d / 10 - 1] -= 2;
    }

    public int GetScoreForTeam(int t)
    {
        return scores[t - 1];
    }

    [Command]
    public void CmdUpdateTime(int t)
    {
        secondsLeft = t;
    }
}
