using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemSpear : BaseWeapon
{
	public ItemSpear()
	{
		itemRarity = ItemRarity.Common;
		itemType   = ItemType.Active;
		cooldown   = 1;
		uses       = -1;
		
		melee = true;
		range = 2;
		directionRange = 75;
		damage = 1;
	}
	
	public override void Attack()
	{
		if (cooldownTimer > 0)
		{
			return;
			Debug.Log("Still Cooling");
		}
			
		Transform playerTransform = GameObject.Find("Hadron").GetComponent<Transform>();
		
		// Get a list of all the enemies on the field that are within attacking distance.
		List<GameObject> enemies = new List<GameObject>();
		enemies.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
		List<GameObject> enemiesInRange = new List<GameObject>();
		
		for (int i = 0; i < enemies.Count; i++)
		{
			if (Vector3.Distance(playerTransform.position, enemies[i].transform.position) <= range)
			{
				// Check if the enemy is infront of us
				//float direction = (enemies[i].transform.position - transform.position).normalized.y;
				float direction = Vector3.Angle(playerTransform.forward, enemies[i].transform.position - playerTransform.position);
				
				if (direction < directionRange)
					enemiesInRange.Add(enemies[i]);
			}
		}
		
		for (int i = 0; i < enemiesInRange.Count; i++)
		{
			enemiesInRange[i].GetComponent<EnemyController>().health -= damage;
			Debug.Log("Attacked");
		}
		
		cooldownTimer = cooldown;
	}
}
