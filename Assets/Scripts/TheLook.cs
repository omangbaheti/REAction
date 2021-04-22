using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheLook : MonoBehaviour
{
    [SerializeField]
    private GameObject playerCam;
    [SerializeField]
    private GameObject orientation;
    [SerializeField]
    private float sensitivity=50f;
    [SerializeField]
    private float clampAngle =80f;
    
    private float rotX;
    private float rotY;


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
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        rotY += mouseX;
        rotX += mouseY;

        rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle);

        playerCam.transform.rotation = Quaternion.Euler(-rotX, rotY, 0.0f);
        orientation.transform.rotation = Quaternion.Euler(0, rotY, 0);

    }
}
