using UnityEngine;
using System.Collections;

public class RotatePowerUp : MonoBehaviour {

	public Transform tf; 
	public float rotateSpeed; 
	
	// Update is called once per frame
	void Update () 
	{
		tf.Rotate(-Vector3.up * rotateSpeed * Time.deltaTime); 
	}
}
