using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : Enemy
{

    private Animator animator;
    private SpriteRenderer sprite;

    public override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }

    protected override void Death(UnitBase unit)
    {
        var len = Utils.GetClipLength(animator, "DeathEnemy");
        StopMovement(len);
        animator.SetTrigger("Death");
        Destroy(gameObject, len);
    }

    protected override void OnHPChange(float hp)
    {

    }

    protected override void OnMovement()
    {

    }

    protected override void OnSwapDirection(int direction)
    {
        if (direction > 0)
            sprite.flipX = true;
        else if (direction < 0)
            sprite.flipX = false;
    }

}