using UnityEngine;
using System.Collections;
[RequireComponent (typeof(TankData))]
[RequireComponent (typeof(TankMotor))]
[RequireComponent (typeof(TankGun))]

public class AIController : MonoBehaviour {
	//Script Objects
	private TankData data; 
	private TankMotor motor; 
	private TankGun gun;  

	public GameObject[] waypoints; 

	private Transform tf; 

	//Variables to handle Enemy personality and state
	public enum EnemyPersonallity { TheCreep, TheStickInTheMud, TheStickler, TheLooseCannon }; 
	public EnemyPersonallity personality; 
	public enum AttackMode { Fight, Flight, Neutral };
	public AttackMode attackMode; 

	//Functionality Variables
	private int currentWaypoint;
	public float closeEnough = 1.0f; 
	private int avoidanceStage; 
	public float avoidanceTime = 2.0f; 
	private float exitTime; 
	public float hearDistance; 
	private SphereCollider sc; 
	private Transform playerTf; 
	public float fleeDistance; 
	private int looseCannonStartWayPoint; 
	void Start()
	{
		//
		//Get Scripts
		//

		if (data == null)
		{
			data = GetComponent<TankData>(); 
		}
		if (motor == null)
		{
			motor = GetComponent<TankMotor>(); 
		}
		if (gun == null)
		{
			gun = GetComponent<TankGun>(); 
		}

		//Add in spawn points
		waypoints = GameObject.FindGameObjectsWithTag ("Spawn"); 
		//Get Transform Component
		tf = this.gameObject.GetComponent<Transform> ();  
		//Get the currentWaypoint (Closest Waypoint); 
		currentWaypoint = GetClosestWayPoint (tf); 
		looseCannonStartWayPoint = currentWaypoint; 

		avoidanceStage = 0; 

		sc = GetComponent<SphereCollider> (); 
		sc.radius = hearDistance; 

		playerTf = GetComponent<Transform> (); 
		playerTf = GameObject.FindGameObjectWithTag ("Player").transform; 
	}

	//Return Closest Waypoint
	int GetClosestWayPoint(Transform tTransform)
	{
		int closestWaypoint = 0; 
		//Loop through all the waypoint
		for (int i = 0; i < waypoints.Length - 1; i++) 
		{
			//if waypoint[i] is closer then waypoint[i + 1] save i as temp int
			if (Vector3.Distance(tTransform.position, waypoints[i].transform.position) < Vector3.Distance(tTransform.position, waypoints[i+1].transform.position))
			{
				int temp = i; 
				//if the current waypoint[closestWaypoint] is further away from waypoint[temp] then save temp as the closest
				if (Vector3.Distance (tTransform.position, waypoints[closestWaypoint].transform.position) > Vector3.Distance(tTransform.position, waypoints[temp].transform.position))
				{
					closestWaypoint = temp; 
				}
			}
		}
		//return closestWaypoint
		return closestWaypoint;
	}

	//Called once per frame MAIN SCRIPT LOOP
	void Update()
	{
		//If player doesn't exist set tanks back to attackMode.Neutral
		if (playerTf == null) {
			attackMode = AttackMode.Neutral; 
		}

		//if data.heatlh < 49 enter flight attackmode
		if (data.health <= 49)
		{
			attackMode = AttackMode.Flight; 
		}

		//
		// Handle Player state
		//

		//Neutral
		if (attackMode == AttackMode.Neutral) {
			//if avoidace stage != 0 then we are in avoidance mode
			if (avoidanceStage != 0) 
			{
				DoAvoidance (); 
			} 
			else 
			{ //we are not in avoidance mode
				//If we can move forward by data.moveSpeed units
				if (CanMove (data.moveSpeed)) {
					if (motor.RotateTowards (waypoints [currentWaypoint].transform.position, data.rotateSpeed)) {
						//Do Nothing we are looking at the current waypoint
					} else {
						//Move Speed
						motor.Move (data.moveSpeed); 
					}
					//If the distance between us and the current waypoint < closeEnough
					if (Vector3.Distance (tf.position, waypoints [currentWaypoint].transform.position) < closeEnough) {
						//If TheStickler 
						if (personality == EnemyPersonallity.TheStickler)
						{
							//increment currentWaypoint or set it back to zero
							currentWaypoint++; 
							if (currentWaypoint == waypoints.Length) {
								currentWaypoint = 0; 
							}
						}
						else if (personality == EnemyPersonallity.TheCreep)
						{
							currentWaypoint = GetClosestWayPoint(playerTf); 
						}
						else if (personality == EnemyPersonallity.TheLooseCannon)
						{
							//GetRandomIndex
							currentWaypoint = Random.Range (0, waypoints.Length); 
						}
						else if (personality == EnemyPersonallity.TheStickInTheMud)
						{
							//if the currentWayPoint == looseCannonStartWayPoint
							if (looseCannonStartWayPoint == currentWaypoint)
							{
								currentWaypoint++; 
							}
							else //else set it back to startingWaypoint
							{
								currentWaypoint = looseCannonStartWayPoint; 
							}
						}
					}
				} else {
					avoidanceStage = 1; 
				}
			}

		} 
		//
		//Fight
		//
		else if (attackMode == AttackMode.Fight) 
		{ 
			//Look for player and see if he is in line of sight - if Player exists that is :)
			if (playerTf != null) 
			{
				if (avoidanceStage != 0)
				{
					DoAvoidance(); 
				}
				else
				{
					//Try and get the enemy tank between 30 and 60 units from player for optimal aim
					if (Vector3.Distance(tf.position, playerTf.position) > 60)
					{
						if (CanMove(data.moveSpeed))
						{
							motor.Move(data.moveSpeed); 
						}
						else
						{
							avoidanceStage = 1; 
						}
					}
					else if (Vector3.Distance(tf.position, playerTf.position) < 30)
					{
						if (CanMove(data.moveSpeed))
						{
							motor.Move(-data.reverseSpeed); 
						}
					}

					RaycastHit hit; 
					Debug.DrawRay (tf.position, playerTf.position - tf.position, Color.grey); 
					if (Physics.Raycast (tf.position, playerTf.position - tf.position, out hit)) 
					{
						if (hit.collider.gameObject.tag != "Player") 
						{
							//Do Nothing the player is not in sight
						} 
						else 
						{
							//Player is in sight
							motor.RotateTowards (playerTf.position, data.moveSpeed); 
							if (gun.isLoaded) 
							{
								gun.Fire (); 
							}
						}
					}
			     }
			}
		}
		//
		//Flight 
		//
		else if (attackMode == AttackMode.Flight)
		{
			if (avoidanceStage != 0)
			{
				DoAvoidance();
			}
			else
			{
				//The vector to target minus the tf.position
				Vector3 vectorToTarget = playerTf.position - tf.position; 

				//Flip Away From Target
				Vector3 vectorAwayFromTarget = -1 * vectorToTarget; 

				//Normalize Vector3
				vectorAwayFromTarget.Normalize(); 

				//Get the distance
				vectorAwayFromTarget *= fleeDistance; 

				Vector3 fleePosition = vectorAwayFromTarget + tf.position; 

				if (CanMove(data.moveSpeed))
				{
					if (motor.RotateTowards(fleePosition, data.rotateSpeed))
					{

					}
					else
					{
						motor.Move(data.moveSpeed); 
					}
				}
				else
				{
					avoidanceStage = 1; 
				}
			}
		}
	}

