using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameOverScreen : MonoBehaviour {

	public static Text scoreText; 
	public static Text highScoreText; 

	// Use this for initialization
	void Awake () {
		scoreText = GameObject.Find ("ScoreText").GetComponent<Text> (); 
		highScoreText = GameObject.Find ("HighScoreText").GetComponent<Text> (); 
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
