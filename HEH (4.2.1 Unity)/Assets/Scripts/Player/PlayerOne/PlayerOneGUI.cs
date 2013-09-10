using UnityEngine;
using System.Collections;

public class PlayerOneGUI : MonoBehaviour {
	
	public Texture2D heartIndicatorOne;
	public Texture2D heartIndicatorTwo;
	public Texture2D heartIndicatorThree;

	void OnGUI()
	{
		GUI.DrawTexture(new Rect(10, 10, 60, 120), heartIndicatorOne, ScaleMode.ScaleToFit, true, 1.0F);
		GUI.DrawTexture(new Rect(80, 10, 60, 120), heartIndicatorTwo, ScaleMode.ScaleToFit, true, 1.0F);
		GUI.DrawTexture(new Rect(150, 10, 60, 120), heartIndicatorThree, ScaleMode.ScaleToFit, true, 1.0F);
	}
}
