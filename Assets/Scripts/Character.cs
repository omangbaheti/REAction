using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Character : MonoBehaviour
{
    public LayerMask layers;
    public Collider[] onGround;
    [SerializeField] private float speed = 10f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float mouseSensitivity = 50f;
    [SerializeField] private float minCamAngleY = -70f, maxCamAngleY = 80f; 
    [SerializeField] private float jumpCollisionRadius = 10f;
    [SerializeField] private Vector3 bottomOffset = new Vector3(0, 1, 0);
    //[SerializeField] private float minCamAngleX = -180f, maxCamAngleX = 180f;
    private CharacterController _characterController;
    private Camera _camera;
    private Vector3 _movement;
    private Vector3 _appliedGravity;
    public Vector3 jumpVector;
    private Vector2 _moveInput;

    private Vector2 _mouseMovement;
    private Vector2 _mouseDelta;
    private float xRotation;
    private float yRotation;
    
    
    
    
    private bool jumpInput;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _characterController = GetComponent<CharacterController>();
        _camera = Camera.main;
    }

    private void Update()
    {
        HandleGroundCollision();
        //handles current movement via WASD
        _movement = (transform.forward * _moveInput.y + transform.right * _moveInput.x).normalized;
        //handles current mouse input 
        _mouseMovement = _mouseDelta * (mouseSensitivity * Time.deltaTime);
        
        //Rotate camera on X axis (effectively moving it vertically)
        xRotation -= _mouseMovement.y;
        xRotation = Mathf.Clamp(xRotation, minCamAngleY, maxCamAngleY);
        _camera.transform.localRotation = Quaternion.Euler(xRotation,yRotation,0);

        //Rotate player on the Y axis 
        transform.Rotate(Vector3.up * _mouseMovement.x);
        
    }

    public void FixedUpdate()
    {
        //Apply Gravity
        if (_characterController.isGrounded)
        {
            _appliedGravity.y = 0f;
        }
        else
        {
            _appliedGravity.y += -9.81f * Time.fixedDeltaTime;
        }

        _characterController.Move((_movement * speed  + _appliedGravity + jumpVector)*Time.fixedDeltaTime);
        jumpVector.y = 0f;

    }

    public void  OnMoveInput(Vector2 _move)
    {
        _moveInput = _move;
    }

    public void OnJumpInput(bool jumpEvent)
    {
        if (onGround.Length > 0)
        {
            jumpVector = Vector3.up * jumpForce;
        }
        else
        {
            jumpVector = Vector3.zero;
        }
    }

    public void OnMouseUpdate(Vector2 _mouse)
    {
        _mouseDelta = _mouse;
    }


    private void HandleGroundCollision()
    {
        onGround = Physics.OverlapSphere(transform.position + bottomOffset, jumpCollisionRadius, layers);
    }
    
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        var positions = new Vector2[] { bottomOffset};

        Gizmos.DrawWireSphere(transform.position + bottomOffset, jumpCollisionRadius);
        //Gizmos.DrawWireSphere((Vector2)transform.position + rightOffset, collisionRadius);
        //Gizmos.DrawWireSphere((Vector2)transform.position + leftOffset, collisionRadius);
    }
}
