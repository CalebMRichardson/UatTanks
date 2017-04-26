using UnityEngine;
using UnityEngine.UI; 
using System.Collections;

public class PlayerUI : MonoBehaviour {

	public PlayerStats playerStats; 
	public TankData data; 

	public Text scoreText; 
	public Text livesText; 

	private int score; 

	void Start()
	{
		if (data == null)
		{
			data = GetComponent<TankData>(); 
		}
	}

	void Update()
	{
		scoreText.text = "Score: " + data.points;
		livesText.text = "Lives: " + data.lives; 
	}
}
