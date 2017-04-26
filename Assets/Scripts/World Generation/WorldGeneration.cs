using UnityEngine;
using System.Collections;
using System; 
[RequireComponent (typeof(Rooms))]

public class WorldGeneration : MonoBehaviour {

	//Functionality Variables
	private int rows = 0; 
	private int cols = 0; 
	private const int minRows = 3; 
	private const int minCols = 3; 
	private int roomWidth = 60; 
	private int roomLength = 60;  
	public GameObject[] roomPrefab; 
	private Rooms[,] room; 
	public int seed; 
	public bool createRandomMap; 

	//Awake called at begining of Script
	void Awake()
	{
		//Decide which Seed to use UnityEngine.Random.seed
		if (PlayerPrefs.GetInt ("UseMapOfTheDay") == 1) 
		{
			UnityEngine.Random.seed = DateToInt(DateTime.Now.Date); 
		}
		else if (createRandomMap)
		{
			UnityEngine.Random.seed = DateToInt(DateTime.Now); 
		}
		//Convert seed to a String
		string seedString = Convert.ToString (UnityEngine.Random.value); 
		//Remove "."
		seedString = seedString.Replace (".", string.Empty); 
		//Remove "0"
		seedString = seedString.Replace ("0", string.Empty); 
		//Set Rows and Cols = to NumericValue of string at index[0] and index[1]
		rows = (int)Char.GetNumericValue (seedString [0]); 
		cols = (int)Char.GetNumericValue (seedString [1]); 

		if (rows < minRows)
		{
			rows = rows + 3; 
		}
		if (cols < minCols)
		{
			cols = cols + 3; 
		}

		GenerateMap ();  
	}

	//Return Date as an int
	private int DateToInt(DateTime date)
	{
		return date.Year + date.Month + date.Day + date.Hour + date.Minute + date.Second + date.Millisecond; 
	}

	//Generates a map
	private void GenerateMap()
	{
		//For i in rows
		for (int i = 0; i < rows; i++)
		{
			//for i in cols
			for (int j = 0; j < cols; j++)
			{
				//Get new position
				float xPos = roomWidth * j; 
				float zPos = roomLength * i; 

				Vector3 newPosition = new Vector3(xPos, 0, zPos); 

				//Create GameObject
				GameObject newRoom = Instantiate(GetRandomPrefab(), newPosition, Quaternion.identity) as GameObject; 

				//Set Transform parent
				newRoom.transform.parent = this.transform; 
				//Set transform name
				newRoom.name = "ROOM_" + j + "_" + i; 

				Rooms tempRoom = newRoom.GetComponent<Rooms>(); 

				//
				//Open doors North and South Remove walls
				//

				//Bottom Row
				if (i == 0)
				{
					tempRoom.northDoor.SetActive(false); 
					tempRoom.northWalls[0].SetActive(false); 
					tempRoom.northWalls[1].SetActive(false); 
				}
				//Top Row
				else if (i == rows - 1)
				{
					tempRoom.southDoor.SetActive(false); 
				}
				//Everything in between :)
				else
				{
					tempRoom.southDoor.SetActive(false);
					tempRoom.northDoor.SetActive(false); 
					tempRoom.northWalls[0].SetActive(false); 
					tempRoom.northWalls[1].SetActive(false); 
				}

				//
				//Open Doors East and West Remove Walls
				//

				//Left most col
				if (j == 0)
				{
					tempRoom.eastDoor.SetActive(false);
					tempRoom.eastWalls[0].SetActive(false); 
					tempRoom.eastWalls[1].SetActive(false); 
				}
				else if (j == cols - 1)
				{
					tempRoom.westDoor.SetActive(false); 
				}
				else 
				{
					tempRoom.eastDoor.SetActive(false); 
					tempRoom.westDoor.SetActive(false); 
					tempRoom.eastWalls[0].SetActive(false); 
					tempRoom.eastWalls[1].SetActive(false); 
				}
			}
		}
	}
	//Retrun Random Prefab from roomPrefabs
	private GameObject GetRandomPrefab()
	{
		return roomPrefab[UnityEngine.Random.Range(0, roomPrefab.Length)]; 
	}
}