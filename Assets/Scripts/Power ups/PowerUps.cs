using UnityEngine;
using System.Collections;
[System.Serializable]

public class PowerUps {

	public float speedModifier = 0; 
	public int health = 0; 
	public float attackSpeedModifier = 0; 

	public float duration; 

	public bool isPermanent; 

	public void OnActive(TankData data)
	{
		data.moveSpeed += speedModifier; 
		data.reloadTime += attackSpeedModifier; 
		data.health += health; 
	}

	public void OnDeactive(TankData data)
	{
		data.moveSpeed += -speedModifier; 
		data.reloadTime += - attackSpeedModifier; 
	}
}
