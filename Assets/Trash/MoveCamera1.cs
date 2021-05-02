using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera1 : MonoBehaviour
{
    [SerializeField] private Transform orientation;
    [SerializeField] private float sensitivity=50f;
    [SerializeField] private float minCamAngleY = -70f, maxCamAngleY = 80f;
    
    private float rotX;
    private float rotY;
    private Vector2 mouseDelta;


    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        
    }

    void Update()
    {
        Look();
    }

    private void Look()
    {
        
        rotY += mouseDelta.x * sensitivity * Time.deltaTime;
        rotX += mouseDelta.y * sensitivity * Time.deltaTime;

        rotX = Mathf.Clamp(rotX, minCamAngleY, maxCamAngleY);

        transform.rotation = Quaternion.Euler(-rotX, rotY, 0.0f);
        orientation.transform.rotation = Quaternion.Euler(0, rotY, 0);

    }
    
    public void OnMouseUpdate(Vector2 _mouseInput)
    {
        mouseDelta = _mouseInput;
    }
}