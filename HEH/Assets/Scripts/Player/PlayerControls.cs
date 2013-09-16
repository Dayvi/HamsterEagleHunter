using UnityEngine;
using System.Collections;

public class PlayerControls : MonoBehaviour 
{
	#region Fields
	
	// Variables
	bool canJump;
	bool canAttack = true;
	
	// Properties
	public float movementSpeed = 10;
    public float turningSpeed = 60;
	public float rotateSpeed;
	public float jumpHeight;
	public float attackDistance; // Later base this on the equipped weapon.
	
	#endregion
	
	#region Unity Functions
	
    void FixedUpdate() 
	{
		if (Input.GetKey(KeyCode.W))
		{
     	 	transform.Translate(0, 0, movementSpeed);
			transform.eulerAngles = new Vector3(0, 0, 0);
			movementSpeed = .1f;
		}
		if (Input.GetKey(KeyCode.S))
		{
     	 	transform.Translate(0, 0, movementSpeed);
			transform.eulerAngles = new Vector3(0, 180, 0);
			movementSpeed = .1f;
		}
		if (Input.GetKey(KeyCode.D))
		{
     	 	transform.Translate(0, 0, movementSpeed);
			transform.eulerAngles = new Vector3(0, 90, 0);
			movementSpeed = .1f;
		}
		if (Input.GetKey(KeyCode.A))
		{
     	 	transform.Translate(0, 0, movementSpeed);
			transform.eulerAngles = new Vector3(0, -90, 0);
			movementSpeed = .1f;
		}
		
		if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D))
		{
			transform.Translate(0, 0, movementSpeed / 2);
			transform.eulerAngles = new Vector3(0, 45, 0);
			movementSpeed = .05f;
		}
		if(Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A))
		{
			transform.Translate(0, 0, movementSpeed / 2);
			transform.eulerAngles = new Vector3(0, -45, 0);
			movementSpeed = .05f;
		}
		if(Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.A))
		{
			transform.Translate(0, 0, movementSpeed / 2);
			transform.eulerAngles = new Vector3(0, -135, 0);
			movementSpeed = .05f;
		}
		if(Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D))
		{
			transform.Translate(0, 0, movementSpeed / 2);
			transform.eulerAngles = new Vector3(0, 135, 0);
			movementSpeed = .05f;
		}
		
		if(Input.GetKey(KeyCode.Space))
		{
			if(canJump = true)
			{
				transform.Translate(Vector3.up * jumpHeight);
				canJump = false;
			}
		}
    }
	
	void OnCollisionEnter(Collision collision)
	{
		if(collision.gameObject.tag == "Ground")
		{
			canJump = true;
		}
	}
	
	#endregion
	
	#region Functions
	
	public void Attack()
	{
		RaycastHit hit;
		
		if (Physics.Raycast(transform.position, Vector3.forward, out hit, attackDistance))
		{
			// Check if the object we hit is an enemy
			if (hit.transform.tag == "Enemy")
				hit.transform.gameObject.GetComponent<EnemyController>().health -= 50;
		}
	}
	
	#endregion
}
