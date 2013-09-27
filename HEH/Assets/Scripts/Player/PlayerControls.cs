using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerControls : MonoBehaviour 
{
	#region Fields
	
	// Variables
	bool canJump;
	bool canAttack = true;
	public float health;
	
	// Properties
	public float maxHealth;
	public float movementSpeed = 10;
    public float turningSpeed = 60;
	public float rotateSpeed;
	public float jumpHeight;
	public float attackDistance; // Later base this on the equipped weapon.
	
	#endregion
	
	#region Unity Functions
	
	void Start()
	{
		health = maxHealth;
	}
	
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
		
		if(Input.GetKeyUp(KeyCode.P))
		{
			Attack();
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
		
		// Get a list of all the enemies on the field that are within attacking distance
		List<GameObject> enemies = new List<GameObject>();
		enemies.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
		List<GameObject> enemiesInRange = new List<GameObject>();
		
		for (int i = 0; i < enemies.Count; i++)
		{
			if (Vector3.Distance(transform.position, enemies[i].transform.position) <= attackDistance)
			{
				// Check if the enemy is infront of us
				//float direction = (enemies[i].transform.position - transform.position).normalized.y;
				float direction = Vector3.Angle(transform.forward, enemies[i].transform.position - transform.position);
				
				if (direction < 75) // Bigger than -75 but less than 75 degrees
					enemiesInRange.Add(enemies[i]);
			}
		}
		
		for (int i = 0; i < enemiesInRange.Count; i++)
		{
			enemiesInRange[i].GetComponent<EnemyController>().health -= 50;
		}
		
		/*RaycastHit hit;
		Vector3 rayStart = transform.position;
		rayStart.y += 1;
		
		if (Physics.SphereCast(rayStart, attackDistance, Vector3.forward, out hit, 10, LayerMask.NameToLayer("EnemyCombatColliders")))
		{
			Debug.Log("Hit: " + hit.transform.tag);
			// Check if the object we hit is an enemy
			if (hit.transform.tag == "Enemy")
			{
				hit.transform.gameObject.GetComponent<EnemyController>().health -= 50;
				Debug.Log("Attack Hit");
			}
		}
		*/
	}
	
	#endregion
}
