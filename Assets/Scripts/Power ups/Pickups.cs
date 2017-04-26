using UnityEngine;
using System.Collections;

public class Pickups : MonoBehaviour {
	//Powerups Scirpt obj
	public PowerUps powerup; 
	//Audio clip
	public AudioClip feedback; 
	//this.transform
	public Transform tf; 
	//SpawnPowerups obj
	public SpawnPowerups powerupsInGame; 
	//An arry of all the enmies
	private GameObject[] enemies; 

	void Start()
	{
		//find and populate enemies
		enemies = GameObject.FindGameObjectsWithTag("Enemy"); 
		//foreach enemy
		foreach (GameObject enemy in enemies)
		{
			//IngoreCollision of SPhere colider (Player Listener)
			Physics.IgnoreCollision(enemy.GetComponent<SphereCollider>(), GetComponent<Collider>()); 
		}
	}

	//OnTriggerEnter
	public void OnTriggerEnter(Collider collider)
	{
		//Get PowerupController
		PowerUpController powCon = collider.GetComponent<PowerUpController> ();

		//if trigger has a powCon Script (it's a tank)
		if (powCon != null)
		{
			powCon.Add(powerup); 

			//Play feedback
			if (feedback != null)
			{
				AudioSource.PlayClipAtPoint(feedback, tf.position, PlayerPrefs.GetFloat("FXVol")); 
			}

			Destroy(this.transform.parent.gameObject); 
			powerupsInGame.DecrementNumInGame(); 
		}
	}
}
