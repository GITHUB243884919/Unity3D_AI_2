using UnityEngine;
using System.Collections;

public class SteeringForQueue : Steering 
{
	public float MAX_QUEUE_AHEAD;
	public float MAX_QUEUE_RADIUS;
	private Collider[] colliders; 
	public LayerMask layersChecked;
	private Vehicle m_vehicle;
	private int layerid;
	private LayerMask layerMask;


	void Start () 
	{
		m_vehicle = GetComponent<Vehicle>();
		//MAX_QUEUE_AHEAD = 6;
		//MAX_QUEUE_RADIUS = 5;
		layerid = LayerMask.NameToLayer("vehicles");
		layerMask = 1 << layerid;

	}

		
	public override Vector3 Force()
	{
		//Vector3 ahead = transform.position + transform.forward * MAX_QUEUE_AHEAD;
		Vector3 velocity = m_vehicle.velocity;
		Vector3 normalizedVelocity = velocity.normalized;
		Vector3 ahead = transform.position + normalizedVelocity * MAX_QUEUE_AHEAD;

		colliders = Physics.OverlapSphere(ahead, MAX_QUEUE_RADIUS, layerMask);

		if (colliders.Length > 0)
		{
			foreach (Collider c in colliders)
			{
				if ((c.gameObject != this.gameObject) && (c.gameObject.GetComponent<Vehicle>().velocity.magnitude < velocity.magnitude))
				{
					m_vehicle.velocity *= 0.8f;
					break;
				}
			}
		}

		/*
		if (colliders.Length > 0)
		{
			//print ("collide!" + colliders.Length);
			m_vehicle.velocity *= 0.3f;
		}*/		

		return new Vector3(0,0,0);
	}
}
