using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPlayerScripp : MonoBehaviour
{
    public LayerMask layers;
    private bool isGrounded;

    [SerializeField] private Vector3 bottomOffset;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float maxSpeed = 100f;
    [SerializeField] private float jumpCollisionRadius  = 0.1f;
    [SerializeField] private float mouseSensitivity = 50f;
    [SerializeField] private float minCamAngleY = -70f, maxCamAngleY = 80f;

    private Vector3 moveInput;
    private Vector2 mouseDelta;
    private Quaternion rotationToCamera;
    
    private float xRotation;
    private float yRotation;
    
    private Rigidbody _rigidbody;
    private Camera _mainCam;
    [SerializeField]private Transform head;

     private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _mainCam = Camera.main;
        
    }

    private void FixedUpdate()
    {
        //ApplyGravity();
        ApplyForceToReachVelocity(_rigidbody, rotationToCamera *  moveInput *  maxSpeed );
        
        
    }

    private void Update()
    {
        HandleGroundCollision();
        //MouseLook(mouseDelta);
        Vector3 projectCameraForward = Vector3.ProjectOnPlane(_mainCam.transform.forward, Vector3.up);
        rotationToCamera = Quaternion.LookRotation(projectCameraForward, Vector3.up);
        
        
    }

    private Vector3 Movement(Vector2 moveInput)
    {
        Vector3 moveDirection = rotationToCamera*(Vector3.right * moveInput.x + Vector3.forward * moveInput.y);
        
        
        //transform.rotation =
            //Quaternion.RotateTowards(transform.rotation, rotationToCamera, mouseSensitivity * Time.deltaTime);

            return moveDirection; //= rotationToCamera * moveDirection;

    }

    private void MouseLook(Vector2 _mouseDelta)
    {
        //handles current mouse input 
        Vector3 mouseMovement = _mouseDelta * (mouseSensitivity * Time.deltaTime);
        
        //Rotate camera on X axis (effectively moving it vertically)
        xRotation -= mouseMovement.y ;
        xRotation = Mathf.Clamp(xRotation, minCamAngleY, maxCamAngleY);
        yRotation += mouseMovement.x;
        
        Vector3 projectCameraForward = Vector3.ProjectOnPlane(_mainCam.transform.forward, Vector3.up);
        rotationToCamera = Quaternion.LookRotation(projectCameraForward, Vector3.up);
        
        //Rotate player on the Y axis for horizontal camera movements
        head.transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }
    
    private void HandleGroundCollision()
    {
        isGrounded = Physics.CheckSphere(transform.position + bottomOffset, jumpCollisionRadius, layers);
    }

    private Vector2 VelocityRelativeToSight()
    {
        float lookAngle = transform.localRotation.y;
        float moveAngle = Mathf.Atan2(_rigidbody.velocity.x, _rigidbody.velocity.y) * Mathf.Rad2Deg;
        float delta = Mathf.DeltaAngle(lookAngle, moveAngle)* Mathf.Deg2Rad;

        float VelocityY = _rigidbody.velocity.magnitude * Mathf.Cos(delta);
        float VelocityX = _rigidbody.velocity.magnitude * Mathf.Cos(Mathf.PI/2 - delta);

        return new Vector2(VelocityX, VelocityY);
    }
    
    public static void ApplyForceToReachVelocity(Rigidbody rigidbody, Vector3 velocity, float force = 1, ForceMode mode = ForceMode.Force)
    {
        if (force == 0 || velocity.magnitude == 0)
            return;

        velocity = velocity + velocity.normalized * 0.2f * rigidbody.drag;

        //force = 1 => need 1 s to reach velocity (if mass is 1) => force can be max 1 / Time.fixedDeltaTime
        force = Mathf.Clamp(force, -rigidbody.mass / Time.fixedDeltaTime, rigidbody.mass / Time.fixedDeltaTime);

        //dot product is a projection from rhs to lhs with a length of result / lhs.magnitude https://www.youtube.com/watch?v=h0NJK4mEIJU
        if (rigidbody.velocity.magnitude == 0)
        {
            rigidbody.AddForce(velocity * force, mode);
        }
        else
        {
            var velocityProjectedToTarget = (velocity.normalized * Vector3.Dot(velocity, rigidbody.velocity) / velocity.magnitude);
            rigidbody.AddForce((velocity - velocityProjectedToTarget) * force, mode);
        }
    }
    
    private void ApplyGravity()
    {
        if (!isGrounded)
            _rigidbody.AddForce(Vector3.up * (gravity * Time.deltaTime), ForceMode.Force);
        //else
            //_rigidbody.AddForce(Vector3.up * 0);
    }
    
    //*******************************HANDLE INPUT*******************************
    public void OnMoveInput(Vector2 _move)
    {
        moveInput = new Vector3(_move.x, 0f, _move.y);

    }

    

    public void OnJumpInput()
    {
        _rigidbody.AddForce(Vector3.up * jumpForce);
    }
    
    
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        var positions = new Vector3[] { bottomOffset};
        Gizmos.DrawWireSphere(transform.position + bottomOffset, jumpCollisionRadius);
    }
}
