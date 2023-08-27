using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class LobbyCamera : MonoBehaviour
{
    private PlayerInput players;
    private GameObject lobbyCam;


    private void Awake()
    {
        lobbyCam = this.gameObject;
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
            lobbyCam.SetActive(false);
        }
    }
}
