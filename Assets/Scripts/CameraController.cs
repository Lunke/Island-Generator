using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float rotateSpeed = 100f;
    public float rotateDistance = 100f;
    public float panSpeed = 100f;
    public float zoomSpeed = 100f;

    private Vector3 prevMousePosition;


    void Update()
    {

        if (Input.GetMouseButton(0))
        {
            Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - prevMousePosition);
            transform.RotateAround(transform.position + transform.forward * rotateDistance, -transform.right, pos.y * rotateSpeed);
            transform.RotateAround(transform.position + transform.forward * rotateDistance, Vector3.up, pos.x * rotateSpeed);
        }

        if (Input.GetMouseButton(1))
        {
            Vector3 pos = Camera.main.ScreenToViewportPoint(prevMousePosition - Input.mousePosition);
            Vector3 move = new Vector3(pos.x * panSpeed, pos.y * panSpeed, 0);
            transform.Translate(move, Space.Self);
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0.0f)
        {
            Vector3 move = scroll * zoomSpeed * transform.forward;
            transform.Translate(move, Space.World);
        }

        prevMousePosition = Input.mousePosition;
    }
}