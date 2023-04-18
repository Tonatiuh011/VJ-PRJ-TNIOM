using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : Enemy
{
    [Header("Bird - Props")]
    public float Speed = 1.5f;

    public GameObject Sound1;

    // Is chasing
    private bool isChasing = false;
    // Sprite component to flip x
    private SpriteRenderer sprite;
    // Trigger animations, set values, etc
    private Animator animator;
    // Range Collider, triggers the collision with the target!
    private Collider2D rangeCollider;
    // Range Sensor
    private Sensor rangeSensor;

    public override void Start()
    {
        base.Start();
        sprite = GetComponent<SpriteRenderer>();
        var obj = transform.Find("RangeSensor");
        rangeCollider = obj.GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        rangeSensor = obj.GetComponent<Sensor>();
    }

    public override void Update()
    {
        base.Update();

        if (Target == null)
            return;

        if(rangeSensor.State())
            isChasing = true;

        if(!rangeSensor.State())
            isChasing = false;

        if (isChasing)
            MoveTowards(Target.Position, Speed);
        else
            MoveTowards(startPosition.position, Speed);

    }

    protected override void OnMovement()
    {
        //// Trigger animation by moving var!
        //if (isMoving)
        //    animator.SetInteger("State", 1);
        //else
        //    animator.SetInteger("State", 0);
    }

    protected override void OnSwapDirection(int direction)
    {
        if (direction > 0)
            sprite.flipX = true;
        else if (direction < 0)
            sprite.flipX = false;
    }

    protected override void Death(UnitBase unit) 
    {
        Instantiate(Sound1);
        var len = Utils.GetClipLength(animator, "Death");
        StopMovement(len);
        animator.SetTrigger("Death");
        Destroy(gameObject, len);
    }

    protected override void OnHPChange(float hp)
    {

    }
}