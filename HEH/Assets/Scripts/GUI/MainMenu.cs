using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour 
{
	#region Fields
	
	private enum Menu { Title, Main, Options, Credits, NewGame, LoadGame };
	Menu currentMenu = Menu.Title;
	
	// References
	public Texture2D backgroundTexture;
	
	#endregion
	
	#region Unity Functions
	
	public void OnGUI()
	{
		// Draw the background
		GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), backgroundTexture);
		
		// Which menu are we on.
		if (currentMenu == Menu.Title)
		{
			#region Title Menu
			
			if (GUI.Button(new Rect(Screen.width / 2 - 50, 500, 100, 30), "Play"))
			{
				currentMenu = Menu.Main;
			}
			
			#endregion
		}
		else if (currentMenu == Menu.Main)
		{
			#region Main Menu
			
			if (GUI.Button(new Rect(Screen.width / 2 - 50, 400, 100, 30), "Continue"))
			{
				Application.LoadLevel("CameraTest");
			}
			
			if (GUI.Button(new Rect(Screen.width / 2 - 50, 440, 100, 30), "New Game"))
			{
				currentMenu = Menu.NewGame;
			}
			
			if (GUI.Button(new Rect(Screen.width / 2 - 50, 480, 100, 30), "Load Game"))
			{
				currentMenu = Menu.LoadGame;
			}
			
			if (GUI.Button(new Rect(Screen.width / 2 - 50, 520, 100, 30), "Options"))
			{
				currentMenu = Menu.Options;
			}
			
			if (GUI.Button(new Rect(Screen.width / 2 - 50, 560, 100, 30), "Credits"))
			{
				currentMenu = Menu.Credits;
			}
			
			#endregion
		}
		else if (currentMenu == Menu.NewGame)
		{
			#region New Game Menu
			
			Application.LoadLevel("CameraTest");
			
			#endregion
		}
		else if (currentMenu == Menu.LoadGame)
		{
			#region Load Game Menu
			#endregion
		}
		
		else if (currentMenu == Menu.Options)
		{
			#region Options Menu
			#endregion
		}
		else if (currentMenu == Menu.Credits)
		{
			#region Credits
			GUI.BeginGroup(new Rect(Screen.width / 2 - 150, 200, 300, 330));
			
			GUI.Box(new Rect(0, 0, 200, 330), "");
			
			GUI.Label(new Rect(0, 0, 100, 20), " Position: Name");
			GUI.Label(new Rect(0, 30, 100, 20), " Position: Name");
			GUI.Label(new Rect(0, 60, 100, 20), " Position: Name");
			GUI.Label(new Rect(0, 90, 100, 20), " Position: Name");
			GUI.Label(new Rect(0, 120, 100, 20), " Position: Name");
			GUI.Label(new Rect(0, 150, 100, 20), " Position: Name");
			GUI.Label(new Rect(0, 180, 100, 20), " Position: Name");
			GUI.Label(new Rect(0, 210, 100, 20), " Position: Name");
			GUI.Label(new Rect(0, 240, 100, 20), " Position: Name");
			GUI.Label(new Rect(0, 270, 100, 20), " Position: Name");
			GUI.Label(new Rect(0, 300, 100, 20), " Position: Name");
			GUI.Label(new Rect(0, 330, 100, 20), " Position: Name");	
			
			GUI.EndGroup();
			
			#endregion
		}
	}
	
	#endregion
}
