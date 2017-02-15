using UnityEngine;
using System.Collections;

public class SteeringForArrive : Steering 
{
	//public enum Deceleration{slow = 3, normal = 2, fast = 1};
	//public Deceleration deceleration;
	public bool isPlanar = true;
	//public float DecelerationTweaker = 0.8f;
	public float arrivalDistance = 0.3f;
	public float characterRadius = 1.2f;

	public float slowDownDistance;

	public GameObject target;
	private Vector3 desiredVelocity;
	private Vehicle m_vehicle;
	private float maxSpeed;


	void Start () {
		m_vehicle = GetComponent<Vehicle>();
		maxSpeed = m_vehicle.maxSpeed;
		isPlanar = m_vehicle.isPlanar;
	}
	

	public override Vector3 Force()
	{
		Vector3 toTarget = target.transform.position - transform.position;
		Vector3 desiredVelocity;
		Vector3 returnForce;
		if (isPlanar) toTarget.y = 0;
		float distance = toTarget.magnitude;

		if (distance > slowDownDistance)
		{
			desiredVelocity = toTarget.normalized * maxSpeed;
			returnForce = desiredVelocity - m_vehicle.velocity;
		}
		else
		{
			desiredVelocity = toTarget - m_vehicle.velocity;
			returnForce = desiredVelocity - m_vehicle.velocity;
		}

		return returnForce;
	}

	void OnDrawGizmos()
	{
		Gizmos.DrawWireSphere(target.transform.position, slowDownDistance);
	}
}
