using UnityEngine;
using System.Collections;

public class BaseShield : MonoBehaviour
{
	public float damageReduction;
	public float damageReductionChance;
	
	public virtual void Block(ref float damage)
	{
		if (Random.value <= damageReductionChance)
			damage -= damageReduction;
	}
}

