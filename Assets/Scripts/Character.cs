using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Character : MonoBehaviour
{
    public LayerMask layers;
    public bool onGround;
    
    [SerializeField] private float speed = 10f;
    private Vector3 _movement;
    
    [SerializeField] private float jumpHeight = 10f;
    [SerializeField] private float jumpCollisionRadius = 10f;
    [SerializeField]private float gravity = -9.81f;
    public Vector3 jumpVector;
    private Vector3 playerVelocity;
    
    [SerializeField] private float mouseSensitivity = 50f;
    [SerializeField] private float minCamAngleY = -70f, maxCamAngleY = 80f; 
    private Vector2 _mouseMovement;
    private Vector2 _mouseDelta;
    
    [SerializeField] private Vector3 bottomOffset = new Vector3(0, 1, 0);
    //[SerializeField] private float minCamAngleX = -180f, maxCamAngleX = 180f;
    
    private CharacterController _characterController;
    private Camera _camera;
    private Vector2 _moveInput;

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

        //Rotate player on the Y axis for horizontal camera movement
        transform.Rotate(Vector3.up * _mouseMovement.x);
        
    }

    private void FixedUpdate()
    {
        //Player Movement
        if (onGround && playerVelocity.y < 0)
            playerVelocity.y = 0f;
        
        //Movement on the plane
        _characterController.Move((_movement * (speed * Time.fixedDeltaTime)));
        
        //apply gravity
        playerVelocity.y += gravity * Time.deltaTime;
        _characterController.Move(playerVelocity * Time.fixedDeltaTime);
    }


    public void  OnMoveInput(Vector2 _move)
    {
        _moveInput = _move;
    }

    public void OnJumpInput(bool jumpEvent)
    {
        if (onGround)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -2.0f * gravity);
        }
    }

    public void OnMouseUpdate(Vector2 _mouse)
    {
        _mouseDelta = _mouse;
    }


    private void HandleGroundCollision()
    {
        onGround = Physics.CheckSphere(transform.position + bottomOffset, jumpCollisionRadius, layers);
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
