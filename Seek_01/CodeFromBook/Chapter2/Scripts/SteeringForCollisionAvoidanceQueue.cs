using UnityEngine;
using System.Collections;

public class SteeringForCollisionAvoidanceQueue : Steering 
{
	public bool isPlanar;	
	private Vector3 desiredVelocity;
	private Vehicle m_vehicle;
	private float maxSpeed;
	private float maxForce;
	public float avoidanceForce;
	public float MAX_SEE_AHEAD;
	private GameObject[] allColliders;

	private int layerid;
	private LayerMask layerMask;

	void Start () 
	{
		m_vehicle = GetComponent<Vehicle>();
		maxSpeed = m_vehicle.maxSpeed;
		maxForce = m_vehicle.maxForce;
		isPlanar = m_vehicle.isPlanar;
		//avoidanceForce = 20.0f;
		if (avoidanceForce > maxForce)
			avoidanceForce = maxForce;

		//MAX_SEE_AHEAD = 20.0f;
		allColliders = GameObject.FindGameObjectsWithTag("obstacle");

		layerid = LayerMask.NameToLayer("obstacle");
		layerMask = 1 << layerid;
	}

	public override Vector3 Force()
	{
		RaycastHit hit;
		Vector3 force = new Vector3(0,0,0);

		Vector3 velocity = m_vehicle.velocity;
		Vector3 normalizedVelocity = velocity.normalized;
		//Debug.DrawLine(transform.position, transform.position + normalizedVelocity * MAX_SEE_AHEAD);

		if (Physics.Raycast(transform.position, normalizedVelocity, out hit, MAX_SEE_AHEAD,layerMask))
		{
			Vector3 ahead = transform.position + normalizedVelocity * MAX_SEE_AHEAD;
			force = ahead - hit.collider.transform.position;
			force *= avoidanceForce; 

			//force = (hit.collider.transform.right + new Vector3(Random.Range(0.1f,0.3f),0,Random.Range(0.1f,0.3f))) * avoidanceForce;
			
			if (isPlanar)
				force.y = 0;

			/*
			foreach (GameObject c in allColliders)
			{
				if (hit.collider.gameObject == c)
				{
					c.renderer.material.color = Color.green;
				}
				else
					c.renderer.material.color = Color.gray;
			}*/
		}
		/*
		else
		{
			foreach (GameObject c in allColliders)
			{
				c.renderer.material.color = Color.gray;
			}
		}*/


		/*
		if (Physics.Raycast(transform.position, transform.forward, out hit, maxSpeed))
		{
			if (hit.collider.tag != "ground")
			{	
				Vector3 ahead = transform.position + transform.forward * maxSpeed;
				force = ahead - hit.collider.transform.position;
				force *= maxForce;

				if (isPlanar)
					force.y = 0;
			
				Debug.DrawLine(transform.position, transform.position + force);			
			}
		}*/

		return force;
	}
}
