using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    private List<PlayerInput> players = new List<PlayerInput>();
    [SerializeField] private List<Transform> startingPoints;
    private PlayerInputManager playerInputManager;

    private void Awake()
    {
        playerInputManager = FindObjectOfType<PlayerInputManager>();
    }

    private void OnEnable()
    {
        playerInputManager.onPlayerJoined += AddPlayer;
    }

    private void OnDisable()
    {
        playerInputManager.onPlayerLeft -= AddPlayer;
    }

    private void AddPlayer(PlayerInput player)
    {
        players.Add(player);
        player.transform.position = startingPoints[players.Count - 1].position;
        player.transform.rotation = startingPoints[players.Count - 1].rotation;

        // Implies odd number
        if (players.Count % 2 == 1)
        {
            player.gameObject.tag = "Team1Car";
            player.gameObject.GetComponentInChildren<DemolitionTrigger>().teamNumber = 1;
            player.GetComponent<DriftPainter>().scoreCapsule.tag = "Team1";
        }
        else
        {
            player.gameObject.tag = "Team2Car";
            player.gameObject.GetComponentInChildren<DemolitionTrigger>().teamNumber = 2;
            player.GetComponent<DriftPainter>().scoreCapsule.tag = "Team2";
        }

        GameModeManager.instance.AddNewPlayer(player.GetComponent<DriftPainter>());
        GameModeManager.instance.PlayTime = 0;
    }
}
