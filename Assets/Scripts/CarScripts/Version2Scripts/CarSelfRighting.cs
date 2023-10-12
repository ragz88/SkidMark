using System;
using UnityEngine;

namespace UnityStandardAssets.Vehicles.Car
{
    public class CarSelfRighting : MonoBehaviour
    {
        // Automatically put the car the right way up, if it has come to rest upside-down.
        [SerializeField] private float m_WaitTime = 3f;           // time to wait before self righting
        [SerializeField] private float m_VelocityThreshold = 1f;  // the velocity below which the car is considered stationary for self-righting
        
        private WheelCollider[] wheels;                           // reference to the position of each of the wheels

        private float m_LastOkTime; // the last time that the car was in an OK state
        private float m_MultipleResetDelay = 3f;                  // How long to delay a second reset if the first one is not effective.          
        private Rigidbody m_Rigidbody;


        private void Start()
        {
            m_Rigidbody = GetComponent<Rigidbody>();
            wheels = GetComponentsInChildren<WheelCollider>();
        }


        private void Update()
        {
            int wheelsGrounded = 0;      // will be used to assess whether the car is safely grounded or stuck. 3 wheels must touch the ground.

            for (int i  = 0; i < wheels.Length; i++)
            {
                Ray ray = new Ray((wheels[i].transform.position), -1 * wheels[i].transform.up);
                RaycastHit hit;

                Debug.DrawLine(wheels[i].transform.position, wheels[i].transform.position - (wheels[i].radius * 1.1f * wheels[i].transform.up), Color.red);

                // We cast a ray from the centre of each wheel downward. The ray is slightly longer than the radius of the wheel.
                if (Physics.Raycast(ray, out hit, (wheels[i].radius * 1.1f) ))
                {
                    wheelsGrounded++;
                }
            }
            
            // is the car is the right way up
            if ( (transform.up.y > 0.8f || m_Rigidbody.velocity.magnitude > m_VelocityThreshold) && wheelsGrounded >= 3)
            {
                m_LastOkTime = Time.time;
            }

            if (Time.time > m_LastOkTime + m_WaitTime)
            {
                RightCar();
            }

            //if (Input.GetKeyDown(KeyCode.R))
            //{
            //    RightCar();
            //}
        }


        // put the car back the right way up:
        private void RightCar()
        {
            // set the correct orientation for the car, and lift it off the ground a little
            transform.rotation = Quaternion.LookRotation(transform.forward);
            transform.position = transform.position + 5 * transform.up + 2 * transform.forward + 2 * transform.right;
            
            m_LastOkTime = Time.time + m_MultipleResetDelay;
        }
    }
}
