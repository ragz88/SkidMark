using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityStandardAssets.Vehicles.Car;

public class NosManager : MonoBehaviour
{
    private CarController car;
    public TMP_Text nitrousMeter;
    public ParticleSystem[] nitrous;
    public AudioSource nosSound;

    void Start()
    {
        car = GetComponent<CarController>();
    }

    public void UpdateNos()
    {
        nitrousMeter.text = car.nitrousGaugePercentage.ToString("###,##0") + "%";
    }

    public void DisplayNosFX(float nosInput)
    {
        if(nosInput > 0)
        {
            nosSound.Play();
            foreach (ParticleSystem nosEffect in nitrous)
            {
                nosEffect.Play();
            }
        }
        else
        {
            nosSound.Stop();
            foreach (ParticleSystem nosEffect in nitrous)
            {
                nosEffect.Stop();
            }
        }
    }

        
}
