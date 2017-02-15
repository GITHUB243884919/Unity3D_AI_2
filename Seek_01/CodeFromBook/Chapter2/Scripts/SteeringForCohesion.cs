using UnityEngine;
using System.Collections;

public class SteeringForCohesion : Steering {

	private Vector3 desiredVelocity;
	private Vehicle m_vehicle;
	private float maxSpeed;


	void Start () {
		m_vehicle = GetComponent<Vehicle>();
		maxSpeed = m_vehicle.maxSpeed;
	}


	public override Vector3 Force()
	{
		Vector3 steeringForce = new Vector3(0,0,0);
		Vector3 centerOfMass = new Vector3(0,0,0);
		int neighborCount = 0;

		foreach (GameObject s in GetComponent<Radar>().neighbors)
		{
			if ((s!=null)&&(s != this.gameObject))
			{
				centerOfMass += s.transform.position;
				neighborCount++;
			}
		}
		
		if (neighborCount > 0)
		{
			centerOfMass /= (float)neighborCount;
			desiredVelocity = (centerOfMass - transform.position).normalized * maxSpeed;
			steeringForce = desiredVelocity - m_vehicle.velocity;

		}
		
		return steeringForce;
	}
}
