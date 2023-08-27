using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Vehicles.Car;

public class FuelPickup : MonoBehaviour
{
    [Tooltip("Between 0 and 100 - percent of fuel to restore.")]
    [SerializeField] float fuelAmount = 100;
    [SerializeField] float respawnTime = 7;

    Collider collider;
    Renderer rend;

    bool onCooldown = false;
    float timer = 0;

    private void Start()
    {
        collider = GetComponent<Collider>();
        rend = GetComponent<Renderer>();
    }

    private void Update()
    {
        if (onCooldown)
        {
            if (timer >= respawnTime)
            {
                collider.enabled = true;
                rend.enabled = true;
                onCooldown = false;
                timer = 0;
            }
            else
            {
                timer += Time.deltaTime;
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        CarController carControl;
        carControl = other.transform.GetComponent<CarController>();

        if (carControl != null)
        {
            if (carControl.currentNitros < carControl.NitrosCapacity)
            {
                carControl.SetCurrentNitros(carControl.currentNitros + ((fuelAmount / 100f) * carControl.NitrosCapacity));

                collider.enabled = false;
                rend.enabled = false;
                onCooldown = true;
            }
        }

        
    }
}
