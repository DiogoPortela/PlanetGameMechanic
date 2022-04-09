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

    private Vector3 targetPosition;                             //Represents the resulting position the lerp should lead to, after panning and zooming. 
    private Quaternion targetRotation;                          //Represents an angle to the desired point in space relative to the planet, we want the camera to be in. Note that this is due to lerping.
    private Vector3 planetToCameraNormalizedVector;             //Represents the normalized vector from the planet to the camera. It's just a direction.

    private Vector3 onMouseclickMousePosition;                  //Mouse position the moment we clicked the screen.
    private Vector2 currentMousePositionOffset;                 //The current offset of the mouse position, to the moment we clicked the screen.

    private float interpolationIndex = 0.0f;


    private float zoomMove = 51.0f;
    private float zoomSpeed = 0.0f;
    private float sizeOfPlanet = 50.0f;

    private void Start()
    {
        targetPosition = transform.position;
        planetToCameraNormalizedVector = (this.transform.position - focusedPlanet.transform.position).normalized;
        //targetRotation = Quaternion.Euler(planetToCameraNormalizedVector);
        //targetDistance = Vector3.Distance(this.transform.position, focusedPlanet.transform.position);
    }

    private Quaternion currentRotation;
    void Update()
    {
        CalculateZoom();
        CalculatePan();
        ApplyPosition();

        DebugDraw();
    }

    private void ApplyPosition()
    {
        //if (this.transform.position != targetPosition && interpolationIndex <= 1)
        {
            interpolationIndex += 0.5f * Time.deltaTime;
            currentRotation = Quaternion.Slerp(Quaternion.identity, Quaternion.FromToRotation(planetToCameraNormalizedVector, targetPosition - focusedPlanet.transform.position), interpolationIndex);
            this.transform.position = currentRotation * planetToCameraNormalizedVector * zoomMove; //Vector3.Lerp(this.transform.position, targetPosition, 0.025f);
            transform.LookAt(focusedPlanet.transform.position, this.transform.up);

            planetToCameraNormalizedVector = (this.transform.position - focusedPlanet.transform.position).normalized;
        }
    }

    private void CalculatePan()
    {

        if (Input.GetMouseButtonDown(2))
        {
            onMouseclickMousePosition = Input.mousePosition;
            interpolationIndex = 0;
        }
        else if (Input.GetMouseButton(2))
        {

            /// It begins by calculating the offset to the position saved during the first mouse down frame.
            /// Then it calculates a rotation around the right vector, and around the up vector, these are relative to the camera.
            /// Finally it calculates the target end position for this movement.

            currentMousePositionOffset = Input.mousePosition - onMouseclickMousePosition;

            targetRotation = Quaternion.AngleAxis(-currentMousePositionOffset.y * rotationSensitivity * Time.deltaTime, this.transform.right) 
                                       * Quaternion.AngleAxis(currentMousePositionOffset.x * rotationSensitivity * Time.deltaTime, this.transform.up);

            targetPosition = focusedPlanet.transform.position + targetRotation * planetToCameraNormalizedVector;
        }
    }

    private void CalculateZoom()
    {
        zoomMove += Input.GetAxis("Mouse ScrollWheel");
        zoomMove = Mathf.Clamp(zoomMove, sizeOfPlanet, sizeOfPlanet + 50.0f);
    }

    private void DebugDraw()
    {
        Debug.DrawRay(focusedPlanet.transform.position, targetPosition * sizeOfPlanet * 2.0f, Color.red, Time.deltaTime);
        Debug.DrawRay(focusedPlanet.transform.position, planetToCameraNormalizedVector * sizeOfPlanet * 2.0f, Color.blue, Time.deltaTime);
        Debug.DrawRay(focusedPlanet.transform.position, currentRotation * planetToCameraNormalizedVector * sizeOfPlanet * 2.0f, Color.green, Time.deltaTime);

        //Debug.DrawRay(focusedPlanet.transform.position, Quaternion.FromToRotation(planetToCameraVector, targetPosition - focusedPlanet.transform.position) * planetToCameraVector * 1.0f, Color.cyan, Time.deltaTime);
        //Debug.DrawRay(focusedPlanet.transform.position, Quaternion.identity * planetToCameraVector * 1.0f, Color.cyan, Time.deltaTime);
    }

    private void old_CalculateMove()
    {
        /*
        if (Input.GetMouseButtonDown(2))
        {
            onMouseclickMousePosition = Input.mousePosition;
        }
        else if (Input.GetMouseButton(2))
        {
            Vector2 mousePositionOffset = Input.mousePosition - onMouseclickMousePosition;
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

            onMouseclickMousePosition = Input.mousePosition;
        }
        */
    }
    private void old_CalculateZoom()
    {
        /*
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
        */
    }
}
