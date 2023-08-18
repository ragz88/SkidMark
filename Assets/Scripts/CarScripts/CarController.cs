using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    Rigidbody rb;
    public WheelColliders wheelColliders;
    public WheelMeshs wheelMeshs;
    public WheelParticles wheelParticles;
    public float throttleInput;
    public float brakeInput;
    public float steeringInput;
    public AnimationCurve steeringCurve;
    public GameObject smokePrefab;
    public float motorPower = 500;
    public float brakePower = 50000;
    public float slipAllowance = 0.3f;
    float slipAngle;
    float speed;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        InstantiateSmoke();
    }

    void Update()
    {
        speed = rb.velocity.magnitude;
        CheckInput();
        ApplyMotorTorque();
        ApplySteering();
        ApplyBrake();
        CheckParticles();
        ApplyWheelPositions();
    }

    void InstantiateSmoke()
    {
        wheelParticles.FLWheel = Instantiate(smokePrefab, wheelColliders.FLWheel.transform.position - Vector3.up * wheelColliders.FLWheel.radius, Quaternion.identity, wheelColliders.FLWheel.transform).GetComponent<ParticleSystem>();
        wheelParticles.FRWheel = Instantiate(smokePrefab, wheelColliders.FRWheel.transform.position - Vector3.up * wheelColliders.FRWheel.radius, Quaternion.identity, wheelColliders.FRWheel.transform).GetComponent<ParticleSystem>();
        wheelParticles.RLWheel = Instantiate(smokePrefab, wheelColliders.RLWheel.transform.position - Vector3.up * wheelColliders.RLWheel.radius, Quaternion.identity, wheelColliders.RLWheel.transform).GetComponent<ParticleSystem>();
        wheelParticles.RRWheel = Instantiate(smokePrefab, wheelColliders.RRWheel.transform.position - Vector3.up * wheelColliders.RRWheel.radius, Quaternion.identity, wheelColliders.RRWheel.transform).GetComponent<ParticleSystem>();
    }
    void CheckInput()
    {
        throttleInput = Input.GetAxis("Vertical");
        steeringInput = Input.GetAxis("Horizontal");
        slipAngle = Vector3.Angle(transform.forward, rb.velocity - transform.forward);
        if (slipAngle < 120f)
        {
            if (throttleInput < 0)
            {
                brakeInput = Mathf.Abs(throttleInput);
                throttleInput = 0;
            }

        }
        else
        {
            brakeInput = 0;
        }
    }

    void ApplyMotorTorque()
    {
        //These are the driven wheels. This particular car is rear wheel drive but that can be easily altered by changing the wheels that recieve torque. 
        wheelColliders.RLWheel.motorTorque = motorPower * throttleInput;
        wheelColliders.RRWheel.motorTorque = motorPower * throttleInput;
    }

    void ApplyWheelPositions()
    {
        UpdateWheelPosition(wheelColliders.FLWheel,wheelMeshs.FLWheel);
        UpdateWheelPosition(wheelColliders.FRWheel, wheelMeshs.FRWheel);
        UpdateWheelPosition(wheelColliders.RLWheel, wheelMeshs.RLWheel);
        UpdateWheelPosition(wheelColliders.RRWheel, wheelMeshs.RRWheel);
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

    void ApplyBrake()
    {
        wheelColliders.FLWheel.brakeTorque = brakePower * brakeInput * 0.7f;  
        wheelColliders.FRWheel.brakeTorque = brakePower * brakeInput * 0.7f;

        wheelColliders.RLWheel.brakeTorque = brakePower * brakeInput * 0.3f;
        wheelColliders.RRWheel.brakeTorque = brakePower * brakeInput * 0.3f;
    }

    void ApplySteering()
    {
        float steeringAngle = steeringInput*steeringCurve.Evaluate(speed);
        wheelColliders.FRWheel.steerAngle = steeringAngle;
        wheelColliders.FLWheel.steerAngle = steeringAngle;
    }
    void UpdateWheelPosition(WheelCollider collider, GameObject wheelMesh)
    {
        Quaternion quaternion;
        Vector3 position;
        collider.GetWorldPose(out position, out quaternion);
        wheelMesh.transform.position = position;
        wheelMesh.transform.rotation = quaternion;
    }

}
[System.Serializable]
public class WheelColliders
{
    public WheelCollider FLWheel;
    public WheelCollider FRWheel;
    public WheelCollider RLWheel;
    public WheelCollider RRWheel;
}
[System.Serializable]
public class WheelMeshs
{
    public GameObject FLWheel;
    public GameObject FRWheel;
    public GameObject RLWheel;
    public GameObject RRWheel;
}

[System.Serializable]
public class WheelParticles
{
    public ParticleSystem FLWheel;
    public ParticleSystem FRWheel;
    public ParticleSystem RLWheel;
    public ParticleSystem RRWheel;
}
