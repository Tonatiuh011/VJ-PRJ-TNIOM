using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovingObject : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] float      maxSpeed = 4.5f;
    [Header("Components")]
    protected BoxCollider2D     boxCollider;
    protected Rigidbody2D       rb2D;    
    protected bool              isMoving = false;
    protected int               facingDirection = 1;
    //[SerializeField] float      jumpForce = 7.5f;
    //protected float             disableMovementTimer = 0.0f;
    //protected bool              isGrounded = false;

    public virtual void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        rb2D = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// Move Object
    /// </summary>
    /// <param name="xDir"> X axis Float direction to move!!  </param>
    /// <param name="yDir"> Y axis Float direction to move!!  </param>
    /// <param name="inputRaw"> Facing direction of the object (Horizontal) </param>    
    protected void Move (float xDir, float yDir, int inputRaw)
    {
        // Attempting to move
        if (!AttemptMove(xDir, yDir))
            return;

        // Is moving validation?
        if (Mathf.Abs(inputRaw) > Mathf.Epsilon && Mathf.Sign(inputRaw) == facingDirection)        
            isMoving = true;
        else
            isMoving = false;

        // Setting facing orientation properties
        if(inputRaw != 0)
        {
            facingDirection = inputRaw;
            OnSwapDirection(facingDirection);
        }        

        // SlowDownSpeed decelarate objects when stoping
        // TODO: Try to add decelaration properties!
        float slowDownSpeed = isMoving ? 1.0f : 0.5f;

        // Movement Logic
        rb2D.velocity = new Vector2(xDir * maxSpeed * slowDownSpeed, yDir);

        // After Movement Event
        OnMovement();
    } 

    /// <summary>
    /// On Swap Direction Event
    /// </summary>
    /// <param name="direction">Facing Direction</param>
    protected abstract void OnSwapDirection(int direction);

    /// <summary>
    /// Attempt Movement Event
    /// </summary>
    /// <param name="xDir">Foat X position</param>
    /// <returns>True if it can move, if not false</returns>
    protected abstract bool AttemptMove(float xDir, float yDir);

    /// <summary>
    /// On Movement Event
    /// </summary>
    protected abstract void OnMovement();
}

/*
 * Non Used Fucntions
    //protected virtual void AttemptMove<T>(int xDir, int yDir) where T : Component
    //{
    //    RaycastHit2D hit;

    //    bool canMove;
    //}

    //protected abstract void OnCantMove<T>(T component)
    //   where T : Component;
 */
