using UnityEngine;
using System.Collections;

public class CameraGUI : MonoBehaviour 
{
	void OnGUI()
	{
		// Update Position and Rotation Debug Text
		 guiText.text = " Rotation: " + Camera.mainCamera.gameObject.transform.eulerAngles + " Height: " + Camera.mainCamera.gameObject.transform.position.y;
	}
}
