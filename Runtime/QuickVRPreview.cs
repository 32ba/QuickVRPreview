using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.XR;
using UnityEngine.XR.Management;

namespace QuickVRPreview
{
    public class QuickVRPreview : MonoBehaviour
    {
        [Header("Movement")]
        public float moveSpeed = 3f;
        public float turnSpeed = 90f;
        public float verticalSpeed = 2f;

        public bool IsRunning { get; private set; }

        private GameObject _cameraObject;
        private Camera _camera;

        private void Awake()
        {
            SetupCameraHierarchy();
        }

        private void SetupCameraHierarchy()
        {
            _cameraObject = new GameObject("VRCamera");
            _cameraObject.transform.SetParent(transform, false);

            _camera = _cameraObject.AddComponent<Camera>();
            _camera.nearClipPlane = 0.01f;

            var trackedPoseDriver = _cameraObject.AddComponent<TrackedPoseDriver>();
            trackedPoseDriver.positionAction = new UnityEngine.InputSystem.InputAction(
                "Position", binding: "<XRHMD>/centerEyePosition");
            trackedPoseDriver.rotationAction = new UnityEngine.InputSystem.InputAction(
                "Rotation", binding: "<XRHMD>/centerEyeRotation");

            _cameraObject.SetActive(false);
        }

        private void Update()
        {
            if (!IsRunning) return;
            HandleMovement();
        }

        private void HandleMovement()
        {
            var leftStick = Vector2.zero;
            var rightStick = Vector2.zero;

            var leftHandDevices = new List<InputDevice>();
            InputDevices.GetDevicesAtXRNode(XRNode.LeftHand, leftHandDevices);
            if (leftHandDevices.Count > 0)
                leftHandDevices[0].TryGetFeatureValue(CommonUsages.primary2DAxis, out leftStick);

            var rightHandDevices = new List<InputDevice>();
            InputDevices.GetDevicesAtXRNode(XRNode.RightHand, rightHandDevices);
            if (rightHandDevices.Count > 0)
                rightHandDevices[0].TryGetFeatureValue(CommonUsages.primary2DAxis, out rightStick);

            // Left stick: free movement relative to head direction
            var headForward = _cameraObject.transform.forward;
            var headRight = _cameraObject.transform.right;
            headForward.y = 0f;
            headRight.y = 0f;
            headForward.Normalize();
            headRight.Normalize();

            var move = headForward * leftStick.y + headRight * leftStick.x;
            transform.position += move * moveSpeed * Time.deltaTime;

            // Right stick X: smooth turn (yaw)
            transform.Rotate(0f, rightStick.x * turnSpeed * Time.deltaTime, 0f);

            // Right stick Y: vertical movement
            transform.position += Vector3.up * (rightStick.y * verticalSpeed * Time.deltaTime);
        }

        public void StartVR()
        {
            if (IsRunning) return;
            StartCoroutine(InitializeXR());
        }

        public void StopVR()
        {
            if (!IsRunning) return;

            _cameraObject.SetActive(false);

            var xrManager = XRGeneralSettings.Instance?.Manager;
            if (xrManager != null)
            {
                xrManager.StopSubsystems();
                xrManager.DeinitializeLoader();
            }

            IsRunning = false;
        }

        private IEnumerator InitializeXR()
        {
            var xrSettings = XRGeneralSettings.Instance;
            if (xrSettings == null)
            {
                Debug.LogError("[QuickVRPreview] XR General Settings not found. Please configure XR Plug-in Management.");
                yield break;
            }

            var xrManager = xrSettings.Manager;
            if (xrManager == null)
            {
                Debug.LogError("[QuickVRPreview] XR Manager not found.");
                yield break;
            }

            yield return xrManager.InitializeLoader();

            if (xrManager.activeLoader == null)
            {
                Debug.LogError("[QuickVRPreview] Failed to initialize XR Loader. Is OpenXR enabled and SteamVR running?");
                yield break;
            }

            xrManager.StartSubsystems();
            _cameraObject.SetActive(true);

            // Enable TrackedPoseDriver input actions
            var tpd = _cameraObject.GetComponent<TrackedPoseDriver>();
            if (tpd != null)
            {
                tpd.positionAction.Enable();
                tpd.rotationAction.Enable();
            }

            IsRunning = true;
            Debug.Log("[QuickVRPreview] VR started.");
        }

        private void OnDestroy()
        {
            if (IsRunning)
            {
                StopVR();
            }
        }
    }
}
