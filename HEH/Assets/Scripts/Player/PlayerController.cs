using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour 
{
	#region Fields
	
	// Variables
	public float health;
	public float flask;
	
	private bool canJump;
	private bool canAttack = true;
	private bool moved = false;
	public  bool dead = false;
	
	private float idleAnimationTimer = 0;
	
	// Properties
	public float maxHealth;
	public float maxFlask;
	public float movementSpeed = 0;
    public float turningSpeed = 60;
	public float rotateSpeed;
	public float jumpHeight;
	public float attackDistance; // Later base this on the equipped weapon.
	public float attackDamage;
	
	// References
	Inventory inventory;
	
	#endregion
	
	#region Properties
	#endregion
	
	#region Unity Functions
	
	void Start()
	{	
		// Get Components
		inventory = GetComponent<Inventory>();
			
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
				}
				else 
				{
					animation.Play("Idle_Reg");
					idleAnimationTimer = animation.GetClip("Idle_Reg").length * 5;
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
			else if (other.transform.parent.GetComponent<Item>()) // If the pickup is an item.
			{
				System.Type itemType = System.Type.GetType(other.transform.parent.GetComponent<Item>().itemName);
				
				// If weapon, equip the item NOTE: Later add a pickup button
				if (itemType.IsSubclassOf(typeof(BaseWeapon)))
				{
					GetComponent<Inventory>().weapon = (BaseWeapon)System.Activator.CreateInstance(itemType);
					Debug.Log("Currently Equipped: " + GetComponent<Inventory>().weapon.GetType().ToString());
				}
				else if (itemType.IsSubclassOf(typeof(BaseShield)))
				{
					GetComponent<Inventory>().shield = (BaseShield)System.Activator.CreateInstance(itemType);
					Debug.Log("Currently Equipped: " + GetComponent<Inventory>().shield.GetType().ToString());
				}
				
				Destroy(other.transform.parent.gameObject);
			}
		}
	}
	
	#endregion
	
	#region Functions
	
	public void Attack()
	{			
		inventory.weapon.Attack();
	}
	
	public void Damage(float damage)
	{
		// Calculate Damage.
		if (inventory.shield != null)
			inventory.shield.Block(ref damage);
		
		health -= damage; // Apply Damage Calculations.
		
		// Check if the player is dead.
		if (health <= 0)
			dead = true;
	}
	
	#endregion
}
