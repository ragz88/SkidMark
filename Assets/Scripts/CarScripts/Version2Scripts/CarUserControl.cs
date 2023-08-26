using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UnityStandardAssets.Vehicles.Car
{
    [RequireComponent(typeof (CarController))]
    public class CarUserControl : MonoBehaviour
    {
        private CarController m_Car; 
        private NosFXManager m_Nos;
        private float throttleInput, steeringInput, handBrakeInput, nosInput;
        private CarInput input;




        private void Awake()
        {
            m_Car = GetComponent<CarController>();
            m_Nos = GetComponent<NosFXManager>();
            input = new CarInput();
        }


        private void FixedUpdate()
        {
            m_Car.Move(steeringInput, throttleInput, throttleInput, handBrakeInput, nosInput);
            m_Nos.UpdateNos();

        }


        private void OnEnable()
        {
            input.Enable();

            input.Car.Throttle.performed += ApplyThrottle;
            input.Car.Throttle.canceled += ReleaseThrottle;

            input.Car.Steering.performed += ApplySteering;
            input.Car.Steering.canceled += ReleaseSteering;

            input.Car.Handbrake.performed += ApplyHandbrake;
            input.Car.Handbrake.canceled += ReleaseHandbrake;

            input.Car.Boost.performed += ApplyBoost;
            input.Car.Boost.canceled += ReleaseBoost;
        }

        private void OnDisable()
        {
            input.Disable();
        }

        private void ApplyThrottle( InputAction.CallbackContext value)
        {
            throttleInput = value.ReadValue<float>();
        }
        private void ReleaseThrottle(InputAction.CallbackContext value)
        {
            throttleInput = 0;
        }

        private void ApplySteering(InputAction.CallbackContext value)
        {
            steeringInput = value.ReadValue<float>();
        }
        private void ReleaseSteering(InputAction.CallbackContext value)
        {
            steeringInput = 0;
        }

        private void ApplyHandbrake(InputAction.CallbackContext value)
        {
            handBrakeInput = value.ReadValue<float>();
        }
        private void ReleaseHandbrake(InputAction.CallbackContext value)
        {
            handBrakeInput = 0;
        }

        private void ApplyBoost(InputAction.CallbackContext value)
        {
            nosInput = value.ReadValue<float>();
        }
        private void ReleaseBoost(InputAction.CallbackContext value)
        {
            nosInput = 0;
        }

    }
}
