using UnityEngine;
using System.Collections;

public class PlayerControls : MonoBehaviour {
	
	/*public float moveSpeed;
	public float jumpHeight;
	public float rotateSpeed;
	public float gravity;
	public float jumpSpeed;
	
	private Vector3 moveDirection = Vector3.zero;*/
	
	public bool canJump;
	
	//public Transform player;*/
	
	public Transform wAngle;
	public Transform sAngle;
	
	public float movementSpeed = 10;
    public float turningSpeed = 60;
	public float rotateSpeed;
	public float jumpHeight;
	
    void FixedUpdate() {
		
		if(Input.GetKey (KeyCode.W))
		{
     	 	transform.Translate(0, 0, movementSpeed);
			transform.eulerAngles = new Vector3(0, 0, 0);
			movementSpeed = .1f;
		}
		if(Input.GetKey (KeyCode.S))
		{
     	 	transform.Translate(0, 0, movementSpeed);
			transform.eulerAngles = new Vector3(0, 180, 0);
			movementSpeed = .1f;
		}
		if(Input.GetKey (KeyCode.D))
		{
     	 	transform.Translate(0, 0, movementSpeed);
			transform.eulerAngles = new Vector3(0, 90, 0);
			movementSpeed = .1f;
		}
		if(Input.GetKey (KeyCode.A))
		{
     	 	transform.Translate(0, 0, movementSpeed);
			transform.eulerAngles = new Vector3(0, -90, 0);
			movementSpeed = .1f;
		}
		
		if(Input.GetKey (KeyCode.W) && Input.GetKey (KeyCode.D))
		{
			transform.Translate(0, 0, movementSpeed / 2);
			transform.eulerAngles = new Vector3(0, 45, 0);
			movementSpeed = .05f;
		}
		if(Input.GetKey (KeyCode.W) && Input.GetKey (KeyCode.A))
		{
			transform.Translate(0, 0, movementSpeed / 2);
			transform.eulerAngles = new Vector3(0, -45, 0);
			movementSpeed = .05f;
		}
		if(Input.GetKey (KeyCode.S) && Input.GetKey (KeyCode.A))
		{
			transform.Translate(0, 0, movementSpeed / 2);
			transform.eulerAngles = new Vector3(0, -135, 0);
			movementSpeed = .05f;
		}
		if(Input.GetKey (KeyCode.S) && Input.GetKey (KeyCode.D))
		{
			transform.Translate(0, 0, movementSpeed / 2);
			transform.eulerAngles = new Vector3(0, 135, 0);
			movementSpeed = .05f;
		}
		
		if(Input.GetKey (KeyCode.Space))
		{
			if(canJump = true)
			{
				transform.Translate (Vector3.up * jumpHeight);
				canJump = false;
			}
		}
		
        //transform.Translate(0, Time.deltaTime, 0, Space.World);
    }
	
	// Update is called once per frame
	/*void FixedUpdate () {
		
		  CharacterController controller = GetComponent<CharacterController>();
        if (controller.isGrounded) {
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= moveSpeed;
            if (Input.GetButton("Jump"))
                moveDirection.y = jumpSpeed;
            
        }
        moveDirection.y -= gravity * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);*/
		
		//transform.rotation = Quaternion.LookRotation(rigidbody.velocity);
		
		//player.transform.rotation = Quaternion.Slerp(player.transform.rotation, 
		
		
	/*	if(Input.GetKey (KeyCode.W))
		{
			transform.Translate(Vector3.forward * moveSpeed);
		}
		if(Input.GetKey (KeyCode.S))
		{
			transform.Translate(Vector3.forward * -moveSpeed);
		}
		if(Input.GetKey (KeyCode.D))
		{
			transform.Translate(Vector3.right * moveSpeed);
		}
		if(Input.GetKey (KeyCode.A))
		{
			transform.Translate(Vector3.right * -moveSpeed);
		}
		if(Input.GetKey (KeyCode.Space))
		{
			if(canJump = true)
			{
				transform.Translate (Vector3.up * jumpHeight);
				canJump = false;
			}
		}
	}*/
	
	void OnCollisionEnter(Collision collision)
	{
		if(collision.gameObject.tag == "Ground")
		{
			canJump = true;
		}
	}
}
