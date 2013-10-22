using UnityEngine;
using System.Collections;

public class BaseArmor : BaseItem
{
	#region Fields
	
	float defaultHealth;
	
	float health; // How much health the armour has left.
	
	#endregion
	
	#region Constructor
	
	public BaseArmor()
	{
		health = defaultHealth;
	}
	
	#endregion
}

