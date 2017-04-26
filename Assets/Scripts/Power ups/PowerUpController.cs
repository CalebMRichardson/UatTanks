using UnityEngine;
using System.Collections;
using System.Collections.Generic; 
[RequireComponent (typeof(PowerUps))]
[RequireComponent (typeof(TankData))]
public class PowerUpController : MonoBehaviour {

	public TankData data; 

	public List<PowerUps> powerups; 

	void Start()
	{
		powerups = new List<PowerUps> (); 
	}

	void Update()
	{
		List<PowerUps> expiredPowerups = new List<PowerUps> (); 
		foreach (PowerUps power in powerups) {
			power.duration -= Time.deltaTime; 

			if (power.duration <= 0) {
				expiredPowerups.Add (power); 
			}
		}
		//Decatvine and clear expired powerups
		foreach (PowerUps power in expiredPowerups) {
			power.OnDeactive (data); 
			powerups.Remove (power); 
		}
		//Clear Expired Powerups...just to be safe
		expiredPowerups.Clear (); 
	}

	public void Add(PowerUps powerup)
	{
		powerup.OnActive (data); 
		if (!powerup.isPermanent)
		{
			powerups.Add (powerup); 
		}
	}
}
