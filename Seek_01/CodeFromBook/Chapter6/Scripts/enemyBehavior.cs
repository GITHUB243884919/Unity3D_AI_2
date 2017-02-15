using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using React;

using  Action = System.Collections.Generic.IEnumerator<React.NodeResult>;

public class enemyBehavior : MonoBehaviour {

	Animation animComponent;
	CharacterController controller;
	public float runSpeed = 70.0f;
	public Transform player;

	private Vector3 walkDirection;
	private bool isRunningToTarget = false;
	private bool runToTarget = false;
	private float turnSpeed = 6.0f;
	private float shootDistance = 100.0f;

	void Start () {
		animComponent = GetComponent<Animation>();
		controller = GetComponent<CharacterController>();
		animComponent.wrapMode = WrapMode.Loop;
	}
	

	void Update () 
	{
		walkDirection = player.position - transform.position;

		Quaternion targetRotation = Quaternion.LookRotation(walkDirection);
		transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime*turnSpeed);
		
		if (runToTarget)
		{
			controller.SimpleMove(walkDirection.normalized * runSpeed * Time.deltaTime);
		}
	}

	public Action Idle()
	{
		print("now Idle");
		animComponent.CrossFade("Idle");
		yield return NodeResult.Success;
	}

	public Action Attack()
	{
		print("now attacking");

		//Quaternion targetRotation = Quaternion.LookRotation(player.position - transform.position);
		//transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime*turnSpeed);

		animComponent.CrossFade("StandingFire");
		yield return NodeResult.Success;
	}

	public Action Reload()
	{
		print("now reloading");
		animComponent.CrossFade("StandingReloadM4");
		yield return NodeResult.Success;
	}

	public Action MoveToTarget()
	{
		runToTarget = true;

		if (walkDirection.sqrMagnitude > shootDistance)
		{
			print("now moving to target");
			animComponent.CrossFade("Run");
			yield return NodeResult.Continue;
		}
		else
		{
			runToTarget = false;
			yield return NodeResult.Success;
		}
	}

	public bool CanSeePlayer ()
	{
		var playerDirection = player.position - transform.position;
		playerDirection.y = 0;
		var ray = new Ray (transform.position+new Vector3(0,1,0), playerDirection);

		var inFOV = Vector3.Angle (transform.forward, playerDirection) < 45; 
		if (inFOV) 
		{
			RaycastHit hit;
			if (Physics.Raycast (ray, out hit, 10000)) 
			{
				return hit.collider.transform == player;
			} 		
		}

		Debug.Log("can't see player!");
		return false;
	} 
}
