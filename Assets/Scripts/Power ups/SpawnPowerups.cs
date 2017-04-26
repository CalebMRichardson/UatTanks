using UnityEngine;
using System.Collections;
[System.Serializable]

public class SpawnPowerups  {

	private static int numInGame = 0; 

	public GameObject[] powerups; 

	public int NumberOnField()
	{
		return numInGame; 
	}

	public void DecrementNumInGame()
	{
		numInGame -= 1; 
	}

	public void IncrementNumInGame()
	{
		numInGame += 1; 
	}

	public GameObject GetPowerUpPrefab()
	{
		Random.seed = (int)System.DateTime.Now.Ticks;
		return powerups [Random.Range (0, powerups.Length)]; 
	}
}
