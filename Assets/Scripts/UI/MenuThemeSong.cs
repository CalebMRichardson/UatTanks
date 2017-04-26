using UnityEngine;
using System.Collections;

public class MenuThemeSong : MonoBehaviour {

	private AudioSource source; 
	private int mainLevel; 

	//On Awake
	void Awake()
	{
		//Don't destroy this between menu screens
		DontDestroyOnLoad (gameObject); 

		if (source == null) {
			source = GetComponent<AudioSource> (); 
		}

	}
	//On Start initialize variables
	void Start()
	{
		mainLevel = 2; 
	}
	//Called once per frame
	void Update()
	{
		//set the volume of our music to equal the MusicVol
		source.volume = PlayerPrefs.GetFloat ("MusicVol"); 
	}

	//Check when the a new level was loaded
	void OnLevelWasLoaded(int level)
	{
		//if the level is mainLevel
		if (level == mainLevel)
		{
			//Destroy this because we are going to have a new song
			Destroy(this.gameObject);  
		}
	}

	//
	//MeunThemeSong (War Thunder - Ground Forces Battle Music 8) was downloaded from GameThemeSongs.com
	//

}
