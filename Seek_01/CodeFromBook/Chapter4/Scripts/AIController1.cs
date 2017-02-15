using UnityEngine;
using System.Collections;
using Pathfinding;

public class AIController1 : AIPath 
{
	public int health;
	public float arriveDistance = 1.0f;
	public Transform patrolWayPoints;
	public GameObject targetPrefab;
	public float shootingDistance = 7.0f;	
	public float chasingDistance = 8.0f;

	private Animation anim;
	private Blackboard bb;
	private int wayPointIndex = 0;
	private Vector3 personalLastSighting;
	private Vector3 previousSighting;
	private Vector3[] wayPoints;
	private Vector3 memoryPos;
	private SenseMemory memory;
	private float timer = 0;
	float stopDistance = 0.6f;
	float waitTime = 6.0f;

	public enum FSMState
	{
		Patrolling = 0,
		Chasing,
		Shooting,
	}

	private FSMState state;


	void Start () 
	{
		health = 30;	

		anim = GetComponent<Animation>();

		bb = GameObject.FindGameObjectWithTag("Blackboard").GetComponent<Blackboard>();
		personalLastSighting = bb.resetPosition;
		previousSighting = bb.resetPosition;

		memory = GetComponent<SenseMemory>();

		memoryPos = new Vector3(0,0,0);

		GameObject newTarget = Instantiate(targetPrefab, transform.position, transform.rotation) as GameObject;
		target = newTarget.transform;

		state = FSMState.Patrolling;

		wayPoints = new Vector3[patrolWayPoints.childCount];
		int c = 0;
		foreach (Transform child in patrolWayPoints) 
		{
			wayPoints[c] = child.position;
			c++;
		}

		target.position = wayPoints[0];

		base.Start();

	}


	public override void Update () 
	{
		if (bb.playerLastPosition != bb.resetPosition)
			memoryPos = bb.playerLastPosition;

		if (bb.playerLastPosition != previousSighting)
		{
			personalLastSighting = bb.playerLastPosition;	
		}

		//previousSighting = bb.playerLastPosition; 

		switch (state)
		{
		case FSMState.Patrolling:
			timer = 0;
			Patrolling();
			break;
		case FSMState.Chasing:
			Chasing();
			break;
		case FSMState.Shooting:
			timer = 0;
			Shooting();
			break;

		}

		previousSighting = bb.playerLastPosition; 

		Debug.Log(state);
	}

	bool CanSeePlayer()
	{
		if (memory != null)
			return memory.FindInList();
		else
			return false;
	}


	void Shooting()
	{
		state = FSMState.Shooting;
		Quaternion targetRotation = Quaternion.LookRotation(personalLastSighting-transform.position);
		transform.rotation = Quaternion.Slerp(transform.rotation,targetRotation, Time.deltaTime*turningSpeed);
		                                                  
		anim.Play("StandingFire");

		if (personalLastSighting == bb.resetPosition)
		{
			if (!CanSeePlayer())
			{
				state = FSMState.Patrolling;

			}
			else
			{
				state = FSMState.Chasing;
				target.position = memoryPos;
			}

			return;
		}

		if (!CanShoot() || ((personalLastSighting != previousSighting)&& Vector3.Distance(transform.position,personalLastSighting) > chasingDistance))
		{
			Debug.Log("change to chasing again.........................");
			state = FSMState.Chasing;
			target.position = personalLastSighting;
		}
	}

	bool CanShoot()
	{
		RaycastHit hit;
		Vector3 rayDirection = personalLastSighting - transform.position;
		rayDirection.y = 0;

		if (Physics.Raycast(transform.position + new Vector3(0,1,0), rayDirection, out hit, shootingDistance))
		{
			if (hit.collider.gameObject.tag == "Player")
			    return true;
		}

		return false;
	
	}

	void Chasing()
	{
		state = FSMState.Chasing;
		timer += Time.deltaTime;
		if (personalLastSighting == bb.resetPosition)
			target.position = memoryPos;
		else
			target.position = personalLastSighting;

		if (Vector3.Distance(transform.position, target.position) > shootingDistance)
		{
			base.Update();
			anim.CrossFade("Run");
		}
		else
		{
			if (CanSeePlayer() && CanShoot())
			{
				state = FSMState.Shooting;
			}
			else
			{
				if (Vector3.Distance(transform.position, target.position) > stopDistance)
				{
					base.Update();
					anim.CrossFade("Run");
				}
				else if (timer < waitTime)
				{
					Debug.Log("waiting");
					transform.Rotate(Vector3.up *30* Time.deltaTime, Space.World);
					anim.CrossFade("StandingAim");
				}
				else
				{
					state = FSMState.Patrolling;
				}
			}
		}

	}

	void Patrolling()
	{
		state = FSMState.Patrolling;

		if (Vector3.Distance(transform.position, target.position) < 3)
		{
			if (wayPointIndex == wayPoints.Length - 1)
			{
				wayPointIndex = 0;
				target.position = wayPoints[wayPointIndex];
			}
			else
			{
				wayPointIndex++;
				target.position = wayPoints[wayPointIndex];
			}
		}

		base.Update();

		anim.Play("Walk");

		if (personalLastSighting != bb.resetPosition)
			state = FSMState.Chasing;

	}

}
