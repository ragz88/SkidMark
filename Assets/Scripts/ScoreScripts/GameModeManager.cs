using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameModeManager : Singleton<GameModeManager>
{
    [Header("Game Mode Settings")]
    [SerializeField] public GameMode gameMode;
    [Header("Coverage")]
    [Tooltip("If a team controls more than this % value in the coverage mode, they immediately win.")]
    [SerializeField] float coverageWinPercent = 65;

    
    [Header("Tug Of War")]
    [Tooltip("In the Tug of War gamemode, if a team controls this % more than the other team, they will win.")]
    [SerializeField] float tugOfWarRelativeWinPercent = 25;

    
    [Header("Zones")]
    [Tooltip("In the Zones gamemode, if a team controls this many zones, they will win.")]
    [SerializeField] int zonesToWin = 4;

    
    [Header("Drift Rush")]
    [Tooltip("In the Drift Rush gamemode, the player has this many seconds to drift before losing.")]
    [SerializeField] float driftRushStartTime = 10;
    float driftRushCurrentTime;


    [Space][Space][Space]
    [Header("General Settigns")]
    [Tooltip("Game will automatically end after this many seconds.")]
    [SerializeField] float gameLength = 300;
    [Tooltip("In the event of a draw, game will be extended by this many seconds.")]
    [SerializeField] float overtime = 60;

    [Space]
    [SerializeField] DriftPainter[] teamOneCars;
    [SerializeField] DriftPainter[] teamTwoCars;

    public Color colour_TeamOne;
    public Color colour_TeamTwo;

    [Space]
    public DriftManager driftManager;
    public GameObject scoreNodePrefab;
    public bool showDebugNodes;


    // Temporary visualisation
    public TMP_Text scoreUI;

    Paintable[] paintableSurfaces;
    bool scoreUpdatedThisFrame = false;

    float score_TeamOne = 0;
    float score_TeamTwo = 0;

    float playTime = 0;

    // sum of the surface area of all paintable areas.
    float totalSurfaceArea = 0;

    // Set to 1 or 2 if either team wins the game.
    int winningTeam = 0;


    /// <summary>
    /// Set to true if any type of Painter has adjusted the score in this frame.
    /// </summary>
    public bool ScoreUpdatedThisFrame
    {
        set
        {
            scoreUpdatedThisFrame = value;
        }
    }

    /// <summary>
    /// The team that one the game - either 1 or 2.
    /// </summary>
    public int WinningTeam
    {
        get { return winningTeam; }
    }

    /// <summary>
    /// The current % of the field covered by Team 1
    /// </summary>
    public float TeamOneScore
    {
        get { return score_TeamOne; }
    }

    /// <summary>
    /// The current % of the field covered by Team 2
    /// </summary>
    public float TeamTwoScore
    {
        get { return score_TeamTwo; }
    }

    /// <summary>
    /// The difference in coverage percent needed to win a tug of war game.
    /// </summary>
    public float TugOfWarRelativeWinPercent
    {
        get { return tugOfWarRelativeWinPercent; }
    }


    public enum GameMode
    {
        Coverage,
        TugOfWar,
        Zones,
        DriftRush,
        FreePlay
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < teamOneCars.Length; i++)
        {
            teamOneCars[i].paintColor = colour_TeamOne;
        }

        for (int i = 0; i < teamTwoCars.Length; i++)
        {
            teamTwoCars[i].paintColor = colour_TeamTwo;
        }

        paintableSurfaces = GameObject.FindObjectsOfType<Paintable>();

        // Calculates total area
        for (int i = 0; i < paintableSurfaces.Length; i++)
        {
            totalSurfaceArea += paintableSurfaces[i].getSurfaceArea();
        }

        driftRushCurrentTime = driftRushStartTime;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (gameMode == GameMode.DriftRush)
        {
            if (driftManager.isDrifting)
            {
                driftRushCurrentTime = driftRushStartTime;
            }
            else
            {
                driftRushCurrentTime -= Time.deltaTime;
            }
            GlobalUIManager.instance.UpdateTimer(driftRushCurrentTime);
        }
        else
        {
            playTime += Time.deltaTime;
            float remainingTime = gameLength - playTime;

            if (remainingTime <= 0)
            {
                remainingTime = 0;
            }

            GlobalUIManager.instance.UpdateTimer(remainingTime);
        }


        // Update the score, if necessary
        if (scoreUpdatedThisFrame)
        {
            UpdateScore();
        }
        scoreUpdatedThisFrame = false;

        CheckEndConditions();

        if (winningTeam != 0)
        {
            GlobalUIManager.instance.ShowEndScreen();
        }
    }



    /// <summary>
    /// Loops through all Paintable Surfaces and averages out their scores to obtain the team score.
    /// </summary>
    void UpdateScore()
    {
        float totalTeamOne = 0;
        float totalTeamTwo = 0;

        for (int i = 0; i < paintableSurfaces.Length; i++)
        {
            totalTeamOne += (paintableSurfaces[i].GetScore(1) * paintableSurfaces[i].getSurfaceArea());
            totalTeamTwo += (paintableSurfaces[i].GetScore(2) * paintableSurfaces[i].getSurfaceArea());
        }

        score_TeamOne = (totalTeamOne / totalSurfaceArea) * 100;
        score_TeamTwo = (totalTeamTwo / totalSurfaceArea) *100;

        GlobalUIManager.instance.UpdateUI();
    }



    /// <summary>
    /// Checks the current score and runtime of the game. Compares these to the necessary conditions to end the game.
    /// </summary>
    void CheckEndConditions()
    {
        switch (instance.gameMode)
        {
            case GameMode.Coverage:
                CheckCoverageEnd();
                break;

            case GameMode.TugOfWar:
                CheckTugOfWarEnd();
                break;

            case GameMode.Zones:
                CheckZonesEnd();
                break;

            case GameMode.DriftRush:
                CheckDriftRushEnd();
                break;
        }
    }


    void CheckCoverageEnd()
    {
        // This needs to be upgraded to account for draws and be fair - but it will suffice for the jam.
        if (playTime > gameLength)
        {
            if (score_TeamOne > score_TeamTwo)
            {
                winningTeam = 1;
            }
            else
            {
                winningTeam = 2;
            }
        }
        else
        {
            if (score_TeamOne > coverageWinPercent)
            {
                winningTeam = 1;
            }
            else if (score_TeamTwo > coverageWinPercent)
            {
                winningTeam = 2;
            }
        }
    }


    void CheckTugOfWarEnd()
    {
        // This needs to be upgraded to account for draws and be fair - but it will suffice for the jam.
        if (playTime > gameLength)
        {
            if (score_TeamOne > score_TeamTwo)
            {
                winningTeam = 1;
            }
            else
            {
                winningTeam = 2;
            }
        }
        else
        {
            if ((score_TeamOne - score_TeamTwo) > tugOfWarRelativeWinPercent)
            {
                winningTeam = 1;
            }
            else if ((score_TeamTwo - score_TeamOne) > tugOfWarRelativeWinPercent)
            {
                winningTeam = 2;
            }
        }
    }

    void CheckZonesEnd()
    {
        //Will not prioritise this game mode right now.
    }


    void CheckDriftRushEnd()
    {
        if (driftRushCurrentTime <= 0)
        {
            // We're using this to represent a loss for Drift Rush, as the player's team is not the winning team.
            winningTeam = 2;
        }
    }



    /// <summary>
    /// Used for Drift Rush and Free Play. Triggered as the player crosses the end line.
    /// </summary>
    public void FinishLineReached()
    {
        winningTeam = 1;
    }




    /// <summary>
    /// Updates the scre of the specified team.
    /// </summary>
    /// <param name="newScore">The updated score as a percent of the map's surface area</param>
    /// <param name="team">The team that this score applies to - either 1 or 2</param>
    void SetScore(float newScore, int team)
    {
        if (newScore < 0 && newScore > 100)
        {
            Debug.LogWarning("Trying to update with a score outside a regular percentage range");
            return;
        }

        if (team == 1)
        {
            score_TeamOne = newScore;
        }
        else if (team == 2)
        {
            score_TeamTwo = newScore;
        }
        else
        {
            Debug.LogWarning("Trying to update the score of a non-existent team. Only 1 or 2 can be passed into 'team' parameter.");
            return;
        }
    }




}
