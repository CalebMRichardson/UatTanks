using UnityEngine;
using UnityEngine.UI; 
using System.Collections;

public class ButtonEvents : MonoBehaviour {

    public AudioClip feedback; 

	public void SinglePlayerOnClick()
	{
        //Load main game single player
		GameManager.isTwoPlayer = false; 
		Application.LoadLevel ("main"); 
	}

	public void TwoPlayerOnClick()
	{
		GameManager.isTwoPlayer = true; 
		Application.LoadLevel ("main"); 
	}

	//On Options Button Click
	public void OptionsOnClick()
	{
        //Load Options
		Application.LoadLevel ("OptionsMenu"); 
	}

	//On Exit Button Click
	public void ExitOnClick()
	{
		//Close App
		Application.Quit ();  
	}

    public void MainMenuOnClick()
    {
        Application.LoadLevel("MainMenu"); 
    }

	//Save Settings
	public void ApplyOnClick()
	{
        //Get Slider position
		float musicVol = GameObject.Find ("MusicSlider").GetComponent<Slider> ().value; 
		float fxVol = GameObject.Find ("FXSlider").GetComponent<Slider> ().value;
        //Get Toggle for MapOfTheDay
        Toggle mapOfTheDayToggle = GameObject.Find("UseMapOfTheDay").GetComponent<Toggle>(); 

        //if mapOfTheDayToggle is on set MapOfTheDay to 1
        if (mapOfTheDayToggle.isOn)
        {
            PlayerPrefs.SetInt("MapOfTheDay", 1); 
        }
        //Else set mapOFTheDay to 0; 
        else
        {
            PlayerPrefs.SetInt("MapOfTheDay", 0); 
        }

        //Set float value
		PlayerPrefs.SetFloat ("MusicVol", musicVol); 
		PlayerPrefs.SetFloat ("FXVol", fxVol); 
        //Save float value
		PlayerPrefs.Save ();
	}

	//Go back to main menu
	public void BackOnClick()
	{
        //Go back to MainMenu
		Application.LoadLevel ("MainMenu"); 
	}

    //Play button feedback
    public void PlayFeedback()
    {
        float vol = PlayerPrefs.GetFloat("FXVol"); 
        AudioSource.PlayClipAtPoint(feedback, this.transform.position, vol); 
    }
}
