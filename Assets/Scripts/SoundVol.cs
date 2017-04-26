using UnityEngine;
using System.Collections;

public class SoundVol : MonoBehaviour {

	public AudioSource source; 
	
	// Update is called once per frame
	void Update () {
		source.volume = PlayerPrefs.GetFloat ("MusicVol"); 
	}
}
