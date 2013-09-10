using UnityEngine;
using System.Collections;

public class FlyingEnemyBehavior : MonoBehaviour {
	
	public Transform player;
	private Vector3 dir;
	public float force = 500f;
	
	public static bool attackPlayer;
	
	public float moveSpeed;

	// Use this for initialization
	void Start () {
		
		attackPlayer = false;
	
	}
	
	// Update is called once per frame
	void Update () {
		
		Debug.Log (attackPlayer);
		if(attackPlayer == true)
		{
			dir = player.position - transform.position;
			transform.Translate(dir * moveSpeed * Time.deltaTime, Space.World);
			transform.LookAt(player);
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
}
