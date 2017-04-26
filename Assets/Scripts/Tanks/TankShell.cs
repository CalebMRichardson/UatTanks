using UnityEngine;
using System.Collections;

public class TankShell : MonoBehaviour {
	
	public int shellDamage;  

	//Functionality 
	public float timeDestroy;

    private TankData data; 

	public AudioClip tankShellCollision; 

	// Use this for initialization
	void Start () {
		Destroy(gameObject, timeDestroy); 
	}

    public void SetTankData(TankData data)
    {
        this.data = data; 
    }

	//On Collision with object
	void OnCollisionEnter(Collision hitInfo)
	{
		//Get the TankMotor of the hit Object
		TankMotor hitTank = hitInfo.collider.gameObject.GetComponent<TankMotor> ();

		if (hitTank == null)
		{
			//Do Nothing because the object hit is not a tank
		}
		else
		{
            data.points += hitTank.pointsPerHit;
			hitTank.TakeDamage(shellDamage); 
		}

		AudioSource.PlayClipAtPoint(tankShellCollision, this.transform.position, PlayerPrefs.GetFloat("FXVol")); 

		Destroy (gameObject); 
	} 
}