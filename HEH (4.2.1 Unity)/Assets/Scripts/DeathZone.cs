using UnityEngine;
using System.Collections;

public class DeathZone : MonoBehaviour {
	
	public GameObject spawnPoint;
	public GameObject player;

	// Use this for initialization
	void OnCollisionEnter(Collision collision)
	{
		if(collision.gameObject.tag == "Player")
		{
			collision.transform.position = spawnPoint.transform.position;
			PlayerOneStats.damageTaken = 0;
			player.rigidbody.velocity = new Vector3(0, 0, 0);
		}
	}
}
