using UnityEngine;
using System.Collections;

public class SteeringForSeek : Steering {
	
	public GameObject target;
	private Vector3 desiredVelocity;
	private Vehicle m_vehicle;
	private float maxSpeed;
	private bool isPlanar;
	
	void Start () {
		m_vehicle = GetComponent<Vehicle>();
		maxSpeed = m_vehicle.maxSpeed;
		isPlanar = m_vehicle.isPlanar;
	}
	
	
	public override Vector3 Force()
	{
		desiredVelocity = (target.transform.position - transform.position).normalized * maxSpeed;
		if (isPlanar)
			desiredVelocity.y = 0;
		return (desiredVelocity - m_vehicle.velocity);
	}
}

