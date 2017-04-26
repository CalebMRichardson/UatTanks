using UnityEngine;
using System.Collections;

public class TankData : MonoBehaviour {

	//Functionality Variable
	public float moveSpeed; 	//Tank MoveSpeed
	public float reverseSpeed;	//Tank ReverseSpeed
	public float rotateSpeed; 	//Tank RotateSpeed
	public float reloadTime;	//Tank Reload Time
	public float shellForce; 	//Shell Force 
	public int health; 			//Tank Health
	public int lives; 			//Tank Lives
    public int points; 
	public GameObject shellPrefab; 	//Tank Gun Shell GameObject
    public GameObject tank; 
}
