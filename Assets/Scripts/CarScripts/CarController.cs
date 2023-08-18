using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public WheelColliders wheelColliders;
    public WheelMeshs wheelMeshs;
    void Start()
    {

    }
    void Update()
    {
        ApplyWheelPositions();
    }
    void ApplyWheelPositions()
    {
        UpdateWheelPosition(wheelColliders.FLWheel,wheelMeshs.FLWheel);
        UpdateWheelPosition(wheelColliders.FRWheel, wheelMeshs.FRWheel);
        UpdateWheelPosition(wheelColliders.RLWheel, wheelMeshs.RLWheel);
        UpdateWheelPosition(wheelColliders.RRWheel, wheelMeshs.RRWheel);
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
