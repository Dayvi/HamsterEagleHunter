using UnityEngine;
using System.Collections;

public class BaseConsumable : BaseItem
{
	public virtual bool Consume() // Return true if the item was consumed.
	{
		return true;
	}
}

