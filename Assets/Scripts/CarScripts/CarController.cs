using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    Rigidbody rb;
    public WheelColliders wheelColliders;
    public WheelMeshs wheelMeshs;
    public float throttleInput;
    public float steeringInput;
    public AnimationCurve steeringCurve;

    public float motorPower = 500;
    float speed;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        speed = rb.velocity.magnitude;
        CheckInput();
        ApplyMotorTorque();
        ApplyWheelPositions();
        ApplySteering();
    }

    void CheckInput()
    {
        throttleInput = Input.GetAxis("Vertical");
        steeringInput = Input.GetAxis("Horizontal");
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
