using UnityEngine;
using System.Collections;
//using System.Collections.Generic;

public class Vehicle : MonoBehaviour {

	private Steering[] steerings;
	public float maxSpeed = 10;
	public float maxForce = 100;
	protected float sqrMaxSpeed;
	public float mass = 1;
	public Vector3 velocity;
	public float damping = 0.9f;
	public float computeInterval = 0.2f;
	public bool isPlanar = true;

	private Vector3 steeringForce;
	protected Vector3 acceleration;
	//private CharacterController controller;
	//private Rigidbody theRigidbody;
	//private Vector3 moveDistance;
	private float timer;


	protected void Start () 
	{
		steeringForce = new Vector3(0,0,0);
		sqrMaxSpeed = maxSpeed * maxSpeed;
		//moveDistance = new Vector3(0,0,0);
		timer = 0;

		steerings = GetComponents<Steering>();

		//controller = GetComponent<CharacterController>();
		//theRigidbody = GetComponent<Rigidbody>();
	}


	void Update () 
	{
		timer += Time.deltaTime;
		steeringForce = new Vector3(0,0,0);  

		//ticked part, we will not compute force every frame
		if (timer > computeInterval)
		{
			foreach (Steering s in steerings)
			{
				if (s.enabled)
					steeringForce += s.Force()*s.weight;
			}

			steeringForce = Vector3.ClampMagnitude(steeringForce,maxForce);
			acceleration = steeringForce / mass;

			timer = 0;
		}

	}

	/*
	void FixedUpdate()
	{
		velocity += acceleration * Time.fixedDeltaTime; 
		
		if (velocity.sqrMagnitude > sqrMaxSpeed)
			velocity = velocity.normalized * maxSpeed;
		
		moveDistance = velocity * Time.fixedDeltaTime;
		
		if (isPlanar)
			moveDistance.y = 0;
		
		if (controller != null)
			controller.SimpleMove(velocity);
		else if (theRigidbody == null || theRigidbody.isKinematic)
			transform.position += moveDistance;
		else
			theRigidbody.MovePosition(theRigidbody.position + moveDistance);		
		
		//updata facing direction
		if (velocity.sqrMagnitude > 0.00001)
		{
			Vector3 newForward = Vector3.Slerp(transform.forward, velocity, damping * Time.deltaTime);
			newForward.y = 0;
			transform.forward = newForward;
		}
	}*/
}
