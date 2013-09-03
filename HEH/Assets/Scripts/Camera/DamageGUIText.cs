using UnityEngine;
using System.Collections;

public class DamageGUIText : MonoBehaviour {

	void OnGUI()
	{
		 guiText.text = "Damage: " + PlayerOneStats.damageTaken;
	}
}
