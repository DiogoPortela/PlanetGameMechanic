using pt.dportela.PlanetGame.Utils;
using UnityEngine;

public class CameraController : SingletonBehaviour<CameraController>
{
    public float rotationSensitivity = 2.0f;
    public float zoomSensivity = 1.0f;
    [Range(3, 50)]
    public float maxZoom = 10.0f;
    [Range(1, 2)]
    public float minZoom = 1.1f;
    public GameObject focusedPlanet;

    private Vector3 targetZoom;
    private Vector3 previousMoveClickMousePosition;


    private void Start()
    {
        targetZoom = transform.position;
    }

    void Update()
    {
        CalculateMove();
        CalculateZoom(); 
        transform.LookAt(focusedPlanet.transform.position);
    }
    
    private void CalculateMove()
    {
        if (Input.GetMouseButtonDown(2))
        {
            previousMoveClickMousePosition = Input.mousePosition;
        }
        else if (Input.GetMouseButton(2))
        {
            Vector2 mousePositionOffset = Input.mousePosition - previousMoveClickMousePosition;
            float rotationSpeed = Vector3.Distance(transform.position, focusedPlanet.transform.position) * rotationSensitivity - 1.0f;

            if (mousePositionOffset.x != 0)
            {
                float zoomMagnitude = (targetZoom - focusedPlanet.transform.position).magnitude;
                transform.RotateAround(focusedPlanet.transform.position, Vector3.up, mousePositionOffset.x * rotationSpeed * Time.deltaTime);
                targetZoom = (transform.position - focusedPlanet.transform.position).normalized * zoomMagnitude;
            }
            if (mousePositionOffset.y != 0)
            {
                float zoomMagnitude = (targetZoom - focusedPlanet.transform.position).magnitude;
                transform.RotateAround(focusedPlanet.transform.position, transform.right, -mousePositionOffset.y * rotationSpeed * Time.deltaTime);

                //float verticalAngle = Vector3.Angle(focusedPlanet.transform.up, transform.forward);
                //if (verticalAngle)

                targetZoom = (transform.position - focusedPlanet.transform.position).normalized * zoomMagnitude;
            }

            previousMoveClickMousePosition = Input.mousePosition;
        }

    }
    private void CalculateZoom()
    {
        float zoomMove = Input.GetAxis("Mouse ScrollWheel");
        if (zoomMove != 0)
        {
            float zoomSpeed = Mathf.Clamp(Vector3.Distance(transform.position, focusedPlanet.transform.position) - 1.0f, 0, 50);
            Vector3 zoomDelta = transform.forward * zoomMove * zoomSensivity * zoomSpeed;

            float targetToPlanetDistance = Vector3.Distance(targetZoom + zoomDelta, focusedPlanet.transform.position);

            if (targetToPlanetDistance >= minZoom && targetToPlanetDistance <= maxZoom)
            {
                targetZoom += zoomDelta;
            }
        }

        if (transform.position != targetZoom)
            transform.position = Vector3.Lerp(transform.position, targetZoom, 0.025f);
    }
}
