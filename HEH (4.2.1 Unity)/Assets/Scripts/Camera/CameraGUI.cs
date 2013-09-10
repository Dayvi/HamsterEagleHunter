using UnityEngine;
using System.Collections;

public class CameraGUI : MonoBehaviour {
	
	public Texture watchTexture;

	// Use this for initialization
	void OnGUI()
	{
		 guiText.text = " Rotation: " + Camera.mainCamera.gameObject.transform.eulerAngles + " Height: " + Camera.mainCamera.gameObject.transform.position.y;
		
		if(PickUpWatch.watchPickup == true)
		GUI.DrawTexture(new Rect(90, 100, 60, 60), watchTexture, ScaleMode.ScaleToFit, true, 1.0F);
	}
}
