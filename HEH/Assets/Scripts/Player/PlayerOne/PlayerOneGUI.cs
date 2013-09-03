using UnityEngine;
using System.Collections;

public class PlayerOneGUI : MonoBehaviour {
	
	public Texture2D heartIndicator;

	void OnGUI()
	{
		GUI.DrawTexture(new Rect(10, 10, 60, 120), heartIndicator, ScaleMode.ScaleToFit, true, 1.0F);
		GUI.DrawTexture(new Rect(80, 10, 60, 120), heartIndicator, ScaleMode.ScaleToFit, true, 1.0F);
		GUI.DrawTexture(new Rect(150, 10, 60, 120), heartIndicator, ScaleMode.ScaleToFit, true, 1.0F);
	}
}
