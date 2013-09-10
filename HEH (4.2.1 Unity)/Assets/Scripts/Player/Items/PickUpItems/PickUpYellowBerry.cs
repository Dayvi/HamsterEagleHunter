using UnityEngine;
using System.Collections;

public class PickUpYellowBerry : MonoBehaviour {
	
	void OnCollisionEnter(Collision collision)
	{
		if(collision.gameObject.tag == "Player")
		{
			Destroy (gameObject);
			PlayerOneInventory.yBerryAmt += 1;
			Debug.Log ("Yellow Berry + " + PlayerOneInventory.yBerryAmt);
		}
	}
}
