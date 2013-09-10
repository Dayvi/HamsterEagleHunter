using UnityEngine;
using System.Collections;

public class AttackRadiusTrigger : MonoBehaviour {

	void OnTriggerEnter(Collider other)
	{
		//Debug.Log ("Attack player");
		if(other.gameObject.tag == "Player")
		{
			FlyingEnemyBehavior.attackPlayer = true;
			Debug.Log ("Attack player");
		}
	}
	
	void OnTriggerExit(Collider other)
	{
		if(other.gameObject.tag == "Player")
		{
			FlyingEnemyBehavior.attackPlayer = false;
			Debug.Log ("Stop attacking player");
		}
	}
	
}
