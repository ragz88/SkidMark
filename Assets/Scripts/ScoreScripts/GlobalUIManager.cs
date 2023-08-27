using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GlobalUIManager : Singleton<GlobalUIManager>
{
    [Header("General References")]
    [SerializeField] TMP_Text timerText;
    [Space]


    [Header("Coverage References")]
    [Tooltip("Bar on left with Team One's Colour.")]
    [SerializeField] Image teamOneCoverageBar;
    [Tooltip("Bar on right with Team Two's Colour.")]
    [SerializeField] Image teamTwoCoverageBar;
    [Tooltip("Will display team one's % score")]
    [SerializeField] TMP_Text teamOneCoverageScoreText;
    [Tooltip("Will display team two's % score")]
    [SerializeField] TMP_Text teamTwoCoverageScoreText;

    [Header("Tug of War References")]
    [Tooltip("Bar on left with Team One's Colour.")]
    [SerializeField] Image teamOneTugOfWarBar;
    [Tooltip("Bar on right with Team Two's Colour.")]
    [SerializeField] Image teamTwoTugOfWarBar;

    [Header("Zones References")]
    

    [Header("Drift Rush References")]
    [Tooltip("Will display Player's score")]
    [SerializeField] TMP_Text driftRushScoreText;


    [Space]
    [Header("End Screen References")]
    [Tooltip("This is a gameobject containing all the UI to be shown at the end of a game.")]
    [SerializeField] GameObject endScreen;

    [Tooltip("Will display winning team name.")]
    [SerializeField] TMP_Text winnerHeading;
    [Tooltip("Will display team one's score")]
    [SerializeField] TMP_Text teamOneScoreText;
    [Tooltip("Will display team two's score")]
    [SerializeField] TMP_Text teamTwoScoreText;


    [Space]
    [Header("UI Containers")]
    [Tooltip("This is a gameobject containing all the necessary UI for the Coverage game mode. Will be set active at the start of a Coverage game.")]
    /// <summary>
    /// Container Game Object for all global Coverage UI
    /// </summary>
    [SerializeField] GameObject coverageUIContainer;

    [Tooltip("This is a gameobject containing all the necessary UI for the Tug of War game mode. Will be set active at the start of a Tug of War game.")]
    /// <summary>
    /// Container Game Object for all global Tug of War UI
    /// </summary>
    [SerializeField] GameObject tugOfWarUIContainer;

    [Tooltip("This is a gameobject containing all the necessary UI for the Zones game mode. Will be set active at the start of a Zones game.")]
    /// <summary>
    /// Container Game Object for all global Zones UI
    /// </summary>
    [SerializeField] GameObject zonesUIContainer;
    
    [Tooltip("This is a gameobject containing all the necessary UI for the Drift Rush game mode. Will be set active at the start of a Drift Rush game.")]
    /// <summary>
    /// Container Game Object for all global Drift Rush UI
    /// </summary>
    [SerializeField] GameObject driftRushUIContainer;

    [Tooltip("This is a gameobject containing all the necessary UI for the Free Play game mode. Will be set active at the start of a Free Play game.")]
    /// <summary>
    /// Container Game Object for all global Free Play UI
    /// </summary>
    [SerializeField] GameObject freePlayUIContainer;





    // Start is called before the first frame update
    void Start()
    {
        // Activate and Deactivate relevant UI based on current game mode.
        switch(GameModeManager.instance.gameMode)
        {
            case GameModeManager.GameMode.Coverage:
                coverageUIContainer.SetActive(true);
                tugOfWarUIContainer.SetActive(false);
                zonesUIContainer.SetActive(false);
                driftRushUIContainer.SetActive(false);
                freePlayUIContainer.SetActive(false);

                timerText.gameObject.SetActive(true);

                teamOneCoverageBar.color = GameModeManager.instance.colour_TeamOne;
                teamTwoCoverageBar.color = GameModeManager.instance.colour_TeamTwo;
                break;

            case GameModeManager.GameMode.TugOfWar:
                coverageUIContainer.SetActive(false);
                tugOfWarUIContainer.SetActive(true);
                zonesUIContainer.SetActive(false);
                driftRushUIContainer.SetActive(false);
                freePlayUIContainer.SetActive(false);

                timerText.gameObject.SetActive(true);

                teamOneTugOfWarBar.color = GameModeManager.instance.colour_TeamOne;
                teamTwoTugOfWarBar.color = GameModeManager.instance.colour_TeamTwo;
                break;

            case GameModeManager.GameMode.Zones:
                coverageUIContainer.SetActive(false);
                tugOfWarUIContainer.SetActive(false);
                zonesUIContainer.SetActive(true);
                driftRushUIContainer.SetActive(false);
                freePlayUIContainer.SetActive(false);

                timerText.gameObject.SetActive(true);
                break;

            case GameModeManager.GameMode.DriftRush:
                coverageUIContainer.SetActive(false);
                tugOfWarUIContainer.SetActive(false);
                zonesUIContainer.SetActive(false);
                driftRushUIContainer.SetActive(true);
                freePlayUIContainer.SetActive(false);

                timerText.gameObject.SetActive(true);
                break;

            case GameModeManager.GameMode.FreePlay:
                coverageUIContainer.SetActive(false);
                tugOfWarUIContainer.SetActive(false);
                zonesUIContainer.SetActive(false);
                driftRushUIContainer.SetActive(false);
                freePlayUIContainer.SetActive(true);

                timerText.gameObject.SetActive(false);
                break;
        }

        endScreen.SetActive(false);
        UpdateUI();
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public void UpdateUI()
    {

        switch (GameModeManager.instance.gameMode)
        {
            case GameModeManager.GameMode.Coverage:
                teamOneCoverageBar.fillAmount = GameModeManager.instance.TeamOneScore / 100;
                teamTwoCoverageBar.fillAmount = GameModeManager.instance.TeamTwoScore / 100;

                teamOneCoverageScoreText.text = GameModeManager.instance.TeamOneScore.ToString("F0") + "%";
                teamTwoCoverageScoreText.text = GameModeManager.instance.TeamTwoScore.ToString("F0") + "%";

                if (GameModeManager.instance.TeamOneScore > GameModeManager.instance.TeamTwoScore)
                {
                    // This moves team 2's bar to the front of the render queue, ensuring it is rendered behind team one's bar.
                    teamTwoCoverageBar.transform.SetAsFirstSibling();
                }
                else
                {
                    // This moves team 1's bar to the front of the render queue, ensuring it is rendered behind team one's bar.
                    teamOneCoverageBar.transform.SetAsFirstSibling();
                }

                break;


            case GameModeManager.GameMode.TugOfWar:

                float relativeScore = (GameModeManager.instance.TeamOneScore - GameModeManager.instance.TeamTwoScore);
                // Remaps the relative score float to a value between 0 and 1
                teamOneTugOfWarBar.fillAmount = Mathf.Lerp(0, 1, Mathf.InverseLerp(-GameModeManager.instance.TugOfWarRelativeWinPercent,
                    GameModeManager.instance.TugOfWarRelativeWinPercent, relativeScore));
                break;


            case GameModeManager.GameMode.Zones:
                
                break;


            case GameModeManager.GameMode.DriftRush:
                
                break;
        }

    }


    /// <summary>
    /// Updates the on screen timer to show the specified remaining time.
    /// </summary>
    /// <param name="time">Remaining time in seconds</param>
    public void UpdateTimer(float time)
    {
        float minutes = time / 60f;
        float seconds = time % 60;

        string minutesStr = Mathf.FloorToInt(minutes).ToString("F0");

        if (minutes < 10)
        {
            minutesStr = "0" + minutesStr;
        }


        string secondsStr = Mathf.FloorToInt(seconds).ToString("F0");

        if (seconds < 10)
        {
            secondsStr = "0" + secondsStr;
        }
        else if (seconds > 59.5f)
        {
            secondsStr = "00";
        }



        timerText.text = minutesStr + ":" + secondsStr;
    }


    /// <summary>
    /// Brings up a screen with some stats, replay options, and a message about the winner.
    /// </summary>
    public void ShowEndScreen()
    {
        endScreen.SetActive(true);

        if (GameModeManager.instance.gameMode == GameModeManager.GameMode.DriftRush)
        {
            if (GameModeManager.instance.WinningTeam == 1)
            {
                winnerHeading.text = "You Made It!";
                winnerHeading.color = GameModeManager.instance.colour_TeamOne;
            }
            else
            {
                winnerHeading.text = "Not Quite...";
                winnerHeading.color = GameModeManager.instance.colour_TeamTwo;
            }

            teamOneScoreText.text = GameModeManager.instance.TeamOneScore.ToString("F1") + "%";
            teamTwoScoreText.text = GameModeManager.instance.driftManager.TotalScore.ToString("F0") + " Points";

            teamOneScoreText.color = GameModeManager.instance.colour_TeamOne;
            teamTwoScoreText.color = GameModeManager.instance.colour_TeamOne;
        }
        else
        {
            if (GameModeManager.instance.WinningTeam == 1)
            {
                winnerHeading.text = "Team 1 Wins!";
                winnerHeading.color = GameModeManager.instance.colour_TeamOne;
            }
            else
            {
                winnerHeading.text = "Team 2 Wins!";
                winnerHeading.color = GameModeManager.instance.colour_TeamTwo;
            }

            teamOneScoreText.text = GameModeManager.instance.TeamOneScore.ToString("F1") + "%";
            teamTwoScoreText.text = GameModeManager.instance.TeamTwoScore.ToString("F1") + "%";

            teamOneScoreText.color = GameModeManager.instance.colour_TeamOne;
            teamTwoScoreText.color = GameModeManager.instance.colour_TeamTwo;
        }
        
        
    }


}
