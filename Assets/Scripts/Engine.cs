using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Engine : MonoBehaviour {

    // Physics-related values
    [SerializeField]
    private bool ignoreGravity;
    [SerializeField]
    [Range(0.0f, 3.0f)] private float heightOfJump; 
    [Range(-1.0f, 1.0f)] private float percentageOfGravity;
    public float PercentageOfGravity
    {
        get
        {
            return percentageOfGravity;
        }

        set
        {
            percentageOfGravity = value;
        }
    }
    public Vector2 Position
    {
        get
        {
            return this.transform.position;
        }

        private set
        {
            this.transform.position = value;
        }
    }

    // Collision-related values
    private bool clampXAbovePosition;
    private float clampXAboveValue;
    private OnCollisionCustomAction leftCollisionAction;

    private bool clampYAbovePosition;
    private float clampYAboveValue;
    private OnCollisionCustomAction bottomCollisionAction;

    private DirectionsEnum.Direction lastClampedXDirection;

    private bool clampXBelowPosition;
    private float clampXBelowValue;
    private OnCollisionCustomAction rightCollisionAction;

    private bool clampYBelowPosition;
    [HideInInspector]
    public float clampYBelowValue;
    private OnCollisionCustomAction topCollisionAction;

    // Standard-Jump-related values
    [SerializeField]
    private int maxSuccessiveJumps;
    private int remainingJumps;
    [HideInInspector]
    public bool isJumping;

    // Wall-Jump Related values
    [HideInInspector]
    public int lastWallHitID;   // Identity of last Jump Wall Surface touched
    [HideInInspector]
    public int lastWallJumpPerformedID;  // Last wall where the player has made use of a walljump
    [HideInInspector]
    public bool canWallJump;
    private bool isWallJumping;

    [SerializeField]
    private float wallJumpDistance;
    [SerializeField] [Range(0.05f, 0.5f)]
    private float wallJumpDuration;
    private float wallJumpSpeed;

    // Speed-related values
    [SerializeField]
    private float maxSpeed;
    private Vector2 speed;
    public Vector2 Speed
    {
        get
        {
            return speed;
        }
        set
        {
            speed = value;
        }
    }
    [SerializeField]
    private float SprintingSpeedMultiplier;
    [HideInInspector]
    public bool isSprinting;

    // Dash-related values;
    [SerializeField] private float dashDistance;
    [SerializeField] private float dashDuration;
    private float dashSpeed;
    private bool isDashing;

    // BELOW THIS - ENGINE METHODS

    public void ClampXAbove(bool clampingXAbove, float clampValue = 0, OnCollisionCustomAction customAction = null)
    {
        clampXAbovePosition = clampingXAbove;
        clampXAboveValue = clampValue;
        leftCollisionAction = customAction;
    }

    public void ClampYAbove(bool clampingYAbove, float clampValue = 0, OnCollisionCustomAction customAction = null)
    {
        clampYAbovePosition = clampingYAbove;
        clampYAboveValue = clampValue;
        bottomCollisionAction = customAction;
    }

    public void ClampXBelow(bool clampingXBelow, float clampValue = 0, OnCollisionCustomAction customAction = null)
    {
        clampXBelowPosition = clampingXBelow;
        clampXBelowValue = clampValue;
        rightCollisionAction = customAction;
    }

    public void ClampYBelow(bool clampingYBelow, float clampValue = 0, OnCollisionCustomAction customAction = null)
    {
        clampYBelowPosition = clampingYBelow;
        clampYBelowValue = clampValue;
        topCollisionAction = customAction;
    }

    public void ResetJumpCount()
    {
        remainingJumps = maxSuccessiveJumps;
    }

    public void Jump()
    {
        if(remainingJumps > 0)
        {
            isJumping = true;
            remainingJumps--;
            PercentageOfGravity = -1.0f;

            if(canWallJump)
            {
                remainingJumps--;
                StartCoroutine(WallJump());
            }
        }
    }

    public IEnumerator WallJump()
    {
        isWallJumping = true;
        lastWallJumpPerformedID = lastWallHitID;
        wallJumpSpeed = wallJumpDistance / wallJumpDuration; 
        switch (lastClampedXDirection)
        {
            case DirectionsEnum.Direction.Left:
                wallJumpSpeed *= 1;
                break;
            case DirectionsEnum.Direction.Right:
                wallJumpSpeed *= -1;
                break;
            default:
                wallJumpSpeed *= 0;
                break;
        }

        yield return new WaitForSeconds(wallJumpDuration); 
        isWallJumping = false;
    }

    public IEnumerator Dash(DirectionsEnum.Direction direction)
    {
        if(!isDashing)
        {
            isDashing = true;
            dashSpeed = dashDistance / dashDuration;
          
            switch(direction)
            {
                case DirectionsEnum.Direction.Left:
                    dashSpeed *= -1;
                    break;
                case DirectionsEnum.Direction.Right:
                    break;
                default:
                    dashSpeed = 0;
                    break;
            }

            yield return new WaitForSeconds(dashDuration);
            isDashing = false;
        }
    }

    private void Start()
    {
        PercentageOfGravity = 1.0f;
        remainingJumps = maxSuccessiveJumps;
        isSprinting = false;
        isDashing = false;
        isJumping = false;
        isWallJumping = false;
    }

    private void Update()
    {
        Vector2 newTranslation = Vector2.zero;

        if (!ignoreGravity && PhysicsManager.Instance != null)
        {
            // Calculing Gravity Effet
            newTranslation -= new Vector2(0, PhysicsManager.Instance.GravityValue * PercentageOfGravity) * Time.deltaTime;
        }
        
        if(isJumping)
        {
            PercentageOfGravity = Mathf.Min(1.0f, PercentageOfGravity + heightOfJump * Time.deltaTime);
        }

        // Adding the player's movement on right/left and the sprint
        newTranslation += Speed * maxSpeed * Time.deltaTime *
                          ((isSprinting && !isJumping) ? SprintingSpeedMultiplier : 1);
        // Adding the dash effect
        newTranslation += Vector2.right * ((isDashing) ? dashSpeed : 0) * Time.deltaTime;

        // Adding the WallJump Effect;
        // newTranslation += Vector2.right * ((isWallJumping) ? wallJumpSpeed : 0) * Time.deltaTime;
        
        Position += newTranslation;

        // Restarting the walljump opportunity.
        canWallJump = false;

        // Clamping Position
        // Top Collider
        if (clampYBelowPosition && Position.y < clampYBelowValue)
        {
            ResetJumpCount();
            isJumping = false;
            lastWallJumpPerformedID = -1;   // Touching the ground restart all ability to walljump 
            if (topCollisionAction != null)
            {
                Position += topCollisionAction.OnCollisionDo();
            }
            Position = new Vector2(Position.x, clampYBelowValue);
        }
        // Bottom Collider
        if (clampYAbovePosition && Position.y > clampYAboveValue)
        {
            percentageOfGravity = Mathf.Max(0, percentageOfGravity);
            if (bottomCollisionAction != null)
            {
                Position += bottomCollisionAction.OnCollisionDo();
            }
            Position = new Vector2(Position.x, clampYAboveValue);
        }
        // Right Collider
        if (clampXBelowPosition && Position.x < clampXBelowValue)
        {
            lastClampedXDirection = DirectionsEnum.Direction.Left;
            if (rightCollisionAction != null)
            {
                Position += rightCollisionAction.OnCollisionDo();
            }
            Position = new Vector2(clampXBelowValue, Position.y);
        }
        // Left Collider
        if (clampXAbovePosition && Position.x > clampXAboveValue)
        {
            lastClampedXDirection = DirectionsEnum.Direction.Right;
            if (leftCollisionAction != null)
            {
                Position += leftCollisionAction.OnCollisionDo();
            }
            Position = new Vector2(clampXAboveValue, Position.y);
        }
    }
}
