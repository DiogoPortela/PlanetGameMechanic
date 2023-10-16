using pt.dportela.PlanetGame.PlanetGeneration;
using pt.dportela.PlanetGame.Utils;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

namespace pt.dportela.PlanetGame.Controls
{
    public class CameraController : SingletonBehaviour<CameraController>
    {
        public Camera TargetCamera;
        public Map TargetMap;
        public bool InvertVertical;
        public bool InvertHorizontal;
        public bool InvertZoom;
        public float SlerpSensitivity = 1.0f;

        [Space(10)]
        public bool CanPan = true;
        public float PanningSensitivity = 1.0f;
        private bool isPanning;
        private Vector2 onMouseClickPosition;

        [Space(10)]
        public bool CanZoom = true;
        public float ZoomSensitivity = 1.0f;
        private float zoomDelta;

        private Transform cameraPositionHelper;

        private void Start()
        {
            cameraPositionHelper = new GameObject().transform;
            cameraPositionHelper.name = "cameraPositionHelper";
            cameraPositionHelper.SetParent(transform);
            cameraPositionHelper.position = TargetCamera.transform.position;
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
            Destroy(cameraPositionHelper);
        }

        // Panning logic
        public void OnPanClick(CallbackContext callbackContext)
        {
            if (callbackContext.started && CanPan)
            {
                isPanning = true;
                onMouseClickPosition = Mouse.current.position.ReadValue();
                Debug.Log($"CameraController OnPanClick true mousePosition={onMouseClickPosition}");
            }
            else if (isPanning && callbackContext.canceled)
            {
                // Clear the panning info.
                isPanning = false;
                onMouseClickPosition = Vector2.zero;
                Debug.Log($"CameraController OnPanClick false mousePosition={onMouseClickPosition}");
            }
        }
        private void ProcessPan()
        {
            if (!isPanning || !CanPan) { return; }

            var currentMousePosition = Mouse.current.position.ReadValue();
            var mouseDelta = currentMousePosition - onMouseClickPosition;
            //Debug.Log($"CameraController ProcessPan mouseDelta={mouseDelta}");

            if (InvertVertical) { mouseDelta.y *= -1; }
            if (InvertHorizontal) { mouseDelta.x *= -1; }

            cameraPositionHelper.RotateAround(TargetMap.transform.position, cameraPositionHelper.transform.right, mouseDelta.y * PanningSensitivity * Time.deltaTime);
            cameraPositionHelper.RotateAround(TargetMap.transform.position, cameraPositionHelper.transform.up, mouseDelta.x * PanningSensitivity * Time.deltaTime);
        }

        // Zoom logic
        public void OnZoomClick(CallbackContext callbackContext)
        {
            if(callbackContext.started && CanZoom)
            {
                zoomDelta += ZoomSensitivity * callbackContext.ReadValue<float>() * (InvertZoom? 1 : -1);
            }
        }
        private void ProcessZoom()
        {
            if (zoomDelta == 0) { return; }

            var cameraToPlanetVector = cameraPositionHelper.transform.position - TargetMap.transform.position;
            cameraToPlanetVector = cameraToPlanetVector + cameraToPlanetVector.normalized * zoomDelta;
            zoomDelta = 0;

            cameraPositionHelper.transform.position = TargetMap.transform.position + cameraToPlanetVector;
            Debug.DrawLine(TargetMap.transform.position, TargetMap.transform.position + cameraToPlanetVector, Color.red, Time.deltaTime);
        }

        // Alinning logic
        public void OnAlignVerticalClick(CallbackContext callbackContext)
        {
            cameraPositionHelper.LookAt(TargetMap.transform);
        }

        private void LerpCameraPositionToHelper()
        {
            TargetCamera.transform.position = Vector3.Slerp(TargetCamera.transform.position, cameraPositionHelper.position, SlerpSensitivity * Time.deltaTime);
            TargetCamera.transform.rotation = Quaternion.Slerp(TargetCamera.transform.rotation, cameraPositionHelper.rotation, SlerpSensitivity * Time.deltaTime);
        }

        private void Update()
        {
            Debug.DrawRay(cameraPositionHelper.transform.position, cameraPositionHelper.transform.right, Color.red, Time.deltaTime);
            Debug.DrawRay(cameraPositionHelper.transform.position, cameraPositionHelper.transform.up, Color.red, Time.deltaTime);

            ProcessZoom();
            ProcessPan();

            LerpCameraPositionToHelper();
        }
    }
}

