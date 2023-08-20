using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CarSystemController : MonoBehaviour
{
    private Rigidbody rb;
    public float steeringInput, throttleInput, brakeInput;
    private float currentSteerAngle;
    private float frontBias;
    public float speed;
    private float slipAngle;
    //Wheels
    [SerializeField] private CarWheelColliders wheelColliders;
    [SerializeField] private CarWheelMeshs wheelMeshs;
    [SerializeField] private CarWheelParticles wheelParticles;

    //Tire Smoke
    [SerializeField] private GameObject smokePrefab;

    // Settings
    [SerializeField] private bool autoCountersteer = true;
    [SerializeField] private float motorForce = 1000f, slipAllowance = 0.3f, brakePower = 3000;
    [SerializeField] [Range(0, 1)] private float rearBias = 0.7f;
    [SerializeField] private AnimationCurve steeringCurve;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        InstantiateSmoke();
    }

    void InstantiateSmoke()
    {
        wheelParticles.FLWheel = Instantiate(smokePrefab, wheelColliders.FLWheel.transform.position - Vector3.up * wheelColliders.FLWheel.radius, Quaternion.identity, wheelColliders.FLWheel.transform).GetComponent<ParticleSystem>();
        wheelParticles.FRWheel = Instantiate(smokePrefab, wheelColliders.FRWheel.transform.position - Vector3.up * wheelColliders.FRWheel.radius, Quaternion.identity, wheelColliders.FRWheel.transform).GetComponent<ParticleSystem>();
        wheelParticles.RLWheel = Instantiate(smokePrefab, wheelColliders.RLWheel.transform.position - Vector3.up * wheelColliders.RLWheel.radius, Quaternion.identity, wheelColliders.RLWheel.transform).GetComponent<ParticleSystem>();
        wheelParticles.RRWheel = Instantiate(smokePrefab, wheelColliders.RRWheel.transform.position - Vector3.up * wheelColliders.RRWheel.radius, Quaternion.identity, wheelColliders.RRWheel.transform).GetComponent<ParticleSystem>();
    }

    private void FixedUpdate()
    {
        frontBias = 1f - rearBias;
        speed = rb.velocity.magnitude;
        GetInput();
        HandleMotor();
        HandleSteering();
        ApplyBrake();
        CheckParticles();
        UpdateWheels();
    }

    private void GetInput()
    {
        steeringInput = Input.GetAxis("Horizontal");
        throttleInput = Input.GetAxis("vertical");

        slipAngle = Vector3.Angle(transform.forward, rb.velocity - transform.forward);

        float movingDirection = Vector3.Dot(transform.forward, rb.velocity);
        if (movingDirection < -0.5f && throttleInput > 0)
        {
            brakeInput = Mathf.Abs(throttleInput);
        }
        else if (movingDirection > 0.5f && throttleInput < 0)
        {
            brakeInput = Mathf.Abs(throttleInput);
        }
        else
        {
            brakeInput = 0;
        }
    }

    private void HandleMotor()
    {
        wheelColliders.FLWheel.motorTorque = throttleInput * motorForce * frontBias;
        wheelColliders.FRWheel.motorTorque = throttleInput * motorForce * frontBias;
        wheelColliders.RLWheel.motorTorque = throttleInput * motorForce * rearBias;
        wheelColliders.RRWheel.motorTorque = throttleInput * motorForce * rearBias;
    }


    private void HandleSteering()
    {
        currentSteerAngle = steeringInput * steeringCurve.Evaluate(speed);
        if(autoCountersteer)
        {
            if (slipAngle < 120f)
            {
                currentSteerAngle += Vector3.SignedAngle(transform.forward, rb.velocity + transform.forward, Vector3.up);
            }
        }
        currentSteerAngle = Mathf.Clamp(currentSteerAngle, -75f, 75f);
        wheelColliders.FRWheel.steerAngle = currentSteerAngle;
        wheelColliders.FLWheel.steerAngle = currentSteerAngle;
    }

    void ApplyBrake()
    {
        wheelColliders.FLWheel.brakeTorque = brakePower * brakeInput * 0.7f;
        wheelColliders.FRWheel.brakeTorque = brakePower * brakeInput * 0.7f;
        wheelColliders.RLWheel.brakeTorque = brakePower * brakeInput * 0.3f;
        wheelColliders.RRWheel.brakeTorque = brakePower * brakeInput * 0.3f;
    }

    void CheckParticles()
    {
        WheelHit[] wheelhits = new WheelHit[4];
        wheelColliders.FLWheel.GetGroundHit(out wheelhits[0]);
        wheelColliders.FRWheel.GetGroundHit(out wheelhits[1]);
        wheelColliders.RLWheel.GetGroundHit(out wheelhits[2]);
        wheelColliders.RRWheel.GetGroundHit(out wheelhits[3]);

        if ((MathF.Abs(wheelhits[0].sidewaysSlip) + Mathf.Abs(wheelhits[0].forwardSlip)) > slipAllowance)
        {
            wheelParticles.FLWheel.Play();
        }
        else
        {
            wheelParticles.FLWheel.Stop();
        }

        if ((MathF.Abs(wheelhits[1].sidewaysSlip) + Mathf.Abs(wheelhits[1].forwardSlip)) > slipAllowance)
        {
            wheelParticles.FRWheel.Play();
        }
        else
        {
            wheelParticles.FRWheel.Stop();
        }

        if ((MathF.Abs(wheelhits[2].sidewaysSlip) + Mathf.Abs(wheelhits[2].forwardSlip)) > slipAllowance)
        {
            wheelParticles.RLWheel.Play();
        }
        else
        {
            wheelParticles.RLWheel.Stop();
        }

        if ((MathF.Abs(wheelhits[3].sidewaysSlip) + Mathf.Abs(wheelhits[3].forwardSlip)) > slipAllowance)
        {
            wheelParticles.RRWheel.Play();
        }
        else
        {
            wheelParticles.RRWheel.Stop();
        }
    }

    private void UpdateWheels()
    {
        UpdateSingleWheel(wheelColliders.FLWheel, wheelMeshs.FLWheel);
        UpdateSingleWheel(wheelColliders.FRWheel, wheelMeshs.FRWheel);
        UpdateSingleWheel(wheelColliders.RLWheel, wheelMeshs.RLWheel);
        UpdateSingleWheel(wheelColliders.RRWheel, wheelMeshs.RRWheel);
    }

    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.rotation = rot;
        wheelTransform.position = pos;
    }
}
[System.Serializable]
public class CarWheelColliders
{
    public WheelCollider FLWheel;
    public WheelCollider FRWheel;
    public WheelCollider RLWheel;
    public WheelCollider RRWheel;
}
[System.Serializable]
public class CarWheelMeshs
{
    public Transform FLWheel;
    public Transform FRWheel;
    public Transform RLWheel;
    public Transform RRWheel;
}

[System.Serializable]
public class CarWheelParticles
{
    public ParticleSystem FLWheel;
    public ParticleSystem FRWheel;
    public ParticleSystem RLWheel;
    public ParticleSystem RRWheel;
}