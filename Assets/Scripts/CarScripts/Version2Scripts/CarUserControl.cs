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
        private InputActionAsset inputAsset;
        private InputActionMap player;




        private void Awake()
        {
            m_Car = GetComponent<CarController>();
            m_Nos = GetComponent<NosFXManager>();
            inputAsset = GetComponent<PlayerInput>().actions;
            player = inputAsset.FindActionMap("Car");
        }


        private void FixedUpdate()
        {
            m_Car.Move(steeringInput, throttleInput, throttleInput, handBrakeInput, nosInput);
            m_Nos.UpdateNos();

        }


        private void OnEnable()
        {
            player.Enable();

            player.FindAction("Throttle").performed += ApplyThrottle;
            player.FindAction("Throttle").canceled += ReleaseThrottle;

            player.FindAction("Steering").performed += ApplySteering;
            player.FindAction("Steering").canceled += ReleaseSteering;

            player.FindAction("Handbrake").performed += ApplyHandbrake;
            player.FindAction("Handbrake").canceled += ReleaseHandbrake;

            player.FindAction("Boost").performed += ApplyBoost;
            player.FindAction("Boost").canceled += ReleaseBoost;
        }

        private void OnDisable()
        {
            player.Disable();
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
