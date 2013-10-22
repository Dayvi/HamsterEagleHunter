using UnityEngine;
using System.Collections;

/*  NOTE:
 *  Later add in support so enemies get hit as the weapon swings e.g. float swingTime;
*/
public class BaseWeapon : BaseItem
{	
	public bool melee; // Is this weapon ranged or melee.
	public float range;
	public float directionRange;
	public float damage;
	
	public virtual void Attack()
	{
	}
}

