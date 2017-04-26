using UnityEngine;
using System.Collections;

public class SpawnPoints : MonoBehaviour {
	//Functionality
	public Transform spawn; 
	public bool isFilled; 

	//Set SpawnPoint position / isFilled = false
	void Start()
	{
		spawn.position = this.transform.position; 
	}
	//Object has entered the spawn area
	void OnTriggerEnter(Collider collider)
	{
		if (collider.gameObject.name != "Floor")
			isFilled = true; 
	}

	//in case one object leaves the spawn area and another is still in there
	void OnTriggerStay(Collider collider)
	{
		if (collider.gameObject.name != "Floor")
			isFilled = true; 
	}

	//object has left the spawn area
	void OnTriggerExit(Collider collider)
	{
		if (collider.gameObject.name != "Floor")
			isFilled = false; 
	}
}
