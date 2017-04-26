using UnityEngine;
using System.Collections;
[RequireComponent (typeof(TankData))]
[RequireComponent (typeof(TankMotor))]
[RequireComponent (typeof(TankGun))]

public class InputHandler : MonoBehaviour {

	//Script Objects
	private TankData data; 
	private TankMotor motor;
	private TankGun gun; 
	//InputScheme
	public enum InputScheme { WASD, ArrowKeys };
	public InputScheme input = InputScheme.WASD; 

	//Initialize Variables
	void Start()
	{
		//if scripts are null set them
		if (data == null)
		{
			data = GetComponent<TankData>();
		}

		if (motor == null)
		{
			motor = GetComponent<TankMotor>(); 
		}

		if (gun == null)
		{
			gun = GetComponent<TankGun>(); 
		}
	}

	//Run once per frame and check for user input
	void Update()
	{
		switch (input) 
		{
			case InputScheme.WASD:
			{
				if (Input.GetKey(KeyCode.W))
				{
					motor.Move(data.moveSpeed); 
				}
				if (Input.GetKey(KeyCode.A))
				{
					motor.Rotate(-data.rotateSpeed);
				}
				if (Input.GetKey(KeyCode.S))
				{
					motor.Move(-data.reverseSpeed);
				}
				if (Input.GetKey (KeyCode.D))
				{
					motor.Rotate(data.rotateSpeed);
				}
				if (Input.GetKey(KeyCode.Space))
				{
					if (gun.isLoaded)
					{
						gun.Fire();
					}
				}
			}
			break; 
			case InputScheme.ArrowKeys:
			{
				if (Input.GetKey(KeyCode.UpArrow))
				{
					motor.Move(data.moveSpeed);
				}
				if (Input.GetKey (KeyCode.LeftArrow))
				{
					motor.Rotate(-data.rotateSpeed);
				}
				if (Input.GetKey(KeyCode.DownArrow))
				{
					motor.Move(-data.reverseSpeed);
				}
				if (Input.GetKey(KeyCode.RightArrow))
				{
					motor.Rotate(data.rotateSpeed);
				}
				if (Input.GetKey (KeyCode.RightControl))
				{
					if (gun.isLoaded)
					{
						gun.Fire();
					}
				}
			}
			break; 
		}
	}
}
