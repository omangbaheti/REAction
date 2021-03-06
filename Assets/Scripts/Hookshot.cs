using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hookshot : MonoBehaviour
{
    public bool hooking;

    [SerializeField] private float HookRange;
    [SerializeField] private LayerMask layer;
    [SerializeField] private float force = 0.5f;
    [SerializeField] private Vector3 hitPosition;
    [SerializeField] private Transform debugPosition;
    
    private Transform _camTransform;
    private Rigidbody _rigidbody;
    private RaycastHit hookHit;
    public float distanceToDestination;

    [Header("Rope Render Parameters")]
    [SerializeField] private int ropeQuality;
    [SerializeField] private AnimationCurve _curve;
    [SerializeField] private float waveHeight;
    [SerializeField] private float waveCount;
    [SerializeField] private float ropeVelocity;
    [SerializeField] private float strength;
    [SerializeField] private float damper;
    private LineRenderer _lineRenderer;
    //private Spring _spring;
    
    
    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        //_spring = new Spring();
    }
    
    void Start()
    {
        _camTransform = Camera.main.transform;
        _rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (hooking)
        {
            HandleHookMovement();
        }
    }

    public void OnHookInput(bool hookingStatus)
    {
        //casts a raycast from current position in the forward direction to a maximum distance of HookRange and 
        //outputs the data obtained into hookHit
        
        if (Physics.Raycast(_camTransform.position, _camTransform.forward,out hookHit, HookRange, layer ))
        {
            hitPosition = hookHit.point;
            debugPosition.position = hookHit.point;
            _rigidbody.velocity = Vector3.zero;
            hooking = hookingStatus;
        }
    }

    void HandleHookMovement()
    {
        distanceToDestination = Vector3.Distance(transform.position, hitPosition);
        if (distanceToDestination<2f)
        {
            // Debug.Log("Your Destination is in front of you");
            hooking = false;
            return;
        }

        Vector3 hookShotDir = (hitPosition - transform.position).normalized;
        _rigidbody.velocity = hookShotDir * force;
        //_rigidbody.AddForce(hookShotDir * force, ForceMode.Impulse);
    }
    
    void DrawRope()
    {
        if(!hooking) return;
        
    }
}
