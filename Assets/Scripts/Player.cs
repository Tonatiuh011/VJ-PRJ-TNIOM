using System;
using Edgar.Unity.Examples.Metroidvania;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : GameUnit
{
    [Header("Player - Movement")]
    public float jumpForce;
    public float jumpTime;
    public float dashingForce;
    public float dashingTime;

    // Player Actions
    public Action <float> hpChange { get; set; }
    private UnitAction<float> DashAction;
    private UnitAction<float> JumpAction;
    private UnitAction<string> AttackAction = null;
    
    // Trigger animations, set values, etc
    private Animator        animator;
    // Sensor class, easily manage collisions
    private Sensor  groundSensor;
    // HitBox sensor
    private Sensor hitbox;
    
    // Is grounded var
    private bool isGrounded = false;

    // MovX
    private float movX;
    private float MovX { get => movX; set => movX = value; }

    // MovY
    private float movY;
    private float MovY { get => movY; set => movY = value; }

    public override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
        groundSensor = transform.Find("GroundSensor").GetComponent<Sensor>();
        hitbox = transform.Find("Hitbox").GetComponent<Sensor>();
        hitbox.OnCollision = OnHitbox;
        DashAction = new UnitAction<float>(dashingTime, Dash);
        JumpAction = new UnitAction<float>(jumpTime, Jump);

        groundSensor.OnCollision = col =>
        {
            // Checking if foe just landed on ground
            if (!isGrounded && groundSensor.State())
            {
                isGrounded = true;
                animator.SetBool("Grounded", isGrounded);
            }
        };
    }

    public override void Update()
    {
        DashAction.Update();
        JumpAction.Update();
        AttackAction?.Update();

        base.Update();

        if (DashAction.Active)
            return;

        MovY = Velocity.y;
        float xDir = MovX;
        float yDir = MovY;

        int inputRaw = 0;

        if (xDir != 0)
            inputRaw = xDir > 0 ? 1 : -1;

        //// Checking if foe just landed on ground
        //if (!isGrounded && groundSensor.State())
        //{
        //    isGrounded = true;
        //    animator.SetBool("Grounded", isGrounded);
        //}

        // Checking if foe fall off
        if (isGrounded && !groundSensor.State())
        {
            isGrounded = false;
            animator.SetBool("Grounded", isGrounded);
        }
        
        if ((xDir != 0 || yDir != 0 || inputRaw != 0) && !DashAction.Active)
            Move(xDir, yDir, inputRaw);

        animator.SetFloat("AirSpeedY", yDir);
    }    

    public void OnTriggerEnter2D(Collider2D collision)
    {
        var gameObject = collision.gameObject;

        if(gameObject.tag == "Hitbox")
        {
            if(!Unit.IsDead && !DashAction.Active && !HitAction.Active)
            {
                var enemy = gameObject.GetComponentInParent<Enemy>();
                Vector2 direction = enemy.FacingDirection > 0 ? Vector2.left : Vector2.right;
                StopMovement(0.1f);
                Hit(enemy.Unit.Damage);
                Force(direction, enemy.pushForce, ForceMode2D.Impulse);
            }
        }
    }

    public void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();
        movX = Mathf.RoundToInt(movementVector.x);
    }

    public void OnJump(InputValue inputValue)
    {
        if (isGrounded && !JumpAction.Active)
            JumpAction.Exec(jumpForce);
    }

    public void OnDash(InputValue inputValue)
    {
        if (isGrounded && !DashAction.Active)
            DashAction.Exec(dashingForce);
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

    private void Dash(float force)
    {
        animator.SetTrigger("Dash");
        rb2D.velocity = new Vector2(transform.localScale.x * force, 0f);
        //(dashingCooldown);
    }

    private void Jump(float force)
    {
        isGrounded = false;
        animator.SetTrigger("Jump");
        animator.SetBool("Grounded", isGrounded);
        movY = force;
        Force(Vector2.up, movY, ForceMode2D.Impulse);
        groundSensor.Disable(0.2f);
    }

    private void Attack()
    {
        if (AttackAction == null || AttackAction?.Active == false)
        {
            var animName = "Attack-A";
            var len = Utils.GetClipLength(animator, animName);

            AttackAction =  new UnitAction<string>(len, name =>
            {
                animator.SetTrigger("AttackA");
                StopMovement(len);
            });

            AttackAction.Exec(animName);
        }
    }

    public void OnHitbox(Collider2D collider)
    {
        var obj = collider.gameObject;
        var enemy = obj.GetComponentInParent<Enemy>();

        if(enemy != null)
        {
            enemy.Hit(Unit.Damage);
            var direction = FacingDirection == 1 ? Vector2.right : Vector2.left;            
            enemy.Force(direction, 5.5f, ForceMode2D.Impulse);
        }
    }

    protected override void Death(UnitBase unit)
    {
        var len = Utils.GetClipLength(animator, "Death");
        StopMovement(len);
        animator.SetTrigger("Death");

        MetroidvaniaGameManager.Instance.LoadNextLevel();
    }

    protected override void OnHPChange(float hp)
    {
        hpChange ?.Invoke(hp);
    }

    public override void Hit(float damage)
    {
        base.Hit(damage);
        animator.SetTrigger("Hit");
    }
}