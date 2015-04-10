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
    public float runningForce = 30f;
    public Vector2 maxVelocity = new Vector2 (9, 15);
    public bool grounded;
    public float airSpeedMultiplier = .3f;
    public float jumpForce;
    public int maxJumpHeight;
    public float reactivityPercentage;
    public bool changedDirection = false;

    public LayerMask groundCollisionLayerMask;

    //private Animator animator;
    private PlayerController controller;
    private BoxCollider2D collider;
    private Rigidbody2D rigidbody;
    private Vector2 absoluteVelocity;
    private float groundCheckRadius = 0.1f;
    private bool moving = false;
    private bool jumping = false;
    private Vector2 force;
    private int currentJumpHeight = 0;
    private float lastMovingX;
    private float distanceToGround;

    void Start ()
    {
        controller = GetComponent<PlayerController> ();
        collider = GetComponent<BoxCollider2D> ();
        rigidbody = GetComponent<Rigidbody2D> ();
        //animator = GetComponent<Animator> ();
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

        grounded = Physics2D.OverlapCircle (collider.bounds.min, groundCheckRadius, groundCollisionLayerMask);

        if (jumping && !grounded) {
            RaycastHit2D hit = Physics2D.Raycast (collider.bounds.min, -Vector2.up, groundCollisionLayerMask);

            distanceToGround = hit.collider != null ? Mathf.Abs (transform.position.y - hit.transform.position.y) : 0.0f;
            Debug.Log (distanceToGround);



        } else {
            distanceToGround = 0.0f;
        }

        if (moving && absoluteVelocity.x < maxVelocity.x) {
            force.x = runningForce * controller.moving.x;
            //force.x = changedDirection ? runningForce * controller.moving.x : runningForce * controller.moving.x * (1.0f * reactivityPercentage);
            // turns the sprite around
            transform.localScale = new Vector3 (force.x > 0 ? 1 : -1, 1, 1);
        }
        if ((!moving && absoluteVelocity.x != 0 && grounded) || changedDirection) {
            rigidbody.AddForce (new Vector2 (rigidbody.velocity.x * -reactivityPercentage, 0.0f), ForceMode2D.Impulse);
            changedDirection = false;
        }

        if (jumping) {
            if (distanceToGround < maxJumpHeight && absoluteVelocity.y < maxVelocity.y) {
                force.y = jumpForce;
            } else {
                jumping = false;
            }
        }
        rigidbody.AddForce (new Vector2 (force.x, force.y));
        //Debug.Log (grounded + "|" + absoluteVelocity.y);
    }

    // Update is called once per frame
    void Update ()
    {
        if (controller.moving.x != 0) {
            moving = true;
            if (lastMovingX != controller.moving.x) {
                lastMovingX = controller.moving.x;
                changedDirection = true;
            }
            //animator.SetInteger ("AnimState", 1);
        } else {
            moving = false;
            //animator.SetInteger ("AnimState", 0);
        }

        if (controller.moving.y > 0 && grounded) {
            jumping = true;
        }
        if (controller.moving.y == 2 && !grounded) {
            jumping = false;
        }
    }

}

















