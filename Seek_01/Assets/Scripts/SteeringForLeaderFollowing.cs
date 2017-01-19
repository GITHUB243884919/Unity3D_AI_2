using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SteeringForArrive))]

public class SteeringForLeaderFollowing : Steering 
{
	public Vector3 target;
	private Vector3 desiredVelocity;
	private Vehicle m_vehicle;
	private float maxSpeed;
	private bool isPlanar;
	public GameObject leader;
	private Vehicle leaderController;
	private Vector3 leaderVelocity;
	private float LEADER_BEHIND_DIST = 2.0f;
	private SteeringForArrive arriveScript;
	private Vector3 randomOffset;


	void Start () 
	{
		m_vehicle = GetComponent<Vehicle>();
		maxSpeed = m_vehicle.maxSpeed;
		isPlanar = m_vehicle.isPlanar;
		leaderController = leader.GetComponent<Vehicle>();
		arriveScript = GetComponent<SteeringForArrive>();	
		arriveScript.target = new GameObject("arriveTarget");
		//arriveScript.target = leader;
		arriveScript.target.transform.position = leader.transform.position;//new Vector3(0,0,0);
		//randomOffset = new Vector3(0,0,0);//(Random.Range(0,3),Random.Range(0,3),Random.Range(0,3));
	}
	
	public override Vector3 Force()
	{
		leaderVelocity = leaderController.velocity;
		target = leader.transform.position + LEADER_BEHIND_DIST * (-leaderVelocity).normalized;
		arriveScript.target.transform.position = target;// + randomOffset;

		return new Vector3(0,0,0);
	}


}
