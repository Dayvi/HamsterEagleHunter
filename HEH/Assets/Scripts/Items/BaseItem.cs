using UnityEngine;
using System.Collections;

public enum ItemRarity { Common, Uncommon, Rare };
public enum ItemType { Passive, Active }; // Change this so people dont get confused between item types of sword shield and item type of passive active

public class BaseItem
{
	#region Fields
	
	// Fields
	public ItemRarity itemRarity;
	public ItemType itemType;
	public float cooldown;
	public float uses; // -1 is infinte
	
	// Variables
	public float cooldownTimer = 0;
	
	#endregion
	
	#region Functions
	
	public void Update()
	{
		if (cooldown > 0)
			cooldown -= Time.deltaTime;
	}
	
	#endregion
}
