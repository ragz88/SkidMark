using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DemolitionTrigger : MonoBehaviour
{
    [HideInInspector] public int teamNumber = 1;

    [SerializeField] float velocityThreshhold = 2;
    [SerializeField] Rigidbody carBody;

    private void OnTriggerEnter(Collider other)
    {
        if (teamNumber == 1)
        {
            if (other.CompareTag("Team2Car"))
            {
                if (carBody.velocity.magnitude >= velocityThreshhold)
                {
                    DemolisionController demoController = other.GetComponent<DemolisionController>();
                    if (demoController != null)
                    {
                        demoController.DemolishCar();
                    }
                }

            }
        }
        else if (teamNumber == 2)
        {
            if (other.CompareTag("Team1Car"))
            {
                if (carBody.velocity.magnitude >= velocityThreshhold)
                {
                    DemolisionController demoController = other.GetComponent<DemolisionController>();
                    if (demoController != null)
                    {
                        demoController.DemolishCar();
                    }
                }
            }
        }
        
    }

}
