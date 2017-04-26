using UnityEngine;
using System.Collections;
[System.Serializable]


public class PlayerStats {
	public int playerScore = 0;
	public static int highScore = PlayerPrefs.GetInt("HighScore"); 
}

