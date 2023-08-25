using System;
using UnityEngine;

namespace UnityStandardAssets.Vehicles.Car
{
    [RequireComponent(typeof (CarController))]
    public class CarUserControl : MonoBehaviour
    {
        private CarController m_Car; // the car controller we want to use
        private NosFXManager m_Nos; // the car controller we want to use

        private void Awake()
        {
            // get the car controller
            m_Car = GetComponent<CarController>();
            m_Nos = GetComponent<NosFXManager>();
        }


        private void FixedUpdate()
        {
            // pass the input to the car!
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("vertical");
            float handbrake = Input.GetAxis("Jump");
            float n = Input.GetAxis("Nos");
            m_Car.Move(h, v, v, handbrake, n);
            m_Nos.UpdateNos();

        }
    }
}
