using UnityEngine;
using System.Collections;

public class SteeringForEvade : Steering {

	public GameObject target;
	private Vector3 desiredVelocity;
	private Vehicle m_vehicle;
	private float maxSpeed;


	void Start () {
		m_vehicle = GetComponent<Vehicle>();
		maxSpeed = m_vehicle.maxSpeed;
	}
	

	public override Vector3 Force()
	{
		Vector3 toTarget = target.transform.position - transform.position;

		float lookaheadTime = toTarget.magnitude / (maxSpeed + target.GetComponent<Vehicle>().velocity.magnitude);

		desiredVelocity = (transform.position - (target.transform.position + target.GetComponent<Vehicle>().velocity * lookaheadTime)).normalized * maxSpeed;

		return (desiredVelocity - GetComponent<Vehicle>().velocity);
	}
}
