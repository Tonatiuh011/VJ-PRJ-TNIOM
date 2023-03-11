using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MovingObject
{
    [Header("Variables")]
    public float  jumpForce = 7.5f;
    public float dashingPower = 24f;
    public float dashingTime = 0.2f;
    public float dashingCooldown = 1f;

    // Sprite component to flip x
    private SpriteRenderer  sprite;
    // Trigger animations, set values, etc
    private Animator        animator;
    // Sensor class, easily manage collisions
    private SensorMovement  groundSensor;    
    // Is grounded var
    private bool isGrounded = false;
    // Double Jump var
    private bool canDoubleJump;
    // Dash Variable
    private bool canDash = true;
    // Is dashing 
    private bool isDashing = false;

    
    public override void Start()
    {
        base.Start();
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        groundSensor = transform.Find("GroundSensor").GetComponent<SensorMovement>();
    }
    
    void Update()
    {
        if (isDashing)
            return;

        float xDir = Input.GetAxis("Horizontal");
        float yDir = rb2D.velocity.y;
        int inputRaw = (int)Input.GetAxisRaw("Horizontal");        

        // Checking if foe just landed on ground
        if(!isGrounded && groundSensor.State())
        {
            isGrounded = true;            
            animator.SetBool("Grounded", isGrounded);
        }
         
        // Checking if foe fall off
        if (isGrounded && !groundSensor.State())
        {
            isGrounded = false;
            canDoubleJump = true;
            animator.SetBool("Grounded", isGrounded);
        }

        if(isGrounded && !Input.GetButton("Jump"))
            canDoubleJump = false;


        if (Input.GetButtonDown("Jump"))
            if (isGrounded || canDoubleJump)
            {
                if (!canDoubleJump)
                    animator.SetTrigger("Jump");
                else
                    animator.SetTrigger("DoubleJump");

                isGrounded = false;            
                animator.SetBool("Grounded", isGrounded);
                yDir = jumpForce;
                groundSensor.Disable(0.2f);

                canDoubleJump = !canDoubleJump; 
            }

        if(Input.GetKeyDown(KeyCode.LeftShift) && canDash)        
            StartCoroutine(Dash());
        

        if(xDir != 0 || yDir != 0 || inputRaw != 0)
            Move(xDir, yDir, inputRaw);

        animator.SetFloat("AirSpeedY", yDir);
    }

    protected override bool AttemptMove(float xDir, float yDir)
    {
        return true;
    }

    protected override void OnMovement()
    {
        // Trigger animation by moving var!
        if (isMoving)
            animator.SetInteger("State", 1);
        else
            animator.SetInteger("State", 0);
    }

    protected override void OnSwapDirection(int direction)
    {
        var localScale = transform.localScale;

        if (direction > 0)
            localScale.x = Mathf.Abs(localScale.x) * 1f;            
        else if (direction < 0)            
            localScale.x = Mathf.Abs(localScale.x) * - 1;

        transform.localScale = localScale;
    }

    private IEnumerator Dash()
    {
        animator.SetTrigger("Dash");
        canDash = false;
        isDashing = true;
        float originalGravity = rb2D.gravityScale;
        rb2D.gravityScale = 0f;
        rb2D.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        yield return new WaitForSeconds(dashingTime);
        rb2D.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }
}
