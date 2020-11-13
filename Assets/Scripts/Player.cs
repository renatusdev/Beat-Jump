using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed;
    public float jumpForce;
    public LayerMask ground;
    public TextBoxStuff txtbs;
    
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

        if (TextBoxStuff.waited)
        {
            MakeTextBoxText("", 0);
            TextBoxStuff.waited = false;
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "FirstTutorialText")
        {
            MakeTextBoxText("Hello i am bob fuck you", 1);
            Destroy(collision.gameObject);
            txtbs.StartCoroutine(txtbs.MakeDelay(5));
            
            
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
        if(Physics.CheckSphere(new Vector3(transform.position.x, transform.position.y - 1, transform.position.z), 0.4f, ground))
        {
            return true;
        }
        return false;
    }

    public void MakeTextBoxText(string text, float delay)
    {
        txtbs.wantedTextObject.text = "";
        //txtbs.StartCoroutine(txtbs.MakeDelay(delay));
        txtbs.wantedText = text;
        txtbs.StartCoroutine(txtbs.MakeTextBeCool(0.1f));
    }
}
