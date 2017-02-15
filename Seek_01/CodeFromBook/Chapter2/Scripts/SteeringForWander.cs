using UnityEngine;
using System.Collections;

//pay attention that this function is quite related with frame rate

public class SteeringForWander : Steering {

	public float wanderRadius;
	public float wanderDistance;
	public float wanderJitter;
	public bool isPlanar;
	//public GameObject targetIndicator;

	private Vector3 desiredVelocity;
	private Vehicle m_vehicle;
	private float maxSpeed;
	private Vector3 circleTarget;
	private Vector3 wanderTarget;


	void Start () {
		m_vehicle = GetComponent<Vehicle>();
		maxSpeed = m_vehicle.maxSpeed;
		isPlanar = m_vehicle.isPlanar;

		circleTarget = new Vector3(wanderRadius*0.707f, 0, wanderRadius * 0.707f);
	}
	

	public override Vector3 Force()
	{
		Vector3 randomDisplacement = new Vector3((Random.value-0.5f)*2*wanderJitter, (Random.value-0.5f)*2*wanderJitter,(Random.value-0.5f)*2*wanderJitter);

		if (isPlanar)
			randomDisplacement.y = 0;

		circleTarget += randomDisplacement;
		circleTarget = wanderRadius * circleTarget.normalized;

		wanderTarget = m_vehicle.velocity.normalized * wanderDistance + circleTarget + transform.position;

		desiredVelocity = (wanderTarget - transform.position).normalized * maxSpeed;
		return (desiredVelocity - m_vehicle.velocity);
	}
}
