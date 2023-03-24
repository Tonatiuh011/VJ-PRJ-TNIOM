using Assets.Scripts;
using Assets.Scripts.Classes;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : GameUnit
{
    [Header("Player - Movement")]
    public float jumpForce = 7.5f;
    public float jumpTime = 0.2f;
    public float dashingForce = 3.3f;
    public float dashingTime = 0.2f;
    public float dashingCooldown = 1f;
    
    // Trigger animations, set values, etc
    private Animator        animator;
    // Sensor class, easily manage collisions
    private Sensor  groundSensor;
    // HitBox sensor
    private Sensor hitbox;
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

    // diable actions 
    private float disableActions;

    public override void Update()
    {
        disableActions -= Time.deltaTime;
        base.Update();

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
        animator = GetComponent<Animator>();
        groundSensor = transform.Find("GroundSensor").GetComponent<Sensor>();
        hitbox = transform.Find("Hitbox").GetComponent<Sensor>();
        hitbox.OnCollision = OnHitbox;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        var gameObject = collision.gameObject;

        if(gameObject.tag == "Hitbox")
        {
            var enemy = gameObject.GetComponentInParent<Enemy>();
            Hit(enemy.Unit.Damage);
        }
    }

    public void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();
        movX = Mathf.RoundToInt(movementVector.x);
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
        rb2D.velocity = new Vector2(transform.localScale.x * dashingForce, 0f);
        yield return new WaitForSeconds(dashingTime);
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
        if (disableActions > 0.1)
            return;

        var animName = "Attack-A";
        animator.SetTrigger("AttackA");
        var len = Utils.GetClipLength(animator, animName);
        StopMovement(len);
        disableActions += len;
    }

    public void OnHitbox(Collider2D collider)
    {
        var obj = collider.gameObject;
        var enemy = obj.GetComponentInParent<Enemy>();
        if(enemy != null)
        {
            enemy.Hit(Unit.Damage);
        }
    }

    protected override void Death(UnitBase unit)
    {

    }

    protected override void OnHPChange(float hp)
    {

    }
}