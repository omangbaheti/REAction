using System;
using UnityEngine;

public class TheMovement : MonoBehaviour
{
    [SerializeField]
    private float extraGrav = 10f;
    [SerializeField]
    private Rigidbody rb;
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private int maxSpeed;
    
    private float x,y;
    private bool jumping;

    void Start()
    {
        Movement();
    }

    void Update()
    {
        MyInput();
    }

    private void MyInput()
    {
        x = Input.GetAxisRaw("Horizontal");
        y = Input.GetAxisRaw("Vertical");
        jumping = Input.GetButton("Jump");
    }

    private void Movement()
    {
        rb.AddForce(Vector3.down * Time.deltaTime * extraGrav);

        Vector2 mag = rb.velocity;
        float xMag = mag.x, yMag=mag.y;

        if (jumping) Jump();

        if (x > 0 && xMag > maxSpeed) x = 0;
        if (x < 0 && xMag < -maxSpeed) x = 0;
        if (y > 0 && yMag > maxSpeed) y = 0;
        if (y < 0 && yMag < -maxSpeed) y = 0;

        rb.AddForce(transform.forward * y * moveSpeed * Time.deltaTime);
    }

    private void Jump()
    {
        throw new NotImplementedException();
    }
}
