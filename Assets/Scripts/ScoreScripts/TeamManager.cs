using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TeamManager : Singleton<TeamManager>
{
    [SerializeField] DriftPainter[] teamOneCars;
    [SerializeField] DriftPainter[] teamTwoCars;

    public Color colour_TeamOne;
    public Color colour_TeamTwo;

    public GameObject scoreNodePrefab;
    public bool showDebugNodes;


    // Temporary visualisation
    public TMP_Text scoreUI;

    Paintable[] paintableSurfaces;
    bool scoreUpdatedThisFrame = false;

    float score_TeamOne = 0;
    float score_TeamTwo = 0;

    // sum of the surface area of all paintable areas.
    float totalSurfaceArea = 0;


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

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < teamOneCars.Length; i++)
        {
            teamOneCars[i].paintColor = colour_TeamOne;
            teamTwoCars[i].paintColor = colour_TeamTwo;
        }

        paintableSurfaces = GameObject.FindObjectsOfType<Paintable>();

        // Calculates total area
        for (int i = 0; i < paintableSurfaces.Length; i++)
        {
            totalSurfaceArea += paintableSurfaces[i].getSurfaceArea();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (scoreUpdatedThisFrame)
        {
            UpdateScore();
        }

        scoreUpdatedThisFrame = false;
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

        if (scoreUI != null)
        {
            scoreUI.text = score_TeamOne.ToString();
        }
    }
}
