using UnityEngine;
using Utils;

public class CameraController : SingletonBehaviour<CameraController>
{
    public float rotationSpeed = 50;
    public float zoomSpeed = 50;
    public KeyCode moveLeft;
    public KeyCode moveRight;
    public KeyCode moveUp;
    public KeyCode moveDown;

    public PlanetPlaneMesh focusedPlanet;

    void Update()
    {
        float horizontalMove = (Input.GetKey(moveLeft) ? 1 : 0) + (Input.GetKey(moveRight) ? -1 : 0);
        if (horizontalMove != 0)
            this.transform.RotateAround(focusedPlanet.transform.position, Vector3.up, horizontalMove * rotationSpeed * Time.deltaTime);

        float verticalMove = (Input.GetKey(moveUp) ? 1 : 0) + (Input.GetKey(moveDown) ? -1 : 0);
        if (verticalMove != 0)
            this.transform.RotateAround(focusedPlanet.transform.position, this.transform.right, verticalMove * rotationSpeed * Time.deltaTime);

        float zoomMove = Input.GetAxis("Mouse ScrollWheel");
        if (zoomMove != 0)
            this.transform.position += this.transform.forward * zoomMove * zoomSpeed * Time.deltaTime;

        this.transform.LookAt(focusedPlanet.transform.position);
    }
}
