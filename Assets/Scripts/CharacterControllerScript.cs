using UnityEngine;
using System.Collections;

public class CharacterControllerScript : MonoBehaviour {

	public float maxSpeed = 10f;
	bool facingRight = true;

	Animator anim;

	bool grounded = false; 
	bool sidecollided = false; 

	public Transform groundCheck;
	public Transform Point1;
	public Transform Point2;

	float groundRadius = 0.5f;
	public LayerMask whatIsGround;
	public float jumpForce = 700f;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
	}

	void FixedUpdate(){
		sidecollided = Physics2D.OverlapArea( new Vector2(Point1.position.x, Point1.position.y),
		                                     new Vector2(Point2.position.x, Point2.position.y), whatIsGround);
		

		grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius,whatIsGround);
		anim.SetBool("Ground", grounded);
		anim.SetFloat("vSpeed", rigidbody2D.velocity.y); 

		float move = Input.GetAxis("Horizontal"); 
		anim.SetFloat("Speed", Mathf.Abs(move));

		if (!sidecollided)
			rigidbody2D.velocity = new Vector2( move * maxSpeed, rigidbody2D.velocity.y);

		if (move > 0 && !facingRight && grounded){
			Flip();
		} else if (move < 0 && facingRight && grounded){
			Flip();
		}

	}
	
	// Update is called once per frame
	void Update () {

		if (grounded && Input.GetKeyDown(KeyCode.Space)){
			anim.SetBool ("Ground", false);
			rigidbody2D.AddForce(new Vector2(0, jumpForce)); 
		}
	
		if (!grounded && Input.GetKey(KeyCode.Space)){
			//anim.SetBool ("Ground", false);
			rigidbody2D.AddForce(new Vector2(0, jumpForce / 50.0f)); 
		}

	}


	void Flip(){
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
}
