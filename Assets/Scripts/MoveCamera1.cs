using System;
using UnityEngine;

public class MoveCamera1 : MonoBehaviour {

    public Transform player;
    
    [SerializeField] private float mouseSensitivity = 50f;
    [SerializeField] private float minCamAngleY = -70f, maxCamAngleY = 80f;
    [SerializeField] private Transform head;
    private Vector3 moveInput;
    private Vector2 mouseDelta;
    private Quaternion rotationToCamera;
    private Camera _mainCam;
    private float xRotation;
    private float yRotation;

    private void Awake()
    {
        _mainCam = Camera.main;
    }

    void LateUpdate() {
        Vector3 mouseMovement = mouseDelta * (mouseSensitivity * Time.deltaTime);
        
        //Rotate camera on X axis (effectively moving it vertically)
        xRotation -= mouseMovement.y ;
        xRotation = Mathf.Clamp(xRotation, minCamAngleY, maxCamAngleY);
        yRotation += mouseMovement.x;
        
        Vector3 projectCameraForward = Vector3.ProjectOnPlane(_mainCam.transform.forward, Vector3.up);
        rotationToCamera = Quaternion.LookRotation(projectCameraForward, Vector3.up);
        
        //Rotate player on the Y axis for horizontal camera movements
        head.transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }
    
    public void OnMouseUpdate(Vector2 _mouseInput)
    {
        mouseDelta = _mouseInput;
    }
}
