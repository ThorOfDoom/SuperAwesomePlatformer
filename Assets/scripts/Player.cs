using UnityEngine;
using System.Collections;


//FIXME strip it down!
/*  
 *  the player proably needs:
 *      - mass
 *      - maxVelocity
 *      - speed/acceleration
 *      - isGrounded
 *      - isJumping 
 */ 
public class Player : MonoBehaviour
{
    // TODO sort the fields!
    //chanage the max velocity and not the run speed thingy
    public float runningForce = 30f;
    public float maxRunningSpeed = 9.0f;
    public float maxWalkingSpeed = 3.0f;
    public bool grounded;
    public float airSpeedMultiplier = .3f;
    public float jumpForce;
    public float reactivityPercentage;
    public bool changedDirection = false;
    public float maxAirTime;
    public float minAirTime;

    public LayerMask groundCollisionLayerMask;

    //private Animator animator;
    private PlayerController controller;
    private BoxCollider2D collider;
    private Rigidbody2D rigidbody;
    private Vector2 absoluteVelocity;
    private float groundCheckRadius = 0.1f;
    private bool shouldMove = false;
    private bool shouldRun = false;
    private bool shouldJump = false;
    private bool isJumping = false;
    private Vector2 force;
    private int currentJumpHeight = 0;
    private float lastMovingX;
    private float airTime = 0.0f;
    private float maxSpeed = 0.0f;

    //DEBUG
    private Vector2 oldPos;
    public float velY;

    void Start ()
    {
        QualitySettings.vSyncCount = 1;
        controller = GetComponent<PlayerController> ();
        collider = GetComponent<BoxCollider2D> ();
        rigidbody = GetComponent<Rigidbody2D> ();
        //animator = GetComponent<Animator> ();

        oldPos = rigidbody.position;
    }

    /*
     * update physics in here
     */
    void FixedUpdate ()
    {
        force.x = 0.0f;
        force.y = 0.0f;

        absoluteVelocity.x = Mathf.Abs (rigidbody.velocity.x);
        absoluteVelocity.y = Mathf.Abs (rigidbody.velocity.y);
        velY = absoluteVelocity.x;

        grounded = Physics2D.OverlapCircle (collider.bounds.min, groundCheckRadius, groundCollisionLayerMask);

        if (shouldRun && grounded) {
            maxSpeed = maxRunningSpeed;
        } else {
            maxSpeed = maxWalkingSpeed;
        }
        if (shouldMove && absoluteVelocity.x < maxSpeed) {
            force.x = runningForce * controller.moving.x;
            transform.localScale = new Vector3 (force.x > 0 ? 1 : -1, 1, 1);
        }

        if (((!shouldMove && absoluteVelocity.x != 0 && grounded) || changedDirection) && absoluteVelocity.x < maxSpeed + (grounded ? 0 : 1)) {
            rigidbody.AddForce (new Vector2 (rigidbody.velocity.x * -reactivityPercentage, 0.0f), ForceMode2D.Impulse);
            changedDirection = false;
        }
        // TODO do a good jump function
        /*
         * keep accelrating til the player liftsthe key, the maxheight is reached or the time for thatran out
         */

        if (shouldJump && grounded) {
            isJumping = true;
        }

        if (isJumping) {
            if (!grounded) {
                airTime += Time.fixedDeltaTime;
            } 
            if ((airTime < maxAirTime && shouldJump)) {
                rigidbody.velocity = new Vector2 (rigidbody.velocity.x, jumpForce);
            } else {
                isJumping = false;
                shouldJump = false;
                airTime = 0.0f;
            }
        } 

        Debug.DrawLine (oldPos, rigidbody.position, Color.green, 5.0f);
        oldPos = rigidbody.position;



        
        rigidbody.AddForce (new Vector2 (force.x, force.y));
    }

    // Update is called once per frame
    void Update ()
    {
        if (controller.moving.x != 0) {
            shouldMove = true;
            if (lastMovingX != controller.moving.x) {
                lastMovingX = controller.moving.x;
                changedDirection = true;
            }
            //animator.SetInteger ("AnimState", 1);
        } else {
            shouldMove = false;
            //animator.SetInteger ("AnimState", 0);
        }

        if (controller.moving.y == 1 && grounded) {
            shouldJump = true;
        }
        if (controller.moving.y == 2 && isJumping) {
            shouldJump = false;
        }
        if (controller.running && shouldMove) {
            shouldRun = true;
        } else {
            shouldRun = false;
        }
    }

}

















