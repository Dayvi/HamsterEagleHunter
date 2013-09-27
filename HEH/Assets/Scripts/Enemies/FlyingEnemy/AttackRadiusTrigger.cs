using UnityEngine;
using System.Collections;

public class AttackRadiusTrigger : MonoBehaviour 
{
	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.tag == "Player")
		{
			Debug.Log ("Attack player");
		}
	}
	
	void OnTriggerExit(Collider other)
	{
		if(other.gameObject.tag == "Player")
		{
			Debug.Log ("Stop attacking player");
		}
	}
}
