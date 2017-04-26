using UnityEngine;
using System.Collections;
[RequireComponent (typeof(TankData))]
[RequireComponent (typeof(TankMotor))]

public class TankGun : MonoBehaviour {

	//Tank Data Object
	private TankData data;
	//Tank Motor Object
	private TankMotor motor; 

	public Transform gunPosition; 

	public AudioClip gunFireSound; 

	//Functionality Variables
	public bool isLoaded; 

	void Start()
	{
		//If data == null GetComponent
		if (data == null)
		{
			data = GetComponent<TankData>(); 
		}
		//if motor == null GetComponent
		if (motor == null)
		{
			motor = GetComponent<TankMotor>(); 
		}
	}
	//Fire Gun
	public void Fire()
	{
		AudioSource.PlayClipAtPoint(gunFireSound, this.transform.position, PlayerPrefs.GetFloat("FXVol")); 
		//Set isLoaded to false
		isLoaded = false; 
		//create gameObject Shell
		GameObject shell = Instantiate (data.shellPrefab, gunPosition.position, Quaternion.identity) as GameObject;
		shell.transform.name = "Shell";
        
        TankShell shellScript = shell.GetComponent<TankShell>();

        shellScript.SetTankData(data); 

		//Add Rigidbody component to shell
		Rigidbody shellRb = shell.GetComponent<Rigidbody> ();
		shellRb.AddForce (motor.tf.forward * data.shellForce); 

		//Simulate Reload Speed
		StartCoroutine(WaitAndReload(data.reloadTime));  
	}

	//Simulate Reload Time
	IEnumerator WaitAndReload(float waitTime)
	{
		yield return new WaitForSeconds(waitTime); 
		isLoaded = true; 
	}
}
