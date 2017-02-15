using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DestinationManager : MonoBehaviour {

	public GameObject destinationObjectToMove;
	public GameObject destinationPrefab;

	private static DestinationManager instance;
	public static DestinationManager Instance{ get{ return instance; } }

	public List<Destination> destinations;

	private List<Boid> boids = new List<Boid>();

	private bool destinationsAreDoneMoving = false;
	private bool destinationsAreAssigned = true;

	private Ray ray;
	private RaycastHit hitInfo;

	//private int maxBoidsCount = 7;
	public float destCircleRadius = 1;
	private bool generateDestination = true;

	private Vector3[] offset;

	void Awake()
	{
		instance = this;
		destinations = new List<Destination>();
		FindBoids();

		offset = new Vector3[13];

		offset[0] = new Vector3(0,0,0);

		offset[1] = new Vector3(1,0,0);
		offset[2] = new Vector3(0.5f,0,0.87f);
		offset[3] = new Vector3(-0.5f,0,0.87f);
		offset[4] = new Vector3(-1,0,0);
		offset[5] = new Vector3(-0.5f,0,-0.87f);
		offset[6] = new Vector3(0.5f,0,-0.87f);

		offset[7] = new Vector3(0.87f,0,0.5f);
		offset[8] = new Vector3(0,0,1);
		offset[9] = new Vector3(-0.87f,0,0.5f);
		offset[10] = new Vector3(-0.87f,0,-0.5f);
		offset[11] = new Vector3(0,0,-1);
		offset[12] = new Vector3(0.87f,0,-0.5f);
	}

	public void FindBoids()
	{
		boids.Clear();

		Boid[] foundBoids = FindObjectsOfType(typeof(Boid)) as Boid[];

		foreach(Boid c in foundBoids)
		{
			boids.Add(c);
		}
	}


	void Start () {
		/*
		placeoffset = new Vector3[maxBoidsCount];
		placeoffset[0] = new Vector3(0,0,0);
		placeoffset[1] = new Vector3(1,0,0);
		placeoffset[2] = new Vector3(0.5f,0,0.87f);
		placeoffset[3] = new Vector3(-0.5f,0,0.87f);
		placeoffset[4] = new Vector3(-1,0,0);
		placeoffset[5] = new Vector3(-0.5f,0,-0.87f);
		placeoffset[6] = new Vector3(0.5f,0,-0.87f);

		int index = 0;*/

	}

	void placeDestination(Vector3 hitPoint)
	{
		int index = 0;
		float radius = destCircleRadius;

		foreach(Boid c in boids)
		{
			if (generateDestination)
			{
				GameObject des = Instantiate(destinationPrefab,hitPoint + radius * offset[index++],Quaternion.identity) as GameObject;
				destinations.Add(des.GetComponent<Destination>());
				c.target = des;
			}
			else
			{
				c.target.transform.position = hitPoint + radius * offset[index++];
			}

			if (index>12)
			{
				index = 1;
				radius *= 4;
			}
		}

		destinationsAreAssigned = false;
		destinationsAreDoneMoving = false;
		generateDestination = false;

		return;
	}


	void Update () {

		if (Input.GetMouseButtonDown (0))
		{
			ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			if (Physics.Raycast(ray.origin, ray.direction, out hitInfo))
			{
				if (hitInfo.collider.gameObject.layer == 8)
				{
					placeDestination(hitInfo.point);
				}

				return;
			}
		}

		if (destinations.Count == 0)
			return;

		Vector3 center = Vector3.zero;
		Vector3 velocity = Vector3.zero;

		foreach(Destination d in destinations)
		{
			center += d.transform.position;
		}

		Vector3 destinationCenter = center/destinations.Count;

		if (destinationsAreDoneMoving && !destinationsAreAssigned)
		{
			AssignNodes();
			destinationsAreAssigned = true;
			return;
		}

		int destinationStopped = 0;

		foreach(Destination dests in destinations)
		{
			dests.CalculateForce(destinationCenter);
			velocity += dests.rigidbody.velocity;
			if (dests.Velocity < 1)
				destinationStopped++;
		}

		Vector3 destinationVelocity = velocity/destinations.Count;

		if (destinationStopped == destinations.Count)
		{
			foreach(Destination dst in destinations)
			{
				dst.rigidbody.velocity = Vector3.zero;
			}
			destinationsAreDoneMoving = true;
		}
	
	}

	private void AssignNodes()
	{
		for (int i=0; i<boids.Count; i++)
		{
			boids[i].CalculatePath();
		}
	}
}
