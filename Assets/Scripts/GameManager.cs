using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public static GameManager instance; 
	//Functionallity Vairables/GameObject
	public GameObject playerOne; 
	public GameObject playerTwo; 
	//tanks scripts
	public TankData playerOneData; 
	public TankData playerTwoData; 
	//respawn timer and maxPowerups
	public float respawnTimer; 
	public int maxPowerups; 
	//index at which the powerup will spawn and the max on the index of powerup spawn locations
	private int powerUpSpawnIndex; 
	private int maxPowerUpSpawnIndex; 

	public GameObject[] enemy;
	public GameObject[] spawns; 

	public SpawnPowerups powerups;  

    public static bool isTwoPlayer;

	private bool playerOneGameOver; 
	private bool playerTwoGameOver; 

	void Awake()
	{
		//if GameManager instance == null create it
		if (instance == null) {
			instance = this; 
		} 
		//If GameManager already exists throw error and destory it
		else {
			Debug.LogError ("There can be only Game Manager, Only one and he does not share POWER!");
			Destroy (gameObject); 
		}
	}

	void Start()
	{
		spawns = GameObject.FindGameObjectsWithTag ("Spawn"); 
		
		if (isTwoPlayer)
		{
			SpawnPrefab(playerOne, 0, "Player1"); 
			SpawnPrefab(playerTwo, 1, "Player2"); 
		}
		else 
		{
			SpawnPrefab(playerOne, 0, "Player1"); 
		}

		//Spawn enemies
		for (int i = 0; i < enemy.Length; i++)
		{
			//spawn at index spawn.length - 1 through spanw.length - 1 - 4
			SpawnPrefab(enemy[i], (spawns.Length - 1) - i, "Enemy"); 
		}

		//Get the middle index of the map - the number of power ups to choose from - 1; 
		powerUpSpawnIndex = spawns.Length / 2 - 1 - powerups.powerups.Length; 
		maxPowerUpSpawnIndex = spawns.Length / 2 - 1; 

		//set playerOne and playerOne data
		playerOne = GameObject.Find ("Player1"); 
		playerOneData = playerOne.GetComponent<TankData> (); 

		//if we are in two player mode set playerTwo and playerTwoData
		if (isTwoPlayer)
		{
			playerTwo = GameObject.Find ("Player2"); 
			playerTwoData = playerTwo.GetComponent<TankData>(); 
		}

		//set gameover conditions to false; 
		playerOneGameOver = false; 
		playerTwoGameOver = false; 

		HandleCameras (); 
	}

	void HandleCameras()
	{
		//Due to weird spawning happening Players One and Two will occupy spawn points 0 and 1
		//The Four Enemies will spawn on  the other side of the map at points spawn.length through spawn.length - 4
		//Power ups will spawn in the middleish of the map and accommodate the procedurally generated maps as best they can
		
		if (isTwoPlayer)
		{
			Camera p1 = playerOne.transform.FindChild("Main Camera").GetComponent<Camera>(); 
			p1.rect = new Rect(0, 0, 1, .5f); 
		}
		else 
		{
			//If Game Is played in two player mode and then one player mode it will continue only rendering half of the camera for some reason...
			//Also it is crucially important that it calls the camera.rect new Rect() before spawning in the player...for some reason... 
			//Most likely won't be an issue when the game is played in and navigated from the menu screens but out of parinoia I will keep it for now
			
			Camera p1 = playerOne.transform.FindChild("Main Camera").GetComponent<Camera>(); 
			p1.rect = new Rect(0, 0, 1, 1); 
		}
	}

	void Update()
	{
		//if the number of power ups on the field < maxPowerups spawn one back in
		if (powerups.NumberOnField() < maxPowerups)
		{
			powerups.IncrementNumInGame(); 
			StartCoroutine(WaitAndSpawn(respawnTimer)); 
		}

		//find index to spawn power ups
		if (powerUpSpawnIndex >= maxPowerUpSpawnIndex)
		{
			powerUpSpawnIndex = spawns.Length / 2 - 1 - powerups.powerups.Length;
		} 

		//if playerOne is active and has lives
		if (playerOne.activeSelf == false && playerOneData.lives > 0)
		{
			//set his health to 100 and spawn him back in
			playerOneData.health = 100; 
			StartCoroutine(WaitAndRespawn(respawnTimer, playerOne, 0)); 
		}

		//if we are in two player mode check if player two is active
		if (isTwoPlayer)
		{
			//if not but he has lives still 
			if (playerTwo.activeSelf == false && playerTwoData.lives > 0)
			{
				//set his health back to 100 and spawn him back in
				playerTwoData.health = 100; 
				StartCoroutine(WaitAndRespawn(respawnTimer, playerTwo, 1)); 
			}
		}

		//Check for a game over condition
		if (GameOverCondition() == true)
		{
			Application.LoadLevel ("GameOver"); 
		}
	}

	private bool GameOverCondition()
	{
		//if playerOne.lives < 0 playerOne is gameover
		if (playerOneData.lives <= 0)
		{
			playerOneGameOver = true; 
		}
		//if we are in two player mode also check for playerTwo 
		if (isTwoPlayer)
		{
			//if playerTwo lives < 0 player two is gameover
			if (playerTwoData.lives <= 0)
			{
				playerTwoGameOver = true; 
			}
		}
		//if we are in two player and both player one and player two are in game over return true
		if (isTwoPlayer)
		{
			if(playerOneGameOver == true && playerTwoGameOver == true)
			{
				return true; 
			}
		}
		//else if we are in one player mode and playerOne is in gameover return true; 
		else if (!isTwoPlayer)
		{
			if (playerOneGameOver == true)
			{
				return true; 
			}
		}

		return false; 
	}

	IEnumerator WaitAndSpawn(float time)
	{
		yield return new WaitForSeconds (time);
		SpawnPrefab (powerups.GetPowerUpPrefab (), powerUpSpawnIndex, "Power Up"); 
		powerUpSpawnIndex += 1; 
	}

	void SpawnPrefab(GameObject obj, int index, string name)
	{
		obj = Instantiate (obj, spawns [index].transform.position, Quaternion.identity) as GameObject; 
		obj.transform.parent = this.transform; 
		obj.name = name; 
	}

	IEnumerator WaitAndRespawn(float time, GameObject obj, int index)
	{
		yield return new WaitForSeconds (time);
		obj.transform.position = spawns [index].transform.position; 
		obj.SetActive (true); 
	}
}
