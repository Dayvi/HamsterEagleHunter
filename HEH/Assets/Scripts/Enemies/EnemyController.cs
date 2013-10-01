using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour 
{
	#region Fields
	
	public static bool worldPeace = false; // If for some reason we dont want any monster to attack the player e.g. cutscene
	
	// Variables
	private Vector3 dir;
	private float attackTimer;
	
	[System.NonSerialized] public float health;
	[System.NonSerialized] public bool stunned = false;
	
	public float moveSpeed;
	public float knockBackForce = 500f;
	public float maxHealth;
	
	public float attackSpeed;
	public float attackRange;
	public float attackAngle;
	public float attackDamage;
	
	public float berryDropPercentage;
	
	// References
	[System.NonSerialized]
	public Transform playerTransform;
	public GameObject berryPrefab;
	
	#endregion
	
	#region Unity Functions
	
	void Start () 
	{
		playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
		health = maxHealth;
		attackTimer = attackSpeed;
	}
	
	// Update is called once per frame
	void Update() 
	{
		if (!stunned)
		{
			if(!worldPeace)
			{
				dir = playerTransform.position - transform.position;
				dir.y = 0;
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
				stunned = true;
		}	
		else // Play animations and do post death things.
		{
			// Randomly drop a berry
			if (Random.value < berryDropPercentage / 100)
			{
				GameObject berry = (GameObject)GameObject.Instantiate(berryPrefab, transform.position, Quaternion.Euler(Vector3.zero));
				
				// Find the direction to send the berry
				Vector3 direction = -playerTransform.position - transform.position;	
				direction.y = 0;
				
				// Later make the final blow relative to how much force is applied
				berry.rigidbody.AddForce(direction.normalized * 5, ForceMode.Impulse);
			}
			
			Destroy(this.gameObject);
		}
	}
	
	void OnCollisionEnter(Collision collision)
	{
		if (!stunned)
		{
			Vector3 dir = collision.transform.position - transform.position;
			
			dir.y = 0;
		
			if(collision.rigidbody)
			{
				collision.rigidbody.AddForce(dir.normalized * knockBackForce, ForceMode.Impulse);
			}
		
			// Stats
			if(collision.gameObject.tag == "Player")
			{	
				// Fix this
			 	PlayerOneStats.damageTaken += 1;
			}
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
