﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float RotateSensitivity = 280.0f;

    private Rigidbody _rigidbody;
    private Vector2 moveInput;
    private Camera _camera;

    private void Start()
    {
        _camera = Camera.main;
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void OnMoveInput(Vector2 _move)
    {
        this.moveInput = _move;
    }

    public void OnJumpInput(float _jumpForce)
    {
        _rigidbody.AddForce(Vector2.up * 100 * 1.5f);
    }

    private void Update()
    {   
        Vector3 moveDirection = Vector3.forward * moveInput.y + Vector3.right * moveInput.x;
        Vector3 projectCameraForward = Vector3.ProjectOnPlane(_camera.transform.forward, Vector3.up);
        Quaternion rotationToCamera = Quaternion.LookRotation(projectCameraForward, Vector3.up);
        
        transform.rotation =
            Quaternion.RotateTowards(transform.rotation, rotationToCamera, RotateSensitivity * Time.deltaTime);
        
        moveDirection = rotationToCamera * moveDirection;
        transform.position += moveDirection * (moveSpeed * Time.deltaTime);

    }
}
