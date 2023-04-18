using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public abstract class MovingObject : MonoBehaviour // Ya me awite, ok <3
{
    [Header("Movement Speed")]
    [SerializeField] float      maxSpeed = 4.5f;

    protected Collider2D        boxCollider;
    protected Rigidbody2D       rb2D;
    protected bool              isMoving = false;
    protected int               facingDirection = 1;

    private float disableMovement;
    //protected float movX;
    //private float movY;

    public Vector2 Velocity => rb2D.velocity;
    public Vector2 Position => rb2D.position;
    public int FacingDirection => facingDirection;

    public virtual void Start()
    {
        boxCollider = GetComponent<Collider2D>();
        rb2D = GetComponent<Rigidbody2D>();
    }

    public virtual void Update() => disableMovement -= Time.deltaTime;

    /// <summary>
    /// Move Object
    /// </summary>
    /// <param name="xDir"> X axis Float direction to move!!  </param>
    /// <param name="yDir"> Y axis Float direction to move!!  </param>
    /// <param name="inputRaw"> Facing direction of the object (Horizontal) </param>    
    protected void Move (float xDir, float yDir, int inputRaw)
    {
        // Attempting to move
        if (AttemptMove())
        {
            // Is moving validation?
            if (Mathf.Abs(inputRaw) > Mathf.Epsilon && Mathf.Sign(inputRaw) == facingDirection)
                isMoving = true;
            else
                isMoving = false;

            // Setting facing orientation properties
            if (inputRaw != 0)
            {
                facingDirection = inputRaw;
                OnSwapDirection(facingDirection);
            }

            // SlowDownSpeed decelarate objects when stoping
            // TODO: Try to add decelaration properties!
            float slowDownSpeed = isMoving ? 1.0f : 0.5f;

            // Movement Logic
            rb2D.velocity = new Vector2(xDir * maxSpeed * slowDownSpeed, yDir);
        }

        // After Movement Event
        OnMovement();
    }

    /// <summary>
    /// Moves the object to a target!!
    /// </summary>    
    /// <param name="target">Target aiming to move!</param>
    /// <param name="speed">Speed that the current object will move!!</param>
    protected void MoveTowards(Vector2 target, float speed)
    {
        // Attempting to move
        if (AttemptMove()) 
        {
            isMoving = !(target == null);
        
            if(isMoving)
            {
                float cX = Position.x, tX = target.x;
                // Getting facing direction from the X values of the two objects.
                // 1 left; -1 rigth
                facingDirection = cX > tX ? 1 : -1;
                OnSwapDirection(facingDirection);

                // Moving Towards!
                transform.position = Vector2.MoveTowards(Position, target, speed * Time.deltaTime);
            }

        }

        OnMovement();
    }

    /// <summary>
    /// Force
    /// </summary>
    /// <param name="vector"></param>
    /// <param name="force"></param>
    /// <param name="mode"></param>
    public void Force(Vector2 vector, float force, ForceMode2D mode) 
        => rb2D.AddForce(vector * force, mode);

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
    protected virtual bool AttemptMove()
    {
        if (disableMovement > 0)
            return false;

        return true;
    }

    /// <summary>
    /// On Movement Event
    /// </summary>
    protected abstract void OnMovement();

    /// <summary>
    /// StopMovement
    /// </summary>
    /// <param name="time">disable Movement</param>
    /// <returns></returns>
    protected void StopMovement(float time) { 
        rb2D.velocity = Vector2.zero;
        isMoving = false;
        disableMovement = time; 
    }
    
}