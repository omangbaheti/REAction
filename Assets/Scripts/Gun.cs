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
            rb.AddForce(bop * force);
        }
        
    }

    void Hook()
    {
        Transform hookPoint = GetHook();
        Vector3 dirn = hookPoint.position- rb.transform.position;
        rb.AddForce(dirn * hookSpeed);
        
    }

    Transform GetHook()
    {

        Transform hookPoint = hook.gameObject.transform;
        return hookPoint;
    }


}
