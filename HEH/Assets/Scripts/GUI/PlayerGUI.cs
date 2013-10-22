using UnityEngine;
using System.Collections;

public class PlayerGUI : MonoBehaviour 
{
	#region Fields
	
	// References
	public Texture2D heartTexture;
	private GameObject player;
	
	#endregion
	
	#region Unity Functions
	
	void Start()
	{
		player = GameObject.FindGameObjectWithTag("Player");
	}
	
	void OnGUI()
	{
		int heartPosition = 10;
		int textureWidth = 60;
		
		for (int i = 0; i < (int)player.GetComponent<PlayerController>().health; i++)
		{
			GUI.DrawTexture(new Rect(heartPosition, 10, textureWidth, 120), heartTexture, ScaleMode.ScaleToFit, true, 1.0F);
			heartPosition += textureWidth + 3;
		}
	}
	
	#endregion
}
