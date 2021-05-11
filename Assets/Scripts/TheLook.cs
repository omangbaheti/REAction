using UnityEngine;

public class TheLook : MonoBehaviour
{
    [SerializeField] private GameObject playerCam;
    [SerializeField] private GameObject orientation;
    [SerializeField] private float sensitivity=50f;
    [SerializeField] private float minCamAngleY = -70f; 
    [SerializeField] private float maxCamAngleY = 80f;
    
    private float _rotX;
    private float _rotY;
    
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
        _rotY += Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        _rotX += Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        _rotX = Mathf.Clamp(_rotX, minCamAngleY, maxCamAngleY);
        //fuck inverted people
        //they do be kinda weird doe
        playerCam.transform.rotation = Quaternion.Euler(-_rotX, _rotY, 0f);
        orientation.transform.rotation = Quaternion.Euler(0f, _rotY, 0f);
    }
}
