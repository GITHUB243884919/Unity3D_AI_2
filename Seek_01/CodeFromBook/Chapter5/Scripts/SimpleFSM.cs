using UnityEngine;
using System.Collections;

public class SimpleFSM : FSM 
{
	public enum FSMState
	{
		Patrol,
		Chase,
		Attack,
		Dead,
	}

	public float chaseDistance = 40.0f;
	public float attackDistance = 20.0f;
	public float arriveDistance = 3.0f;

	public Transform bulletSpawnPoint;

	//Get character controller
	private CharacterController controller;

	private Animation animComponent;

	//Current state that AI is reaching
	public FSMState curState;

	//speed of the AI
	//public float curSpeed = 120.0f;
	public float walkSpeed = 80.0f;
	public float runSpeed = 160.0f;

	//Rotation Speed
	public float curRotSpeed = 6.0f;

	//Bullet
	public GameObject Bullet;

	//Whether the AI is destroyed or not
	private bool bDead;
	private int health;

	//Initialize the Finite State machine for AI
	protected override void Initialize()
	{
		curState = FSMState.Patrol;

		bDead = false;
		elapsedTime = 0.0f;
		shootRate = 1.0f;
		health = 100;

		//Get the list of patrol points
		pointList = GameObject.FindGameObjectsWithTag("PatrolPoint");

		//Get charactercontroller
		controller = GetComponent<CharacterController>();
		//Get animation
		animComponent = GetComponent<Animation>();

		//Set Random destination point first
		FindNextPoint();

		//Get target(player)
		GameObject objPlayer = GameObject.FindGameObjectWithTag("Player");

		playerTransform = objPlayer.transform;

		if (!playerTransform)
			print ("Player doesn't exist, please add one player with Tag 'Player'");

	}


	protected override void FSMUpdate()
	{
		switch (curState)
		{
		case FSMState.Patrol: 
			UpdatePatrolState();
			break;
		case FSMState.Chase:
			UpdateChaseState();
			break;
		case FSMState.Attack:
			UpdateAttackState();
			break;
		case FSMState.Dead:
			UpdateDeadState();
			break;
		}

		//Update the time
		elapsedTime += Time.deltaTime;

		//Go to dead state when no health left
		if (health <= 0)
			curState = FSMState.Dead;
	}


	protected void UpdatePatrolState()
	{
		//Find another random patrol point if current point is reached
		//check distance from player, when AI is near the player, change to chase state
		if (Vector3.Distance(transform.position, destPos) <= arriveDistance)
		{
			print ("Reached to the destination point, calculating the next point");
			FindNextPoint();
		}
		else if (Vector3.Distance(transform.position, playerTransform.position) <= chaseDistance)
		{
			print ("Switch to chase state");
			curState = FSMState.Chase;
		}

		//Rotate to the target point
		Quaternion targetRotation = Quaternion.LookRotation(destPos - transform.position);
		transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime*curRotSpeed);

		//Go Forward
		controller.SimpleMove(transform.forward * Time.deltaTime * walkSpeed);

		animComponent.CrossFade("Walk");

	}


	protected void UpdateChaseState()
	{
		//Set target position as the player position
		destPos = playerTransform.position;

		//Check the distance with player when in a range, change to attack state
		//Go back to patrol if it becomes too far
		float dist = Vector3.Distance(transform.position, playerTransform.position);

		if (dist <= attackDistance)
		{
			curState = FSMState.Attack;
		}
		else if (dist >= chaseDistance)
		{
			curState = FSMState.Patrol;
		}

		//Rotate to the target point
		Quaternion targetRotation = Quaternion.LookRotation(destPos - transform.position);
		transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime*curRotSpeed);

		//Go Forward
		controller.SimpleMove(transform.forward * Time.deltaTime * runSpeed);

		animComponent.CrossFade("Run");

	}

	protected void UpdateAttackState()
	{
		Quaternion targetRotation;

		//Set target position as the player position
		destPos = playerTransform.position;

		//Check the distance with player when in a range, change to attack state
		float dist = Vector3.Distance(transform.position, playerTransform.position);

		if (dist >= attackDistance && dist < chaseDistance)
		{
			curState = FSMState.Chase;//FSMState.Attack;
			return;
		}
		else if (dist >= chaseDistance)  //change to patrol state if too far
		{
			curState = FSMState.Patrol;
			return;
		}

		targetRotation = Quaternion.LookRotation(destPos - transform.position);
		transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime*curRotSpeed);
		
		ShootBullet();

		animComponent.CrossFade("StandingFire");
	}

	private void ShootBullet()
	{
		if (elapsedTime >= shootRate)
		{
			GameObject bulletObj = Instantiate(Bullet, bulletSpawnPoint.position, transform.rotation) as GameObject;
			bulletObj.GetComponent<Bullet>().Go();
			elapsedTime = 0.0f;
		}
	}


	protected void UpdateDeadState()
	{
		//Play dead animation
		if (!bDead)
		{
			bDead = true;
			//animComponent.CrossFade("death");
		}
	}


	void onCollisionEnter(Collision collision)
	{
		//Reduce health
		if (collision.gameObject.tag == "Bullet")
		{
			health -= collision.gameObject.GetComponent<Bullet>().damage;
		}
	}


	protected void FindNextPoint()
	{
		print ("Finding next point");
		int rndIndex = Random.Range(0, pointList.Length);
		destPos = pointList[rndIndex].transform.position ;


	}

}
