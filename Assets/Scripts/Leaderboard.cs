using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leaderboard : MonoBehaviour {

    public GameObject bananaField;
    public GameObject peachField;
    public GameObject pearField;
    public GameObject cherryField;

    public TextMesh bananaScore;
    public TextMesh peachScore;
    public TextMesh pearScore;
    public TextMesh cherryScore;

    GameObject[] fields;
    TextMesh[] texts;
    int numTeams = 0;
    int[] teamList;
    int[] teamScores;
    int[] teamOrders;
    bool shouldUpdatePosition;
    float moveSpeed = 1.0f;

    float[] listY = { 0.04f, -0.58f, -1.20f, -1.82f };
    Vector3[] listPositions;

	// Use this for initialization
	void Start () {
	}
	
    public void InitiateWithActiveTeams(bool[] t)
    {
        fields = new GameObject[] { bananaField, peachField, pearField, cherryField };
        texts = new TextMesh[] { bananaScore, peachScore, pearScore, cherryScore };
        teamList = new int[]{ 0, 0, 0, 0 };
        teamScores = new int[] { 0, 0, 0, 0 };
        teamOrders = new int[] { 0, 0, 0, 0 };
        listPositions = new Vector3[4];

        numTeams = 0;
        for (int i = 0; i < 4;i++)
        {
            fields[i].SetActive(t[i]);
            if (t[i])
            {
                fields[i].transform.position = new Vector3(transform.position.x, transform.position.y+listY[numTeams], transform.position.z);
                listPositions[numTeams] = fields[i].transform.position;
                teamList[numTeams] = i;
                teamOrders[numTeams] = numTeams;
                numTeams++;
            }
        }
        shouldUpdatePosition = false;
 
    }

	// Update is called once per frame
	void Update () {
        if (shouldUpdatePosition)
        {
            for (int i = 0; i < numTeams; i++)
            {
                fields[teamList[i]].transform.position = Vector3.MoveTowards(fields[teamList[i]].transform.position, listPositions[teamOrders[i]], Time.deltaTime*moveSpeed);
            }
        }
	}

    public void SetTeamScore(int t, int s)
    {
        texts[t - 1].text = "" + s;
        teamScores[t - 1] = s;
        // Sort scores
        int[][] ss = new int[4][];
        for (int i = 0; i < numTeams;i++)
        {
            ss[i] = new int[2];
            ss[i][0] = teamScores[teamList[i]];
            ss[i][1] = i;
        }
        for (int i = 0; i < numTeams - 1;i++)
        {
            for (int j = i + 1; j < numTeams;j++)
            {
                if (ss[i][0] < ss[j][0])
                {
                    int p = ss[i][0];
                    ss[i][0] = ss[j][0];
                    ss[j][0] = p;
                    p = ss[i][1];
                    ss[i][1] = ss[j][1];
                    ss[j][1] = p;
                }
            }
        }
        for (int i = 0; i < numTeams; i++)
        {
            teamOrders[i] = ss[i][1];
        }
        shouldUpdatePosition = true;
    }
}
