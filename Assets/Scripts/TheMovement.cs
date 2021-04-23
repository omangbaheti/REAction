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
    

    public bool isGrounded;
    public bool cooledDown;
    public bool hasShot=false;
    private Vector3 moveInput;
    private Vector2 mouseDelta;
    private Quaternion rotationToCamera;
    
    private Rigidbody _rigidbody;
    private Camera _mainCam;
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _mainCam = Camera.main;
    }

    void Update()
    {
        HandleGroundCollision();
        Vector3 projectCameraForward = Vector3.ProjectOnPlane(_mainCam.transform.forward, Vector3.up);
        rotationToCamera = Quaternion.LookRotation(projectCameraForward, Vector3.up);
    }

    private void FixedUpdate()
    {
        ApplyForceToReachVelocity(_rigidbody, rotationToCamera *  moveInput *  maxSpeed );
    }
    
    public static void ApplyForceToReachVelocity(Rigidbody rigidbody, Vector3 velocity, float force = 3, ForceMode mode = ForceMode.Force)
    {
        if (force == 0 || velocity.magnitude == 0)
            return;

        velocity = velocity + velocity.normalized * 0.2f * rigidbody.drag;

        //force = 1 => need 1 s to reach velocity (if mass is 1) => force can be max 1 / Time.fixedDeltaTime
        force = Mathf.Clamp(force, -rigidbody.mass / Time.fixedDeltaTime,
                                        rigidbody.mass / Time.fixedDeltaTime);

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
        if (isGrounded)
        { 
            _rigidbody.AddForce(Vector3.up * jumpForce);
            
        }
    }

    void shootCooldown()
    {
        cooledDown = true;
    }

    public void OnShoot(bool _shoot)
    {
        
        if (hasShot ) return;
        
        if(isGrounded)
            _rigidbody.velocity=Vector3.zero;
        
        _rigidbody.AddForce(-1 * _mainCam.transform.forward * recoilForce );
        hasShot = true;
        Invoke(nameof(CanShoot),1f);
        //Debug.Log("pow");
    }
    void CanShoot()
    {
        hasShot = false;
    }
    //*******************************GIZMOS*******************************
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        var positions = new Vector3[] { bottomOffset};
        Gizmos.DrawWireSphere(transform.position + bottomOffset, jumpCollisionRadius);
    }
}
