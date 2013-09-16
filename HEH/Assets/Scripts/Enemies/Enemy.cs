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
	
	// Properties 
	public float moveSpeed;
	public float force = 500f;
	public float maxHealth;
	
	// References
	public Transform playerTransform;
	
	#endregion
	
	#region Unity Functions
	
	void Start () 
	{
		playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(!worldPeace)
		{
			dir = playerTransform.position - transform.position;
			transform.Translate(dir * moveSpeed * Time.deltaTime, Space.World);
			transform.LookAt(playerTransform);
		}
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
	#endregion
}
