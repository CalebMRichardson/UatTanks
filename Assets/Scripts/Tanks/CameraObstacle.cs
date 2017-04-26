using UnityEngine;
using System.Collections;

public class CameraObstacle : MonoBehaviour {

	public Transform player; 
	private RaycastHit hit;
	private Color originalColor; 
	private GameObject wall; 
	private bool wallObjectFilled; 

	void Start()
	{
		wallObjectFilled = false; 
	}

	void Update()
	{
		//DrawRay from Cameara to Player for debugging purposes
		//Debug.DrawRay (this.transform.position, (player.position - this.transform.position), Color.green); 
		//Pysics.Raycast from Camera to Player
		if (Physics.Raycast(this.transform.position, (player.position - this.transform.position), out hit))
		{
			//if hit.tranform.tag != "Wall" and wallObjectFilled == false return out of function. (This is used to avoid the NullRefrenceException Error)
			if (hit.transform.tag != "Wall" && wallObjectFilled == false)
			{
				return; 
			}
			//if Physics.Raycast hit == "Wall"
			if (hit.transform.tag == "Wall")
			{
				//Set GameObject wall to the specific wall that has been hit
				wall = hit.transform.gameObject; 
				wallObjectFilled = true; 
				//Get MeshRenderer Componenet for manipulation
				MeshRenderer renderer = hit.transform.gameObject.GetComponent<MeshRenderer>() as MeshRenderer;
				//Set the original Color for resetting
				originalColor = renderer.material.color;
				//Get the Color
				Color c = renderer.material.color; 
				//Set Color alpha to 20% of original 
				c.a = .3f;
				//Set the renderer.material.color to c
				renderer.material.color = c; 
			}
			//if hit != wall
			if (hit.transform.gameObject != wall)
			{
				//Get MeshRender Component
				MeshRenderer renderer = wall.transform.gameObject.GetComponent<MeshRenderer>() as MeshRenderer; 
				//Set Render.material.color to originalColor
				renderer.material.color = originalColor; 
				//Set OriginalColor alpha value to 100%
				originalColor.a = 1.0f; 
				//Set material color back to originalColor
				renderer.material.color = originalColor; 
			}
		}
	}
}