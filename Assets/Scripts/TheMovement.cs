﻿using UnityEngine;
//need to change the damn name of the class (aka refactor)
public class TheMovement : MonoBehaviour
{
    [Header("Movement Parameters")]
    [SerializeField] private float maxSpeed = 100f;
    [SerializeField] private float recoilForce = 100f;
    [SerializeField] private float moveForce = 3f;

    [Header("Current Status")]
    public bool isGrounded;
    public bool cantShoot;
    public bool hasShot;

    [Header("Jump parameters")]
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float jumpCollisionRadius  = 0.1f;
    [SerializeField] private float airControl=3f;
    [SerializeField] private Vector3 bottomOffset;

    [Header("References")]
    public LayerMask layers;
    [SerializeField] private Transform orientation;
    
    private Vector3 _moveInput;
    private Vector2 _mouseDelta;
    private Quaternion _rotationToCamera;
    private RaycastHit _projectOnGround;
    private Rigidbody _rigidbody;
    private Transform _mainCam;
    private bool _cantJump;
    private float _mass;
    private float _groundAngle;
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        if (!(Camera.main is null)) _mainCam = Camera.main.transform;
        _mass = _rigidbody.mass;
    }

    private void Update()
    {
        HandleGroundCollision();
        _rotationToCamera = Quaternion.LookRotation(orientation.forward, Vector3.up);

        
    }
    
    private void FixedUpdate()
    {
        float effectiveMoveForce = moveForce;
        if (!isGrounded) effectiveMoveForce = moveForce / airControl;
        Vector3 movement = _rotationToCamera * _moveInput * maxSpeed;
        ApplyForceToReachVelocity(_rigidbody, movement, effectiveMoveForce );
    }

    
    private void ApplyForceToReachVelocity(Rigidbody rb, Vector3 desiredVelocity, float force = 1.5f, ForceMode mode = ForceMode.Force)
    {
        if (Mathf.Approximately(force , 0f) || Mathf.Approximately(desiredVelocity.sqrMagnitude , 0))
            return;

        desiredVelocity += desiredVelocity.normalized * (0.2f * rb.drag);

        //force = 1 => need 1 s to reach velocity (if mass is 1) => force can be max 1 / Time.fixedDeltaTime
        
        force = Mathf.Clamp(force, -_mass / Time.fixedDeltaTime,_mass / Time.fixedDeltaTime);

        //dot product is a projection from rhs to lhs with a length of result / lhs.magnitude https://www.youtube.com/watch?v=h0NJK4mEIJU
        if (rb.velocity.sqrMagnitude == 0)
        {
            rb.AddForce(desiredVelocity * force, mode);
        }
        else
        {
            Vector3 velocityProjectedToTarget = (desiredVelocity.normalized * Vector3.Dot(desiredVelocity, rb.velocity) / desiredVelocity.magnitude);
            rb.AddForce((desiredVelocity - velocityProjectedToTarget) * force, mode);
            
        }
        
    }
    
    private void HandleGroundCollision()
    {
        isGrounded = Physics.CheckSphere(transform.position + bottomOffset, jumpCollisionRadius, layers);
    }


    //*******************************HANDLE INPUT*******************************
    
    public void OnMoveInput(Vector2 move)
    { 
        _moveInput = new Vector3(move.x, 0f, move.y).normalized;  
        
    }
    public void OnJumpInput(bool jump)
    {
        if (!isGrounded||_cantJump) return;
        
        cantShoot = true;
        Invoke(nameof(ShootCd), 0.2f);
        _rigidbody.AddForce(0f , jumpForce,0f, ForceMode.Impulse);
        
    }

    void ShootCd()
    {
        cantShoot = false;
    }

    void JumpCd()
    {
        _cantJump = false;
    }

    public void OnShoot(bool shoot)
    {
        
        if (hasShot||cantShoot) return;
        
        _cantJump = true;
        Invoke(nameof(JumpCd), 0.2f);

        Vector3 camAngle = _mainCam.forward.normalized;
        //I am proud of this shit fwak u
        Vector3 velocity = _rigidbody.velocity;
        velocity = new Vector3(
            velocity.x * (1f - Mathf.Abs(camAngle.x)),
            velocity.y * (1f - Mathf.Abs(camAngle.y)), 
            velocity.z * (1f - Mathf.Abs(camAngle.z)));
        _rigidbody.velocity = velocity;
    
        _rigidbody.AddForce(-1f * camAngle * recoilForce );
        hasShot = true;
        Invoke(nameof(CanShoot),1f);
        
    }
    void CanShoot()
    {
        hasShot = false;
    }

    
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + bottomOffset, jumpCollisionRadius);
    }
}
