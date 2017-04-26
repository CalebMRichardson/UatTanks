using UnityEngine;
using System.Collections;
[RequireComponent (typeof(TankData))]

public class TankMotor : MonoBehaviour {

	//TankData data object
	public TankData data;

    //Number of points players will get for hitting Tank
    public int pointsPerHit; 

	public AudioClip tankDeathSound; 

	//functionallity variables
	private CharacterController characterController; 
	public Transform tf; 

	//Called at the start of this script
	void Start()
	{
		characterController = GetComponent<CharacterController> (); 

		//if data does not exist create it
		if (data == null)
		{
			data = GetComponent<TankData>(); 
		}
	}

	//Move Funtion
	public void Move(float moveSpeed)
	{
		//Create new SpeedVector
		Vector3 speedVector;
		//Get transfrom forward position
		speedVector = tf.forward;
		
		speedVector *= moveSpeed;
		//SimpleMove(speedVector) in meters per sec
		characterController.SimpleMove(speedVector);
	}

	//Roate Function
	public void Rotate(float rotateSpeed)
	{
		//Create new RotateVector
		Vector3 rotateVector;
		
		rotateVector = Vector3.up; 
		
		rotateVector *= rotateSpeed;
		
		rotateVector *= Time.deltaTime;
		
		tf.Rotate(rotateVector, Space.World);   //Rotate tank
	}

	//RotateTowardsObject
	public bool RotateTowards(Vector3 target, float speed)
	{
		Vector3 vectorToTarget; 

		vectorToTarget = target - tf.position; 

		Quaternion targetRotation = Quaternion.LookRotation (vectorToTarget); 

		tf.rotation = Quaternion.RotateTowards (tf.rotation, targetRotation, data.rotateSpeed * Time.deltaTime); 

		return false; 
	}

	//TankDamage
	public void TakeDamage(int damage)
	{
		data.health -= damage; 
	}

	void Update()
	{
		//if health of the tank is < 1
		if (data.health < 1)
		{
			//it its a player or enemy
			if (this.gameObject.CompareTag("Player") || this.gameObject.CompareTag("Enemy"))
			{

				tf.gameObject.SetActive(false); 
			}

			AudioSource.PlayClipAtPoint(tankDeathSound, tf.position, PlayerPrefs.GetFloat("FXVol"));

			data.lives -= 1; 
		}
	}
}
