using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamManager : Singleton<TeamManager>
{
    public Color colour_TeamOne;
    public Color colour_TeamTwo;

    float score_TeamOne = 0;
    float score_TeamTwo = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
