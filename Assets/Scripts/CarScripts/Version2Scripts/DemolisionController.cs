using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Vehicles.Car;


public class DemolisionController : MonoBehaviour
{
    [SerializeField] float demoDelay = 2;
    [SerializeField] GameObject demolitionEffectPrefab;
    [SerializeField] Renderer[] renderers;

    Collider[] colliders;
    Rigidbody carBody;
    CarUserControl carControl;


    float demoTimer = 0;
    bool demolished = false;
    
    // Start is called before the first frame update
    void Start()
    {
        carControl = GetComponent<CarUserControl>();
        carBody = GetComponent<Rigidbody>();
        colliders = GetComponentsInChildren<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (demolished)
        {
            if (demoTimer > 0)
            {
                demoTimer -= Time.deltaTime;
            }
            else
            {
                ResetCar();
            }
        }
    }


    void ResetCar()
    {
        demolished = false;

        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].enabled = true;
        }

        for (int i = 0; i < renderers.Length; i++)
        {
            //renderers[i].material.color = renderers[i].material.color - new Color(0, 0, 0, 1f);
            renderers[i].enabled = true;
        }

        carBody.useGravity = true;
        carControl.enabled = true;
    }


    /// <summary>
    /// Call from a demolitionCollider if it hits this car hard enough.
    /// </summary>
    public void DemolishCar()
    {
        demolished = true;
        demoTimer = demoDelay;

        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].enabled = false;
        }

        for (int i = 0; i < renderers.Length; i++)
        {
            //renderers[i].material.color = renderers[i].material.color - new Color(0, 0, 0, 0.5f);
            renderers[i].enabled = false;
        }

        carBody.useGravity = false;
        carControl.enabled = false;
        Instantiate(demolitionEffectPrefab, transform.position - Vector3.up, Quaternion.identity);
    }
}
