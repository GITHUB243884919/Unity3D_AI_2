using UnityEngine;
using System.Collections;

public class EvadeController : MonoBehaviour 
{
	public GameObject leader;
	private Vehicle leaderLocomotion;
	private Vehicle m_vehicle;
	private bool isPlanar;
	private Vector3 leaderAhead;
	private float LEADER_BEHIND_DIST;
	private Vector3 dist;
	public float evadeDistance;
	private float sqrEvadeDistance;
	private SteeringForEvade evadeScript;

	// Use this for initialization
	void Start () 
	{
		leaderLocomotion = leader.GetComponent<Vehicle>();
		evadeScript = GetComponent<SteeringForEvade>();
		m_vehicle = GetComponent<Vehicle>();
		isPlanar = m_vehicle.isPlanar;
		LEADER_BEHIND_DIST = 2.0f;
		sqrEvadeDistance = evadeDistance * evadeDistance;
	}


	void Update () 
	{
		leaderAhead = leader.transform.position + leaderLocomotion.velocity.normalized * LEADER_BEHIND_DIST;

		dist = transform.position - leaderAhead;
		if (isPlanar)
			dist.y = 0;

		if (dist.sqrMagnitude < sqrEvadeDistance)
		{
			evadeScript.enabled = true;
			//print("isEvadingLeader!" + Time.time);
			Debug.DrawLine(transform.position, leader.transform.position);

		}
		else
		{
			evadeScript.enabled = false;
		}
	
	}
}