	void DoAvoidance()
	{
		RaycastHit leftHit;
		RaycastHit rightHit; 
		RaycastHit frontHit; 

		if (avoidanceStage == 1)
		{
			//Check for nearest Escape
			//if physics.raycast(left and right) we are really close to a wall or in a corner so turn left
			if (Physics.Raycast(tf.position + tf.forward * 5.7f, tf.forward, out frontHit, 15))
			{
				motor.Rotate(-1 * data.rotateSpeed); 
			}

			else if (Physics.Raycast(tf.position + tf.forward * 5.7f, tf.forward - tf.right, out leftHit, 15) && 
			    (Physics.Raycast(tf.position + tf.forward * 5.7f, tf.forward + tf.right, out rightHit, 15)))
			{
				if (leftHit.collider.gameObject.tag == "Wall" && rightHit.collider.gameObject.tag == "Wall")
				{
					motor.Rotate(-1 * data.rotateSpeed);
				}
			}
			//else if we are hitting left turn right
			else if (Physics.Raycast(tf.position + tf.forward * 5.7f, tf.forward - tf.right, out leftHit, 15))
			{
				motor.Rotate(data.rotateSpeed); 
			}
			//else if we are hitting right turn left
			else if (Physics.Raycast (tf.position + tf.forward * 5.7f, tf.forward + tf.right, out rightHit, 15))
			{
				motor.Rotate(-1 * data.rotateSpeed); 
			}
			//if we CanMove forward by moveSpeed units then advance avoidanceStage to two
			if (CanMove (data.moveSpeed))
			{
				avoidanceStage = 2; 
				//Set the number of seconds we will stay in stage 2
				exitTime = avoidanceTime; 
			}
		}
		//if avoidance stage == 2
		else if (avoidanceStage == 2)
		{
			//if we can move forward then do it
			if (CanMove(data.moveSpeed))
			{
				exitTime -= Time.deltaTime; 
				motor.Move (data.moveSpeed); 

				if (exitTime <= 0)
				{
					avoidanceStage = 0; 
				}
			}
			else
			{
				//if we can't move forward set avoidance stage back to one
				avoidanceStage = 1; 
			}
		}
	}

	//Check if we can move forward "speed" units
	bool CanMove(float speed)
	{
		//Cast a ray forward in the distance that we sent in
		//if our ray hits something
		RaycastHit hit; 
		{
			if (Physics.Raycast(tf.position, tf.forward, out hit, speed))
			{
				//if what we hit was a wall
				if (hit.collider.CompareTag("Wall"))
				{
					return false; 
				}
			}
		}
		return true; 
	}

	//On Trigger Enter
	void OnTriggerEnter(Collider collider)
	{
		//If collider is Player the enemy can "Hear" the player and will switch to attackMode.Fight
		if (collider.gameObject.tag == "Player" && data.health >= 50)
		{
			attackMode = AttackMode.Fight; 
		}
	}
	//On Trigger Exit
	void OnTriggerExit(Collider collider)
	{
		//if collider is player
		if (collider.gameObject.tag == "Player")
		{
			//if enemy is in fight mode then set its fight mode to Neutral
			if (attackMode == AttackMode.Fight)
			{
				attackMode = AttackMode.Neutral; 
			}
			//If attackMode is in flight set it to Neutral
			if (attackMode == AttackMode.Flight)
			{
				attackMode = AttackMode.Neutral; 
			}
		}
	}
}
