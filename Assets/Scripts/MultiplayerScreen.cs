using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerScreen : MonoBehaviour, ButtonHolder {

    public ViewHolder owner;

    public GameObject background;
    public GameObject grid;
    public GameObject cursor;
    Color[] teamColors = { Color.gray, Color.yellow, new Color(1.0f, 0.6f, 0), Color.green, Color.red };

    public Sprite grid9x9;
    public Sprite grid6x6;
    public Sprite grid4x4;

    public TextMesh timerText;
    public GameObject digitButtons4;
    public GameObject digitButtons6;
    public GameObject digitButtons9;
    public TextMesh viewerModeText;

    public Match match;

    bool[] activeTeams = new bool[4];
    public Leaderboard leaderboard;

    // Prefab board digits
    public GameObject digit9;
    public GameObject digit6;
    public GameObject digit4;

    int size = 4;
    int gridType = 0;
    int myTeam = 0;
    bool isHost = false;
    float timeLeft = 0;
    int lastDisplayedTime;
    float[] digitOffsets = {1.86f,2.20f,2.45f};
    float[] digitSpaces = { 1.23f, 0.88f, 0.613f};
    float[] cursorScales = { 1.9f, 1.33f, 0.9f};

    int cursorX = -1;
    int cursorY = -1;

    int[] scores = new int[4];

    Color[] textColors = { Color.gray, new Color(0.7f, 0.7f, 0), new Color(0.8f, 0.4f, 0), new Color(0, 0.85f, 0), Color.red , Color.black};

    public GameObject rightParticle;
    public GameObject wrongParticle;

    // Sound effects
    int soundH, soundV, soundD;
    public GameObject rightSound;
    public AudioClip rightClip;
    public GameObject wrongSound;
    public AudioClip wrongClip;
    public GameObject endSound;
    public AudioClip endClip;
    public GameObject refreshSound;
    public AudioClip refreshClip;

    int gridFadePhase = 0;
    float fadeTimer = 0;

    public GameObject endSign;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        // Handle touches
        bool repositionCursor = false;
        if (Input.touchCount > 0 && myTeam > 0 && timeLeft > 0)
        {
            Vector3 world = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            float xCoord = world.x;
            float yCoord = world.y;
            int x = Mathf.FloorToInt((xCoord + digitOffsets[gridType] + 0.5f * digitSpaces[gridType]) / digitSpaces[gridType]);
            int y = Mathf.FloorToInt((digitOffsets[gridType] + 0.5f * digitSpaces[gridType] - yCoord + grid.transform.position.y) / digitSpaces[gridType]);
            if (y >= 0 && y < size)
            {
                cursorX = x;
                cursorY = y;
                repositionCursor = true;
            }
        }
        else if (Input.GetMouseButtonDown(0) && myTeam > 0 && timeLeft > 0)
        {
            Vector3 world = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            float xCoord = world.x;
            float yCoord = world.y;
            int x = Mathf.FloorToInt((xCoord+digitOffsets[gridType]+0.5f*digitSpaces[gridType])/digitSpaces[gridType]);
            int y = Mathf.FloorToInt((digitOffsets[gridType] + 0.5f * digitSpaces[gridType] -yCoord+grid.transform.position.y) / digitSpaces[gridType]);
            if (y >=0 && y < size)
            {
                cursorX = x;
                cursorY = y;
                repositionCursor = true;
            }
        }
        if (repositionCursor)
        {
            if (cursorX >= 0 && cursorX < size && cursorY >= 0 && cursorY < size && match.GetValueAt(cursorX, cursorY) < 10)
            {
                cursor.transform.position = new Vector3(grid.transform.position.x -
                                                        digitOffsets[gridType] + digitSpaces[gridType] * cursorX,
                                                        grid.transform.position.y +
                                                      digitOffsets[gridType] - digitSpaces[gridType] * cursorY, -0.2f);
                cursor.SetActive(true);
            }
            else
            {
                cursorX = -1;
                cursor.SetActive(false);
            }
        }

        // Handle timer
        if (isHost && timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            int newTime = Mathf.FloorToInt(timeLeft);
            if (newTime != match.secondsLeft)
            {
                if (newTime >= 0)
                {
                    match.CmdUpdateTime(newTime);
                }
                else
                {
                    AudioSource source = endSound.GetComponent<AudioSource>();
                    source.PlayOneShot(endClip, 1.0f);
                    endSign.gameObject.SetActive(true);
                }
            }
        }
        if (lastDisplayedTime != match.secondsLeft)
        {
            lastDisplayedTime = match.secondsLeft;
            timerText.text = string.Format("{0}:{1}{2}", lastDisplayedTime / 60, (lastDisplayedTime / 10) % 6, lastDisplayedTime % 10);
        }

        // Handle grid fade
        if (gridFadePhase > 0)
        {
            SpriteRenderer gridSprite = grid.GetComponent<SpriteRenderer>();
            fadeTimer += Time.deltaTime;
            Color fadeColor = Color.white;
            if (gridFadePhase == 1)
            {
                // Fade out
                if (fadeTimer < 1.0f)
                {
                    fadeColor = new Color(1f, 1f, 1f, Mathf.SmoothStep(1.0f, 0, fadeTimer));
                }
                else
                {
                    fadeColor = new Color(1f, 1f, 1f, 0);
                    fadeTimer = 0;
                    gridFadePhase = 2;
                    HideCursor();
                    ClearNumbers();
                    PlaceNumbers();
                }

            }
            else if (gridFadePhase == 2)
            {
                // Fade in
                if (fadeTimer < 1.0f)
                {
                    fadeColor = new Color(1f, 1f, 1f, Mathf.SmoothStep(0, 1.0f, fadeTimer));
                }
                else
                {
                    gridFadePhase = 0;
                    fadeColor = new Color(1f, 1f, 1f, 1f);
                }
            }
            gridSprite.color = fadeColor;
            GameObject[] oldNumbers = GameObject.FindGameObjectsWithTag("Digit");
            foreach (GameObject gob in oldNumbers)
            {
                Color dCol = gob.GetComponent<TextMesh>().color;
                dCol.a = fadeColor.a;
                gob.GetComponent<TextMesh>().color = dCol;
            }
        }

	}

    void CheckForActiveTeams()
    {
        for (int i = 0; i < 4; i++)
            activeTeams[i] = match.GetActiveTeam(i);
        //GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        //foreach (GameObject gob in players)
        //{
        //    Player pl = gob.GetComponent<Player>();
        //    if (pl.team > 0)
        //        activeTeams[pl.team - 1] = true;
        //}
        leaderboard.InitiateWithActiveTeams(activeTeams);
    }

    public void SetupWithMatch(Match m, Player p)
    {
        myTeam = p.team;
        isHost = p.isServer;
        match = m;
        timeLeft = m.secondsLeft;
        lastDisplayedTime = -1;
        int skill = m.GetSkillLevel();
        gridType = skill / 3;
        SpriteRenderer gr = grid.GetComponent<SpriteRenderer>();
        if (gridType == 0)
        {
            gr.sprite = grid4x4;
        }
        else if (gridType == 1)
        {
            gr.sprite = grid6x6;
        }
        else
        {
            gr.sprite = grid9x9;
        }

        digitButtons4.SetActive(skill < 3 && myTeam > 0);
        digitButtons6.SetActive(skill >= 3 && skill <= 5 && myTeam > 0);
        digitButtons9.SetActive(skill > 5 && myTeam > 0);
        viewerModeText.gameObject.SetActive(myTeam == 0);

        CheckForActiveTeams();

        // Set colors for background and cursor
        SpriteRenderer sr = background.GetComponent<SpriteRenderer>();
        sr.color = teamColors[myTeam];
        sr = cursor.GetComponent<SpriteRenderer>();
        sr.color = textColors[myTeam];

        cursor.transform.localScale = new Vector3(cursorScales[gridType],cursorScales[gridType],1.0f);

        HideCursor();

        PlaceNumbers();

        for (int i = 0; i < 4;i++)
        {
            scores[i] = m.GetScoreForTeam(i + 1);
            leaderboard.SetTeamScore(i + 1, scores[i]);
        }

        soundH = -1;
        soundV = -1;

        transform.gameObject.SetActive(true);
        endSign.gameObject.SetActive(timeLeft <= 0);
    }

    public void PlaceNumbers()
    {
        size = match.GetSize();
        for (int i = 0; i < size;i++)
        {
            for (int j = 0; j < size;j++)
            {
                int d = match.GetValueAt(j, i);
                if (d > 9)
                {
                    SetDigit(d, j, i);
                }
            }
        }
    }

    void ClearNumbers()
    {
        GameObject[] oldNumbers = GameObject.FindGameObjectsWithTag("Digit");
        foreach (GameObject gob in oldNumbers)
        {
            Destroy(gob);
        }
    }

    public void SetDigit(int d, int h, int v)
    {
        GameObject newDigit;
        if (gridType == 0)
        {
            newDigit = Instantiate(digit4);
        }
        else if (gridType == 1)
        {
            newDigit = Instantiate(digit6);
        }
        else
        {
            newDigit = Instantiate(digit9);
        }
        newDigit.tag = "Digit";
        TextMesh digitMesh = newDigit.GetComponent<TextMesh>();
        digitMesh.text = "" + (d % 10);
        digitMesh.color = textColors[d / 10];
        newDigit.transform.position = new Vector3(grid.transform.position.x - 
                                                  digitOffsets[gridType]+digitSpaces[gridType]*h,
                                                  grid.transform.position.y +
                                                  digitOffsets[gridType] - digitSpaces[gridType] * v, -0.2f);
        newDigit.transform.parent = grid.transform;
    }

    public void ManuallySetDigit(int d, int h, int v)
    {
        SetDigit(d, h, v);
        if (cursorX == h && cursorY == v)
        {
            HideCursor();
        }
        int team = d / 10;
        scores[team - 1] += 1;
        leaderboard.SetTeamScore(team, scores[team - 1]);
        GameObject particle = Instantiate(rightParticle, new Vector3(grid.transform.position.x -
                                                  digitOffsets[gridType] + digitSpaces[gridType] * h,
                                                  grid.transform.position.y +
                                                  digitOffsets[gridType] - digitSpaces[gridType] * v, -0.2f), Quaternion.identity);
        ParticleSystem.MainModule settings = particle.GetComponent<ParticleSystem>().main;
        settings.startColor = textColors[team];
        Destroy(particle, 1.5f);
        // Check if this was my move
        if (soundH == h && soundV == v && soundD == d)
        {
            AudioSource source = rightSound.GetComponent<AudioSource>();
            source.PlayOneShot(rightClip, 1.0f);
        }
        soundH = -1;
    }

    public void WrongDigit(int d, int h, int v)
    {
        int team = d / 10;
        scores[team - 1] -= 2;
        leaderboard.SetTeamScore(team, scores[team - 1]);
        GameObject particle = Instantiate(wrongParticle, new Vector3(grid.transform.position.x -
                                                  digitOffsets[gridType] + digitSpaces[gridType] * h,
                                                  grid.transform.position.y +
                                                  digitOffsets[gridType] - digitSpaces[gridType] * v, -0.2f), Quaternion.identity);
        ParticleSystem.MainModule settings = particle.GetComponent<ParticleSystem>().main;
        settings.startColor = textColors[team];
        Destroy(particle, 1.5f);
        // Check if this was my move
        if (soundH == h && soundV == v && soundD == d)
        {
            AudioSource source = wrongSound.GetComponent<AudioSource>();
            source.PlayOneShot(wrongClip, 1.0f);
        }
        soundH = -1;
    }

    public void ButtonPressed(int b)
    {
        if (b >= 1 && b <= 9 && match.secondsLeft > 0) // More checks later
        {
            if (cursorX >=0 && cursorX < size && cursorY >=0 && cursorY < size)
            {
                soundH = cursorX;
                soundV = cursorY;
                soundD = b + 10 * myTeam;
                owner.AskToSetDigit(b + 10 * myTeam, cursorX, cursorY);
            }
        }
        else if (b == 0) // Back
        {
            //owner.AskToDisconnect();
            owner.BackButtonPressed();
        }
    }

    public bool IsResponsive()
    {
        // Temp
        return true;
    }

    void HideCursor()
    {
        cursorX = -1;
        cursorY = -1;
        cursor.SetActive(false);
    }

    public void Leave()
    {
        ClearNumbers();
        gameObject.SetActive(false);
        HideCursor();

        // More stuff later


    }

    public void FadeInNewBoard()
    {
        fadeTimer = 0;
        gridFadePhase = 1;
        AudioSource source = refreshSound.GetComponent<AudioSource>();
        source.PlayOneShot(refreshClip, 1.0f);
    }
}
