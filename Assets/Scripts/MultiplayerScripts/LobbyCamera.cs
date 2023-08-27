using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class LobbyCamera : MonoBehaviour
{
    private PlayerInput players;
    private Camera lobbyCam;


    private void Awake()
    {
        lobbyCam = GetComponent<Camera>();
    }

    private void Update()
    {
        players = GameObject.FindObjectOfType<PlayerInput>();
        AdjustCamera();
    }

    private void AdjustCamera()
    {
        if(players)
        {
            lobbyCam.enabled = false;
        }
        else
        {
            lobbyCam.enabled = true;
        }


    }
}
