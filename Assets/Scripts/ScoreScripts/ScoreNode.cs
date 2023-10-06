using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Vehicles.Car;

public class ScoreNode : MonoBehaviour
{
    /// <summary>
    /// The team that currently controls this node. 0 if it has yet to be painted.
    /// </summary>
    private int controllingTeam = 0;


    /// <summary>
    /// The team that currently controls this node. 0 if it has yet to be painted.
    /// </summary>
    public int ControllingTeam
    {
        get { return controllingTeam; }
        set 
        {
            if (value == 0 || value == 1 || value == 2)
            {
                controllingTeam = value;
            }
            else
            {
                Debug.LogWarning("Tried to set score node team to value out of standard range. Only 0, 1, and 2 allowed");
            }
        }
    }
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDrawGizmos()
    {
        if (GameModeManager.instance.showDebugNodes)
        {
            Color teamColour;

            if (ControllingTeam == 1)
            {
                teamColour = GameModeManager.instance.colour_TeamOne;
            }
            else if (ControllingTeam == 2)
            {
                teamColour = GameModeManager.instance.colour_TeamTwo;
            }
            else
            {
                teamColour = Color.white;
            }

            Gizmos.color = teamColour;
            Gizmos.DrawSphere(transform.position, 1f);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Team1"))
        {
            if (controllingTeam == 2)
            {
                ColourReplaced(other.transform);
            }

            controllingTeam = 1;
        }
        else if (other.transform.CompareTag("Team2"))
        {
            if (controllingTeam == 1)
            {
                ColourReplaced(other.transform);
            }

            controllingTeam = 2;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.CompareTag("Team1"))
        {
            if (controllingTeam == 2)
            {
                ColourReplaced(other.transform);
            }

            controllingTeam = 1;
        }
        else if (other.transform.CompareTag("Team2"))
        {
            if (controllingTeam == 1)
            {
                ColourReplaced(other.transform);
            }

            controllingTeam = 2;
        }
    }


    void ColourReplaced(Transform carTransform)
    {
        carTransform.GetComponentInParent<CarController>().AddPaintReplaceNitros();
    }

}
