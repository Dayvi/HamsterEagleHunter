using UnityEngine;
using System.Collections;

public class Inventory : MonoBehaviour
{
	#region Fields
	
	public BaseWeapon weapon;
	public BaseShield shield;
	public BaseArmor armour;
	
	#endregion
	
	#region Unity Functions
	
	void Update()
	{
		// Update all the items in the inventory.
		if (weapon != null)
			weapon.Update();
	}
	
	#endregion
}
