using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed;
    public float jumpForce;
    public LayerMask ground;
    private float x;
    private float z;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        x = Input.GetAxisRaw("Horizontal");
        z = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded())
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
        }
    }

    private void FixedUpdate()
    {
        rb.AddForce(transform.right * x * moveSpeed, ForceMode.VelocityChange);
        rb.AddForce(transform.forward * z * moveSpeed, ForceMode.VelocityChange);

        counterForce(0, 0.75f);
    }

    void counterForce(float maxSpeed, float stopStrength)
    {
        Vector3 vel = rb.velocity;
        vel.y = 0;
        float mag = vel.magnitude;
        if (mag > maxSpeed)
        {
            rb.AddForce(-vel.normalized * (mag - maxSpeed) * stopStrength, ForceMode.VelocityChange);
        }
    }

    bool isGrounded()
    {
        if(Physics.Raycast(transform.position, Vector3.down, 1, ground))
        {
            return true;
        }
        return false;
    }
}
