using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class Skeleton : Enemy
{
    [Header("Movement Speed")]
    public float walkDistance = 7f;
    public float speed = 5f;

    private Animator animator;
    private SpriteRenderer sprite;
    private GameObject pos1, pos2;

    private bool walkingMode = true;

    public override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();

        pos1 = new GameObject();
        pos2 = new GameObject();

        pos1.name = name + "_pos1";
        pos2.name = name + "_pos2";

        pos1.transform.position = new Vector3(startPosition.position.x - walkDistance, transform.position.y);
        pos2.transform.position = new Vector3(startPosition.position.x + walkDistance, transform.position.y);
    }


    public override void Update()
    {
        base.Update();
        Vector2 target;

        if(!HitAction.Active && (pos1 != null && pos2 != null))
        {
            if ((int)Position.x == (int)pos1.transform.position.x)
            {
                walkingMode = true;
            }

            if ((int)Position.x == (int)pos2.transform.position.x)
            {
                walkingMode = false;
            }

            target = walkingMode ? Vector2.right : Vector2.left;
            Move(target.x, Velocity.y, -1*(int)target.x);            
        }        
    }

    protected override void Death(UnitBase unit)
    {
        var len = Utils.GetClipLength(animator, "DeathEnemy");
        StopMovement(len);
        animator.SetTrigger("Death");
        Destroy(gameObject, len);
        Destroy(pos1);
        Destroy(pos2);
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
            sprite.flipX = false;
        else if (direction < 0)
            sprite.flipX = true;
    }

}