using System;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class WallRun : MonoBehaviour
{
    public float wallMaxDistance = 1;
    public float wallSpeedMultiplier = 1.2f;
    public float gravityOnWallRide = 4;
    public float maxAngleRoll = 25;
    public float cameraTransitionDuration = 1;
    public float jumpDuration = 1;
    public float wallBouncing = 3;
    public float minHeightForWallRun = 1.2f;

    private PlayerController m_Controller;
    
    Vector3[] directions;
    RaycastHit[] hits;
    Vector3 lastWallPosition;
    Vector3 lastWallNormal;
    bool isWallRunning = false;
    bool jumping;
    float elapsedTimeSinceWallAttach = 0;
    float elapsedTimeSinceWallDetatch = 0;
    float elapsedTimeSinceJump = 0;

    public bool IsWallRunning() => isWallRunning;

    void Start()
    {
        m_Controller = GetComponent<PlayerController>();

        directions = new Vector3[]
        { 
            Vector3.right, 
            Vector3.right + Vector3.forward,
            Vector3.forward, 
            Vector3.left + Vector3.forward, 
            Vector3.left
        };   
    }

    private void Update()
    {
        // Useful variable for determening the player's jump 
        // and how long he will go up before attatching to a wall.
        if(Input.GetButtonDown("Jump"))
            jumping = true;
    }

    // This method allows the player to jump to a wall only during a fixed timeframe.
    // This timeframe is 1 second after the they have jumped.
    // Meaning the player will not be attached to a wall right after they jump.
    // Rather they will press jump, wait 1 second, and then they can attach themselves to the wall. 
    private bool CanAttach()
    {
        // If player pressed jump button at some time
        if(jumping)
        {
            // Sum up the current frame's time to the elapsed time since the player pressed jump
            elapsedTimeSinceJump += Time.deltaTime;
            
            // If the elapsed time since the player jumped is greater than the jump duration
            if(elapsedTimeSinceJump > jumpDuration)
            {
                // Reset elapse time since jump.
                elapsedTimeSinceJump = 0;

                // The player has finally reached the jump duration.
                // Meaning now they can attach themselves to a wall.
                // Instead of attaching instantly to a wall upon pressing space bar.
                jumping = false;
                return true;
            }
            
            // The player is still going up to the wall. 
            return false;
        }
        else
        {
            // The jump duration is complete.
            // The player can already jump to a wall.
            return true;
        }
    } 

    private void LateUpdate()
    {
        isWallRunning = false;

        if(CanAttach())
        {

            // Raycast hits for the possible directions for a walljump.
            hits = new RaycastHit[directions.Length];

            // Check each direction to see if there's a wall ABOUT to be jumped on. (Close distance).
            for(int i = 0; i < directions.Length; i++)
                Physics.Raycast(transform.position, transform.TransformDirection(directions[i]), out hits[i], wallMaxDistance);

            
            // If the player is pressing W, sprinting, and not on the ground, then the player can wallrun.
            if(CanWallRun())
            {
                // Sort all raycast hits that did collide with a wall.
                // And order them by the closest in distance. (The closest being being in the index zero).
                hits = hits.ToList().Where(h => h.collider != null).OrderBy(h => h.distance).ToArray();

                // If at least one raycast hit a wall.
                // Recall that the first raycast index is the closest one to a wall.
                if(hits.Length > 0)
                {
                    // Register the normal of the hit.
                    lastWallNormal = hits[0].normal;
                    // Register the point in which the hit occured.
                    lastWallPosition = hits[0].point;
                    // Begin wallriding.
                    WallRiding(hits[0]);
                }
            }
        }

        if(isWallRunning)
        {
            // Timer for camera dutch on wall attatchment
            elapsedTimeSinceWallDetatch = 0;
            elapsedTimeSinceWallAttach += Time.deltaTime;

            // Apply wallrun gravity
            m_Controller.velocity += Vector3.down * gravityOnWallRide * Time.deltaTime;
        }
        else
        {   
            // Timer for camera dutch on wall detatchment
            elapsedTimeSinceWallAttach = 0;
            elapsedTimeSinceWallDetatch += Time.deltaTime;
        }
    }

    private void WallRiding(RaycastHit hit)
    {
        // The player will now haev a forward velocity based on the wallRun speed and them pressing W.
        m_Controller.velocity = transform.forward * Input.GetAxisRaw("Vertical") * wallSpeedMultiplier;
        
        // Activate wall running flag to true.
        isWallRunning = true;
    }

    float CalculateSide()
    {
        if(isWallRunning)
        {
            Vector3 heading = lastWallPosition - transform.position;
            Vector3 perp = Vector3.Cross(transform.forward, heading);
            float dir = Vector3.Dot(perp, transform.up);
            return dir;
        }
        return 0;
    }

    public float GetCameraRoll()
    {
        float dir = CalculateSide();
        float cameraAngle = m_Controller.cam.transform.eulerAngles.z;
        float targetAngle = 0;
        
        if(dir != 0)
        {
            targetAngle = Mathf.Sign(dir) * maxAngleRoll;
        }

        return Mathf.LerpAngle(cameraAngle, targetAngle, Mathf.Max(elapsedTimeSinceWallAttach, elapsedTimeSinceWallDetatch) / cameraTransitionDuration);
    }

    public Vector3 GetWallJumpDirection()
    {
        if(!isWallRunning)
            return Vector3.zero;
        return lastWallNormal * wallBouncing + Vector3.up;
    } 

    private bool CanWallRun()
    {
        // If the player is pressing W.
        bool canWallRun = Input.GetAxisRaw("Vertical") > 0;
        // And is sprinting
        canWallRun &= Input.GetButton("Sprint");
        // And is not on the ground
        canWallRun &= !m_Controller.isGrounded;
        // and there is no ground below
        canWallRun &= !Physics.Raycast(transform.position, Vector3.down, minHeightForWallRun);
        
        return canWallRun;
    }
    
}
