using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MovingObject
{
    [Header("Variables")]
    public float jumpForce = 7.5f;
    public float jumpTime = 0.2f;
    public float dashingForce = 3.3f;
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

    // MovX
    private float movX;
    private float MovX { get => movX; set => movX = value; }

    // MovY
    private float movY;
    private float MovY { get => movY; set => movY = value; }    

    public void Update()
    {
        if (isDashing)
            return;

        MovY = Velocity.y;
        float xDir = MovX;
        float yDir = MovY;

        int inputRaw = 0;

        if (xDir != 0)
            inputRaw = xDir > 0 ? 1 : -1;        

        // Checking if foe just landed on ground
        if (!isGrounded && groundSensor.State())
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

        if (isGrounded && !Input.GetButton("Jump"))
            canDoubleJump = false;

        if ((xDir != 0 || yDir != 0 || inputRaw != 0) && !isDashing)
            Move(xDir, yDir, inputRaw);

        animator.SetFloat("AirSpeedY", yDir);
    }

    public override void Start()
    {
        base.Start();
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        groundSensor = transform.Find("GroundSensor").GetComponent<SensorMovement>();
    }   

    public void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();
        movX = Mathf.RoundToInt(movementVector.x);
        //movY = movementVector.y;
    }

    public void OnJump(InputValue inputValue)
    {
        if (isGrounded || canDoubleJump)
            StartCoroutine(Jump());
    }

    public void OnDash(InputValue inputValue)
    {
        if (canDash && isGrounded)
            StartCoroutine(Dash());
    }

    public void OnAttack(InputValue inputValue)
    {
        Attack();
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
        //float originalGravity = rb2D.gravityScale;
        //rb2D.gravityScale = 0f;
        rb2D.velocity = new Vector2(transform.localScale.x * dashingForce, 0f);
        yield return new WaitForSeconds(dashingTime);
        //rb2D.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;        
    }

    private IEnumerator Jump()
    {
        if (!canDoubleJump)
            animator.SetTrigger("Jump");
        else
            animator.SetTrigger("DoubleJump");

        isGrounded = false;
        animator.SetBool("Grounded", isGrounded);
        movY = jumpForce;
        Force(Vector2.up, movY, ForceMode2D.Impulse);
        groundSensor.Disable(0.2f);
        canDoubleJump = !canDoubleJump;
        yield return new WaitForSeconds(jumpTime);
    }

    private void Attack()
    {
        animator.SetTrigger("AttackA");
    }

    //void FixedUpdate()
    //{
    //}

}
