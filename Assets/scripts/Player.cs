using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public float speed = 10f;
	public Vector2 maxVelocity = new Vector2(3, 5);
	public bool standing;
	public bool grounded;
	public float JumpForce = 50f;
	public float airSpeedMultiplier = .3f;
	public float distToGround;

	//private Animator animator;
	private PlayerController controller;
	private BoxCollider2D collider;

	void Start(){
		controller = GetComponent<PlayerController> ();
		collider = GetComponent<BoxCollider2D> ();
		//animator = GetComponent<Animator> ();
		distToGround = collider.bounds.extents.y;
	}

	bool IsGrounded() {
		//Physics.Raycast(new Ray())
		//return Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1);
		return true;
	}

	// Update is called once per frame
	void Update () {
		var forceX = 0f;
		var forceY = 0f;

		var absVelX = Mathf.Abs (GetComponent<Rigidbody2D>().velocity.x);
		var absVelY = Mathf.Abs (GetComponent<Rigidbody2D>().velocity.y);

		if (absVelY < .2f) {
			standing = true;
		} else {
			standing = false;
		}

		if (controller.moving.x != 0) {
			if (absVelX < maxVelocity.x) {

				forceX = standing ? speed * controller.moving.x : (speed * controller.moving.x * airSpeedMultiplier);

				transform.localScale = new Vector3 (forceX > 0 ? 1 : -1, 1, 1);
			}
			//animator.SetInteger ("AnimState", 1);
		} else {
			//animator.SetInteger ("AnimState", 0);
		}

		if (controller.moving.y > 0) {
			int layerMask = 1 << 8;
			/*if(collider.IsTouchingLayers(layerMask)){
				forceY = 100.0f;
			}*/
			float someValue = 0.6f;
			RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up, someValue, layerMask);
			if(hit.collider != null && grounded)
			{
				grounded = false;
				forceY = 200.0f;
			}else if(!grounded){
				grounded = true;
			}
			
		}

		GetComponent<Rigidbody2D>().AddForce (new Vector2 (forceX, forceY));
	}
}
