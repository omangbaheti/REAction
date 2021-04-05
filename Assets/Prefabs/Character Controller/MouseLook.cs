using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [SerializeField] private float sensitivityX = 8f;
    [SerializeField] private float sensitivityY = 0.5f;
    private Vector2 mouse;

    private void Update()
    {
        transform.Rotate(Vector3.up, mouse.x * Time.deltaTime);
    }

    public void ReceiveInput(Vector2 _mouseInput)
    {
        mouse.x = _mouseInput.x * sensitivityX;
        mouse.y = _mouseInput.y * sensitivityY;
    }
}
