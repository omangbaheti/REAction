using System;
using UnityEngine;

public class TheMovement : MonoBehaviour
{
    public LayerMask layers;
    [SerializeField] private Vector3 bottomOffset;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float maxSpeed = 100f;
    [SerializeField] private float jumpCollisionRadius  = 0.1f;
    [SerializeField] private float recoilForce = 100f;
    [SerializeField] private float airControl=3f;
    [SerializeField] private float moveForce = 3f;
    
    [SerializeField] private GameObject orientation;
    

    public bool isGrounded;
    public bool cantShoot;
    public bool hasShot=false;
    private Vector3 moveInput;
    private Vector2 mouseDelta;
    private Quaternion rotationToCamera;
    
    private Rigidbody _rigidbody;
    private Camera _mainCam;
    private bool cantJump=false;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _mainCam = Camera.main;
    }

    void Update()
    {
        HandleGroundCollision();
        Vector3 projectCameraForward = Vector3.ProjectOnPlane(_mainCam.transform.forward, Vector3.up);
        rotationToCamera = Quaternion.LookRotation(orientation.transform.forward, Vector3.up);
    }

    private void FixedUpdate()
    {
        float effectiveMoveForce = moveForce;
        if (!isGrounded) effectiveMoveForce/=airControl;
        ApplyForceToReachVelocity(_rigidbody, rotationToCamera *  moveInput *  maxSpeed, effectiveMoveForce );
    }
    
    public void ApplyForceToReachVelocity(Rigidbody rigidbody, Vector3 velocity, float force = 3, ForceMode mode = ForceMode.Force)
    {
        if (Mathf.Approximately(force , 0) || Mathf.Approximately(velocity.magnitude , 0))
            return;

        velocity += velocity.normalized * (0.2f * rigidbody.drag);

        //force = 1 => need 1 s to reach velocity (if mass is 1) => force can be max 1 / Time.fixedDeltaTime
        force = Mathf.Clamp(force, -rigidbody.mass / Time.fixedDeltaTime,rigidbody.mass / Time.fixedDeltaTime);

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
    
    private void HandleGroundCollision()
    {
        isGrounded = Physics.CheckSphere(transform.position + bottomOffset, jumpCollisionRadius, layers);
    }
    
    //*******************************HANDLE INPUT*******************************
    
    public void OnMoveInput(Vector2 _move)
    { 
        moveInput = new Vector3(_move.x, 0f, _move.y).normalized;  
        
    }
    public void OnJumpInput(bool _jump)
    {
        if (!isGrounded||cantJump) return;
        
        cantShoot = true;
        Invoke(nameof(ShootCd), 0.2f);
        _rigidbody.AddForce(Vector3.up * jumpForce);
        
    }

    void ShootCd()
    {
        cantShoot = false;
    }

    void JumpCd()
    {
        cantJump = false;
    }

    public void OnShoot(bool _shoot)
    {
        
        if (hasShot||cantShoot) return;
        
        cantJump = true;
        Invoke(nameof(JumpCd), 0.2f);

        _rigidbody.velocity=Vector3.zero;
        
        _rigidbody.AddForce(-1 * _mainCam.transform.forward * recoilForce );
        hasShot = true;
        Invoke(nameof(CanShoot),1f);
        
    }
    void CanShoot()
    {
        hasShot = false;
    }
    //*******************************GIZMOS*******************************
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        var positions = new Vector3[] {bottomOffset};
        Gizmos.DrawWireSphere(transform.position + bottomOffset, jumpCollisionRadius);
    }
}
