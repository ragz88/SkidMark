using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityStandardAssets.Vehicles.Car;

public class NosFXManager : MonoBehaviour
{
    private CarController car;
    public TMP_Text nitrousMeter;
    public ParticleSystem[] nitrous;
    public AudioSource nosSound;
    public bool applyNosFX { get; set; }

void Start()
    {
        car = GetComponent<CarController>();
    }

    public void UpdateNos()
    {
        nitrousMeter.text = car.nitrousGaugePercentage.ToString("###,##0") + "%";
        if (applyNosFX )
        {
            if(!nosSound.isPlaying)
            nosSound.Play();

            foreach (ParticleSystem nosEffect in nitrous)
            {
                if(!nosEffect.isPlaying)
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
