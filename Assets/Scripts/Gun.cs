using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public Rigidbody rb;
    public Transform cam;
    public float force = 100f;
    private bool hasBopped;
    public float hookRange = 10f;
    public float dotThreshold = 0.9f;
    public GameObject hook;
    public int hookSpeed=10;
    private bool hooking = false;
    Vector3 dirn;

    void Update()
    {
        
        if (Input.GetMouseButtonDown(0) && !hasBopped)
        {
            Bop();
            hasBopped = true;
            Invoke(nameof(CanBop),1f);
        }

        if (Input.GetMouseButtonDown(1))
        {
            Hook();
            //Debug.Log("I am pepega");
            //Debug.Log(rb.transform);
        }
        
    }

    private void FixedUpdate()
    {
        if (hooking)
        {
            rb.AddForce(dirn * hookSpeed);
            Invoke(nameof(StopHook), 0.1f);
        }
    }
    bool CanBop()
    {
        hasBopped = false;
        return true;
    }
    void Bop()
    {
        if (!hasBopped)
        {
            Vector3 bop = -cam.forward;
            rb.velocity = Vector3.zero;
            rb.AddForce(transform.up * hookSpeed * 0.1f);
            rb.AddForce(bop * force);
        }
        
    }

    void Hook()
    {
        Transform hookPoint = GetHook();
        dirn = (hookPoint.position- rb.transform.position);
        dirn.Normalize();
        rb.AddForce(transform.up*hookSpeed*0.1f);
        hooking = true;
    }

    Transform GetHook()
    {
        Transform hookPoint = hook.gameObject.transform;
        return hookPoint;
    }

    void StopHook()
    {
        hooking = false;
    }


}
