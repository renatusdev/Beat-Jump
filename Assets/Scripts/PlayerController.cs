using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int rotationSpeed;
    public int movementSpeed;
    public int airMaxSpeed;
    public int airAccelerationSpeed;
    public float sprintSpeedModifier;
    public float gravityFallMultiplier;
    public float gravityForce;
    public float gravitySmallJump;
    public float jumpForce;
    public LayerMask groundCheckLayers;

    public bool isGrounded { get; private set; }    
    public Vector3 velocity { get; set; }    
    public Camera cam { get; set; }

    private float m_CamVerticalAngle;
    private float m_LastTimeJumped;
    private CharacterController m_Controller;
    private WallRun m_WallRun;
    private Vector3 m_GroundNormal;
    
    const float k_JumpCheckPreventionTime = 0.4f;
    const float k_GroundCheckDistance = 1;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        m_Controller = GetComponent<CharacterController>();
        cam = GetComponentInChildren<Camera>();
        m_WallRun = GetComponent<WallRun>();
    }

    void Update()
    {
        GroundCheck();
        Movement();

        if (transform.position.y <= -5)
        {
            foreach (GameObject o in GameObject.FindGameObjectsWithTag("CurrentCheckpoint"))
            {
                Debug.Log(o.transform.position);
                transform.position = o.transform.position;
            }
        }
    }

    void GroundCheck()
    {
        isGrounded = false;

        Vector3 topCapsHemisphere = transform.position + transform.up * m_Controller.radius;
        Vector3 botCapsHemisphere = transform.position - transform.up * m_Controller.radius;

        // Makes sure that GroundCheck is not performed right after a jump.
        if(Time.time > m_LastTimeJumped + k_JumpCheckPreventionTime)
        {
            if(Physics.CapsuleCast(topCapsHemisphere, botCapsHemisphere, m_Controller.radius, Vector3.down,
                out RaycastHit hit, m_Controller.skinWidth + 0.1f, groundCheckLayers, QueryTriggerInteraction.Ignore))
            {
                m_GroundNormal = hit.normal;
                isGrounded = true;
            }
        }
    }

    void Movement()
    {
        // Looking
        if(!Player.Paused){
            // Horizontal Rotation
            transform.Rotate(new Vector3(0, Input.GetAxisRaw("Mouse X") * rotationSpeed, 0), Space.Self);

            m_CamVerticalAngle += -Input.GetAxisRaw("Mouse Y") * rotationSpeed;
            m_CamVerticalAngle = Mathf.Clamp(m_CamVerticalAngle, -89f, 89f);

            // Vertical Camera Rotation

            // If the player can wallrun.
            if(m_WallRun != null)
            {
                cam.transform.localEulerAngles = new Vector3(m_CamVerticalAngle, 0, m_WallRun.GetCameraRoll());
            }
            else
            {
                cam.transform.localEulerAngles = new Vector3(m_CamVerticalAngle, 0, 0);
            }
        }

        // Moving
        {
            float speedModifier = 1;    

            if(Input.GetButton("Sprint"))
                speedModifier = sprintSpeedModifier;

            Vector3 move = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
            move.Normalize();
            move = transform.TransformVector(move);
            
            if(isGrounded || (m_WallRun != null && m_WallRun.IsWallRunning()))
            {
                velocity = move * movementSpeed * speedModifier;
                Vector3 directionRight = Vector3.Cross(velocity.normalized, transform.up);
                velocity = Vector3.Cross(m_GroundNormal, directionRight).normalized * velocity.magnitude;
        
                // Jump
                if(Input.GetButtonDown("Jump") & BeatSignal.i.IsInLowBeat()) 
                {   
                    velocity += new Vector3(move.x, 0, move.z);
                    if(!m_WallRun.IsWallRunning())
                    {
                        velocity += Vector3.up * jumpForce;
                    }
                    else
                    {
                        velocity += m_WallRun.GetWallJumpDirection() * jumpForce;
                    }
                    isGrounded = false;
                    m_LastTimeJumped = Time.time;
                }
            }
            else
            {
                // Air Movement
                velocity += move * airAccelerationSpeed * Time.deltaTime;
                    
                // Horizontal clamp
                Vector3 horizontalVelocity = Vector3.ProjectOnPlane(velocity, Vector3.up);
                horizontalVelocity = Vector3.ClampMagnitude(horizontalVelocity, movementSpeed * speedModifier);
                velocity = horizontalVelocity + (Vector3.up) * velocity.y;

                // Gravity
                velocity += Vector3.down * gravityForce * Time.deltaTime;

                // Better Gravity
                if(velocity.y < 0)
                    velocity += Vector3.down * gravityForce * gravityFallMultiplier * Time.deltaTime;
                else if(velocity.y > 0 & !Input.GetButton("Jump"))
                    velocity += Vector3.down * gravityForce * gravitySmallJump * Time.deltaTime;
            }

            m_Controller.Move(velocity * Time.deltaTime);
        }
    }

    private void OnGUI()
    {
        GUI.TextArea(new Rect(25,25, 50, 50), "isGrounded: " + isGrounded);    
    }
}