using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityStandardAssets.Vehicles.Car;

public class Controls : MonoBehaviour
{
    // Start is called before the first frame update
     CarController carController;
    void Start()
    {
        carController = GameObject.FindObjectOfType<CarController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(carController.CurrentSpeed > 1)
        {
            this.gameObject.SetActive(false);
        }
    }
}
