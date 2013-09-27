using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour 
{
	#region Fields
	
	public static bool worldPeace = false; // If for some reason we dont want any monster to attack the player e.g. cutscene
	
	// Variables
	private Vector3 dir;
	[System.NonSerialized]
	public float health;
	
	// [System.NonSerialized]
	public float attackTimer;
	
	public float moveSpeed;
	public float force = 500f;
	public float maxHealth;
	
	public float attackSpeed;
	public float attackRange;
	public float attackAngle;
	public float attackDamage;
	
	// References
	public Transform playerTransform;
	
	#endregion
	
	#region Unity Functions
	
	void Start () 
	{
		playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
		health = maxHealth;
		attackTimer = attackSpeed;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(!worldPeace)
		{
			dir = playerTransform.position - transform.position;
			transform.Translate(dir * moveSpeed * Time.deltaTime, Space.World);
			//transform.LookAt(playerTransform);
		}
		
		if (attackTimer > 0)
			attackTimer -= Time.deltaTime;
		else if (Attack())
		{
			attackTimer = attackSpeed; 
		}
		
		if (health <= 0)
			Destroy(this.gameObject);
	}
	
	void OnCollisionEnter(Collision collision)
	{
		Vector3 dir = collision.transform.position - transform.position;
		dir.y = 0;
		
		if(collision.rigidbody)
		{
			collision.rigidbody.AddForce(dir.normalized * force);
		}
		
		if(collision.gameObject.tag == "Player")
		{
			PlayerOneStats.damageTaken += 1;
		}
	}
	
	#endregion
	
	#region Functions
	
	protected virtual bool Attack()
	{
		GameObject player = GameObject.Find("Hadron");
		
		if (Vector3.Distance(transform.position, player.transform.position) <= attackRange)
		{
			// Check if the player is infront of us
			float direction = Vector3.Angle(transform.forward, player.transform.position - transform.position);
				
			if (direction < attackAngle)
			{
				player.GetComponent<PlayerControls>().health -= attackDamage;
				return true;
			}
		}
		
		return false;
	}
	
	#endregion
}
