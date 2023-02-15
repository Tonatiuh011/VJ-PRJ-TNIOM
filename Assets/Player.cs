using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MovingObject
{
    [Header("Variables")]
    [SerializeField] float  jumpForce = 7.5f;
    [Header("Components")]
    // Sprite component to flip x
    private SpriteRenderer  sprite;
    // Trigger animations, set values, etc
    private Animator        animator;
    // Sensor class, easily manage collisions
    private SensorMovement  groundSensor;
    // Is grounded var
    private bool isGrounded = false;
    
    public override void Start()
    {
        base.Start();
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        groundSensor = transform.Find("GroundSensor").GetComponent<SensorMovement>();
    }
    
    void Update()
    {
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
            animator.SetBool("Grounded", isGrounded);
        }        

        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            animator.SetTrigger("Jump");
            isGrounded = false;            
            animator.SetBool("Grounded", isGrounded);
            yDir = jumpForce;
            groundSensor.Disable(0.2f);
        }

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
        if(direction > 0)        
            sprite.flipX = false;
        else if(direction < 0)        
            sprite.flipX = true;        
    }    
}
