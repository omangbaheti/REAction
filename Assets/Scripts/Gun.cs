using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public Rigidbody rb;
    public Transform cam;
    public float force = 100f;
    private bool hasBopped;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetMouseButtonDown(0) && !hasBopped)
        {
            Bop();
            hasBopped = true;
            Invoke(nameof(CanBop),1f);
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
            //rb.velocity = Vector3.zero;
            //Vector3 temp = transform.rotation.eulerAngles.x ;
            //bop += transform.up * force *0.05f;
            rb.AddForce(bop * force);
        }
        
    }


}
