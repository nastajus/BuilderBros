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

	private GameObject goHolder;


	private float x;
	private float xx;
	private float y;
	private float yy;
	private float z;
	
	// Use this for initialization
	void Start () {
		goHolder = GameObject.Find("HoldUserBlocks");
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
		
		//if (!sidecollided)
		//	rigidbody2D.velocity = new Vector2( move * maxSpeed, rigidbody2D.velocity.y);

		if ( GameControl.instance.CurrentMode == State.BuildMode ||  GameControl.instance.CurrentMode == State.TestMode ){
			if (sidecollided && !grounded )
			{
				rigidbody2D.velocity = new Vector2( 0 , rigidbody2D.velocity.y);
			}
			else
			{
				rigidbody2D.velocity = new Vector2( move * maxSpeed, rigidbody2D.velocity.y);
			}
			
			if (move > 0 && !facingRight){
				Flip();
			} else if (move < 0 && facingRight){
				Flip();
			} 
		}	
	}
	
	// Update is called once per frame
	void Update () {

		//TODO: SHORTCIRCUIT IF TOO HIGH, MANUALLY IF NECESSARY, AT PERCENTAGE HEIGHT.
		//NOTE: uses both GetKeyDown AND GetKey .. should know why exactly!
		if (grounded && Input.GetKeyDown( GameControl.SemanticToKey[ SemanticAction.Jump ] )){
			anim.SetBool ("Ground", false);
			rigidbody2D.AddForce(new Vector2(0, jumpForce)); 
		}
		
		if (!grounded && Input.GetKey( GameControl.SemanticToKey[ SemanticAction.Jump ] )){
			//anim.SetBool ("Ground", false);
			rigidbody2D.AddForce(new Vector2(0, jumpForce / 50.0f)); 
		}
		


		x = Mathf.RoundToInt(transform.position.x); 
		xx=0;
		y = Mathf.RoundToInt(transform.position.y);
		yy=0;
		z = Mathf.RoundToInt(transform.position.z);

	
		if (Input.GetKeyDown ( GameControl.SemanticToKey[ SemanticAction.Build ] ) && GameControl.instance.CurrentMode == State.BuildMode ) {

			if ( GameControl.instance.PointsRemaining >= GameValues.items[ GameControl.instance.CurrentItem ] ){
				DestroyBlock();
				BuildBlock();
				GameControl.instance.PointsRemaining -= GameValues.items[ GameControl.instance.CurrentItem ];
			}
			else {
				//WHINE TO USER? or just be silent letting them wonder why they cannot build...?
			}
		}

		else if ( Input.GetKeyDown ( GameControl.SemanticToKey[ SemanticAction.Destroy ]) && GameControl.instance.CurrentMode == State.BuildMode ){
			DestroyBlock();
		}
		else if ( Input.GetKeyDown ( GameControl.SemanticToKey[ SemanticAction.NextItem ]) ){
			GameControl.instance.NextItem();
		}
		else if ( Input.GetKeyDown ( GameControl.SemanticToKey[ SemanticAction.PrevItem ]) ){
			GameControl.instance.PrevItem();
		}
	
	}

	void BuildBlock(){
		if (facingRight)  x++; else x--;
		goHolder = GameControl.instance.CreateHolder(); //TODO: MINOR: refactor this probably...
		GameObject go = (GameObject)Instantiate( GameControl.instance.TileItems[ GameControl.instance.CurrentItem ], new Vector3( x,y,z) , Quaternion.identity  ); //GameObject instance = (GameObject)
		go.transform.parent = goHolder.transform;

		GameControl.instance.PushUserGO(go);

	}

	void DestroyBlock(){
		if (facingRight)  {x++; xx++;} else {x--; xx--;}
		
		//detect object at position...
		Collider2D coll = Physics2D.OverlapArea( new Vector2( x,y ), new Vector2 ( x+xx, y+yy), whatIsGround );
		if ( coll ){
			Destroy ( coll.transform.gameObject );
			//search toolbox items for match.. if yes woohoo reclaim those points!!... um by magic of hardcoding indxes

			//coll.name

			//TODO: Ask Dual Team ZackDante if better way to write this:
			if (coll.name.Contains("(Clone)"))
			{
				string targetName = ""; 
				targetName = (coll.name).Substring(0, (coll.name).Length - "(Clone)".Length ) ;
				GameObject match = GameControl.instance.TileItems.Find(
					dingus => dingus.name == targetName
				);
				if (match != null) 
				{ 
					int indexMatch = GameControl.instance.TileItems.IndexOf(match);
					//return points
					GameControl.instance.PointsRemaining += GameValues.items[indexMatch];
				}
			}
		}
	}

	void OnDrawGizmos(){
		Gizmos.DrawLine( new Vector3( x,y,z), new Vector3( x+xx,y+yy,z)  );
	}


	/*
	void OnPostRender(){
		GL.Begin( GL.LINES );
		GL.Color( new Color(1f,1f,1f,0.5f) );
		GL.Vertex3( 0, 0, 0 );
		GL.Vertex3( 1, 0, 0 );
		GL.Vertex3( 0, 1, 0 );
		GL.Vertex3( 1, 1, 0 );
		GL.Color( new Color(0f,0f,0f,0.5f) );
		GL.Vertex3( 0, 0, 0 );
		GL.Vertex3( 0, 1, 0 );
		GL.Vertex3( 1, 0, 0 );
		GL.Vertex3( 1, 1, 0 );
		GL.End();
	}
	*/
	
	void Flip(){
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
}
