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
	bool moved = false;
	float idleAnimationTimer = 0;
	
	// Properties
	public float maxHealth;
	public float movementSpeed = 0;
    public float turningSpeed = 60;
	public float rotateSpeed;
	public float jumpHeight;
	public float attackDistance; // Later base this on the equipped weapon.
	public float attackDamage;
	
	#endregion
	
	#region Unity Functions
	
	void Start()
	{
		health = maxHealth;
		animation.Play("Idle_Reg");
	}
	
    void FixedUpdate() 
	{
		if (Input.GetAxis("Vertical") > 0)
		{
     	 	transform.Translate(0, 0, movementSpeed);
			transform.eulerAngles = new Vector3(0, 0, 0);
			movementSpeed = .1f;
			moved = true;
		}
		if (Input.GetAxis("Vertical") < 0)
		{
     	 	transform.Translate(0, 0, movementSpeed);
			transform.eulerAngles = new Vector3(0, 180, 0);
			movementSpeed = .1f;
			moved = true;
		}
		if (Input.GetAxis("Horizontal") > 0)
		{
     	 	transform.Translate(0, 0, movementSpeed);
			transform.eulerAngles = new Vector3(0, 90, 0);
			movementSpeed = .1f;
			moved = true;
		}
		if (Input.GetAxis("Horizontal") < 0)
		{
     	 	transform.Translate(0, 0, movementSpeed);
			transform.eulerAngles = new Vector3(0, -90, 0);
			movementSpeed = .1f;
			moved = true;
		}
		
		if (Input.GetAxis("Vertical") > 0 && Input.GetAxis("Horizontal") > 0)
		{
			transform.Translate(0, 0, movementSpeed / 4f);
			transform.eulerAngles = new Vector3(0, 45, 0);
			movementSpeed = .05f;
			moved = true;
		}
		if(Input.GetAxis("Vertical") > 0 && Input.GetAxis("Horizontal") < 0)
		{
			transform.Translate(0, 0, movementSpeed / 4f);
			transform.eulerAngles = new Vector3(0, -45, 0);
			movementSpeed = .05f;
			moved = true;
		}
		if(Input.GetAxis("Vertical") < 0 && Input.GetAxis("Horizontal") < 0)
		{
			transform.Translate(0, 0, movementSpeed / 4f);
			transform.eulerAngles = new Vector3(0, -135, 0);
			movementSpeed = .05f;
			moved = true;
		}
		if(Input.GetAxis("Vertical") < 0 && Input.GetAxis("Horizontal") > 0)
		{
			transform.Translate(0, 0, movementSpeed / 4f);
			transform.eulerAngles = new Vector3(0, 135, 0);
			movementSpeed = .05f;
			moved = true;
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
		
		if (moved != false)
		{
			animation.Play("Run");
			moved = false;
			idleAnimationTimer = 0;
		}
		else
		{
			if (idleAnimationTimer > 0)
			{
				idleAnimationTimer -= Time.deltaTime;
			}
			else
			{
				if (Random.value < 0.1)
				{
					animation.Play("Idle_Rare");
					idleAnimationTimer = animation.GetClip("Idle_Rare").length;
					Debug.Log("Idle_Rare playing");
				}
				else 
				{
					animation.Play("Idle_Reg");
					idleAnimationTimer = animation.GetClip("Idle_Reg").length * 5;
					Debug.Log("Idle_Reg playing");
				}
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
	
	void OnTriggerEnter(Collider other)
	{
		// Handle Pickups
		if (other.tag == "Pickup")
		{
			// Blue Berry
			if (other.transform.parent.name.Contains("BlueBerry"))
			{
				Destroy(other.transform.parent.gameObject);
				health += 0.5f;
				Debug.Log("Picked up fruit");
			}
			
			Debug.Log("Failed");
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
			enemiesInRange[i].GetComponent<EnemyController>().health -= attackDamage;
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
