using UnityEngine;
using System.Collections;

public class PickUpWatch: MonoBehaviour {
	
	//public GameObject spawnPoint;
	
	public static bool watchPickup;
	
	void OnCollisionEnter(Collision collision)
	{
		if(collision.gameObject.tag == "Player")
		{
			Destroy (gameObject);
			watchPickup = true;
		}
	}
	
}
