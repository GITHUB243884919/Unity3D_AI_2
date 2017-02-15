using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;

public enum MovementState:int
{
	IDLE,
	MOVING,
	ORGANIZING
}

public class Boid : MonoBehaviour {

	public float movementSpeed = 1;
	public GameObject target;

	private MovementState currentMovementState;
	public MovementState CurrentMovementState{
		get{
			return currentMovementState;
		}
		set{
			currentMovementState = value;
		}
	}

	public float coherencyWeight = 0;
	public float separationWeight = 1;

	public float turnSpeed = 4.0f;

	private Vector3 relativePos;
	private Vector3 coherency;
	private Vector3 separation;
	private Vector3 boidBehaviorForce;
	List<Boid> boids;

	private CharacterController controller;

    //Radar
	public float radius = 1;
	public int pingsPerSecond = 10;
	
	public float PingFrequency{
		get{
			return (1/pingsPerSecond);
		}
	}
	
	public LayerMask radarLayers;
	
	private List<Boid> neighbors = new List<Boid>();
	private Collider[] detected;


	//Pathfinding
	private Seeker seeker;
	private Path path;
	private int currentWayPoint = 0;
	private float nextWayPoint = 1;
	private bool journeyComplete = true;
	private Vector3 pathDirection;

	//applying forces
	private Vector3 center;
	private Vector3 steerForce;
	private Vector3 seekForce;
	private Vector3 driveForce = Vector3.zero;

	//private Vector3 newForward;

	public Vector3 FollowPath()
	{
		if (path == null || currentWayPoint >= path.vectorPath.Count || target == null)
			return Vector3.zero;

		if (currentWayPoint >= path.vectorPath.Count || Vector3.Distance(transform.position, target.transform.position)<0.2)
		{
			journeyComplete = true;
			return Vector3.zero;
		}

		pathDirection = (path.vectorPath[currentWayPoint] - transform.position).normalized;
		pathDirection *= movementSpeed * Time.deltaTime;

		if (Vector3.Distance(transform.position, path.vectorPath[currentWayPoint])<nextWayPoint)
		{
			currentWayPoint++;
		}

		return pathDirection;
	}

	public void OnPathComplete(Path p)
	{
		if (p.error)
		{
			Debug.Log ("Can't find path!");
			return;
		}

		path = p;
		
		currentWayPoint = 0;
		CurrentMovementState = MovementState.MOVING;
	}

	public void CalculatePath()
	{
		if (target == null)
		{
			Debug.LogWarning("Target is null.Aborting Pathfinder...");
			return;
		}

		seeker.StartPath(transform.position, target.transform.position, OnPathComplete);
		journeyComplete = false;

	}

	void Start () {
		controller = GetComponent<CharacterController>();
		seeker = GetComponent<Seeker>();

		Boid[] foundBoids = FindObjectsOfType(typeof(Boid)) as Boid[];

		boids = new List<Boid>();

		foreach (Boid b in foundBoids)
		{
			boids.Add(b);
		}

		CurrentMovementState = MovementState.IDLE;

		StartCoroutine("StartTick", PingFrequency);
	
	}

	public Vector3 BoidBehaviors()
	{
		Vector3 boidBehaviorForce;

		coherency = center - transform.position;
		separation = Vector3.zero;

		foreach (Boid b in neighbors)
		{
			if (b!=this)
			{
				relativePos = (transform.position - b.transform.position);
				separation += relativePos / relativePos.sqrMagnitude;
			}
		}

		boidBehaviorForce = (coherency * coherencyWeight) + (separation * separationWeight);

		boidBehaviorForce.y = 0;

		return boidBehaviorForce;
	}

	void Update () {
	
		center = Vector3.zero;

		if (boids.Count > 0)
		{
			foreach(Boid b in boids)
			{
				center += b.transform.position;
			}

			center = center/boids.Count;
		}

		steerForce = Vector3.zero;
		seekForce = Vector3.zero;

		if (!journeyComplete && currentMovementState == MovementState.MOVING)
		{
			seekForce = FollowPath();
		}
		else
		{
			if (CurrentMovementState != MovementState.ORGANIZING && CurrentMovementState != MovementState.IDLE)
				CurrentMovementState = MovementState.ORGANIZING;
		}

		if (currentMovementState == MovementState.ORGANIZING)// || EveryoneStopped() == false)
		{
			steerForce = BoidBehaviors();
		}

		driveForce = steerForce + seekForce;

		controller.Move(driveForce);

		if (!journeyComplete && currentMovementState == MovementState.MOVING)
			TurnToFaceMovementDirection(driveForce);
	}

	private void TurnToFaceMovementDirection(Vector3 newVel)
	{
		if (movementSpeed > 0 && newVel.sqrMagnitude > 0.00005f);
		{
			float step = turnSpeed * Time.deltaTime;
			Vector3 newDir = Vector3.RotateTowards(transform.forward, newVel.normalized, step, 0.0F);
			transform.rotation = Quaternion.LookRotation(newDir);
		}
	}

	private IEnumerator StartTick(float freq)
	{
		yield return new WaitForSeconds(freq);
		
		RadarScan();
	}
	
	private void RadarScan()
	{
		neighbors.Clear();

		detected = Physics.OverlapSphere(transform.position, radius,radarLayers);

		foreach(Collider c in detected)
		{
			if (c.GetComponent<Boid>() !=null && c.gameObject != this.gameObject)
			{
				Boid foundBoid = c.GetComponent<Boid>() as Boid;
				neighbors.Add(foundBoid);
			}
		}

		if (neighbors.Count == 0 && currentMovementState != MovementState.MOVING && currentMovementState == MovementState.ORGANIZING)
		{
			Debug.LogWarning(currentMovementState);
			CurrentMovementState = MovementState.IDLE;
		}

		StartCoroutine("StartTick", PingFrequency);
	}

}