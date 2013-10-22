using UnityEngine;
using System.Collections;

public class Item50PenceShield : BaseShield
{
	#region Constructor
	
	public Item50PenceShield()
	{
		damageReduction = 0.5f;
		damageReductionChance = 0.4f;
	}
	
	#endregion
}
