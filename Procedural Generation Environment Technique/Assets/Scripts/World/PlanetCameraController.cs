using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetCameraController : MonoBehaviour
{
    public float mouseDragSensitivity = 1.0f;
    public float mouseZoomSensitivity = 1.0f;

    public float minZoom = 2.0f;
    public float maxZoom = 10.0f;

    Vector3 preMousePosition;
    float middleZoom;
    public Camera cam;

    private void Start()
    {
        middleZoom = Mathf.Lerp(minZoom, maxZoom, 0.5f);
        cam.clearFlags = CameraClearFlags.SolidColor;
    }

    void Update()
    {
        Vector3 currentMousePosition = Input.mousePosition;

        if (Input.GetMouseButtonDown(0))
        {
            preMousePosition = currentMousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 mouseDisplacement = currentMousePosition - preMousePosition;
            mouseDisplacement.x /= Screen.width;
            mouseDisplacement.y /= Screen.height;
            Quaternion yaw = Quaternion.AngleAxis(mouseDisplacement.x * mouseDragSensitivity, transform.up);
            Quaternion pitch = Quaternion.AngleAxis(mouseDisplacement.y * mouseDragSensitivity, -transform.right);
            transform.localRotation = yaw * pitch * transform.localRotation;
        }

        float mouseWheelInput = Input.GetAxis("Mouse ScrollWheel");

        if (mouseWheelInput != 0.0f)
        {
            middleZoom += mouseZoomSensitivity * mouseWheelInput;
            middleZoom = Mathf.Clamp(middleZoom, minZoom, maxZoom);
        }

        transform.position = transform.forward * -middleZoom;
        preMousePosition = currentMousePosition;
    }
}
