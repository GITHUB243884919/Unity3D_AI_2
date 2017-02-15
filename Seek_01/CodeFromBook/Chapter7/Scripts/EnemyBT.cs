using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using React;
using Action = System.Collections.Generic.IEnumerator<React.NodeResult>;

public class EnemyBT : MonoBehaviour 
{
	private Animation anim;
	private EnemyAIController aiController;

	private bool delayFlag = false;
	private bool delayStart = false;
	private bool delayRandomFlag = false;
	private bool delayRandomStart = false;
	private float randomDelayTime = 0;

	//animation control
	private float standingTime;
	private bool finishStandingAnimation = false;
	private bool standingStart = false;

	private float crouchTime;
	private bool finishCrouchAnimation = false;
	private bool crouchStart = false;

	private float aimTime;
	private bool finishAimAnimation = false;
	private bool aimStart = false;

	private bool finishFireAnimation = false;
	private bool fireStart = false;
	private float randomFireTime = 0;

	private float reloadTime;
	private bool finishReloadAnimation = false;
	private bool reloadStart = false;


	void Start () 
	{
		anim = GetComponent<Animation>();
		aiController = GetComponent<EnemyAIController>();

		standingTime = anim["Standing"].length;
		crouchTime = anim["Crouch"].length;
		aimTime = anim["StandingAimCenter"].length;
		reloadTime = anim["Reload"].length;

	}

	void Update () {
	
	}

	//Crouch node-----------------------------------------------------------------------
	public Action Crouch()
	{
		Debug.Log("crouching");

		aiController.Crouch();

		if (!crouchStart)
		{
			StartCoroutine("WaitForCrouchAnimation");
			crouchStart = true;
		}
		
		if (finishCrouchAnimation)
		{
			finishCrouchAnimation = false;
			crouchStart = false;
			yield return NodeResult.Success;
		}
		else
		{
			yield return NodeResult.Continue;
		}
	}

	IEnumerator WaitForCrouchAnimation()
	{
		yield return new WaitForSeconds(crouchTime);
		finishCrouchAnimation = true;
	}


	//Stand node------------------------------------------------------------------------
	public Action Stand()
	{
		Debug.Log("standing!");

		aiController.Stand();
		
		if (!standingStart)
		{
			StartCoroutine("WaitForStandingAnimation");
			standingStart = true;
		}
		
		if (finishStandingAnimation)
		{
			finishStandingAnimation = false;
			standingStart = false;
			yield return NodeResult.Success;
		}
		else
		{
			yield return NodeResult.Continue;
		}
	}

	IEnumerator WaitForStandingAnimation()
	{
		yield return new WaitForSeconds(standingTime);
		finishStandingAnimation = true;
	}


	//Aim node-------------------------------------------------------------------------
	public Action Aim()
	{
		Debug.Log("aiming!");

		aiController.Aim();

		if (!aimStart)
		{
			StartCoroutine("WaitForAimAnimation");
			aimStart = true;
		}
		
		if (finishAimAnimation)
		{
			finishAimAnimation = false;
			aimStart = false;
			yield return NodeResult.Success;
		}
		else
		{
			yield return NodeResult.Continue;
		}
	}

	IEnumerator WaitForAimAnimation()
	{
		yield return new WaitForSeconds(aimTime);
		finishAimAnimation = true;
	}


	//Fire node------------------------------------------------------------------------
	public Action Fire()
	{
		Debug.Log("firing!");

		aiController.Fire();

		if (!fireStart)
		{
			StartCoroutine("WaitForFireAnimation");
			fireStart = true;
		}
		
		if (finishFireAnimation)
		{
			finishFireAnimation = false;
			fireStart = false;
			aiController.StopFire();
			yield return NodeResult.Success;
		}
		else
		{
			yield return NodeResult.Continue;
		}
	}

	IEnumerator WaitForFireAnimation()
	{
		yield return new WaitForSeconds(randomFireTime);
		finishFireAnimation = true;
	}

	//Choose random fire time--------------------------------
	public Action PickRandomFireTime()
	{
		Debug.Log("picking random fire time!");
		
		int minTime = 1;
		int maxTime = 4;
		randomFireTime = Random.Range(minTime,maxTime);
		
		yield return NodeResult.Success;
	}


	//Reload node-----------------------------------------------------------------------
	public Action Reload()
	{
		Debug.Log("reloading!");
		
		aiController.Reload();
		
		if (!reloadStart)
		{
			StartCoroutine("WaitForReloadAnimation");
			reloadStart = true;
		}
		
		if (finishReloadAnimation)
		{
			finishReloadAnimation = false;
			reloadStart = false;
			yield return NodeResult.Success;
		}
		else
		{
			yield return NodeResult.Continue;
		}
	}
	
	IEnumerator WaitForReloadAnimation()
	{
		yield return new WaitForSeconds(reloadTime);
		finishReloadAnimation = true;
	}


	//Delay node------------------------------------------------------------------------
	public Action Delay()
	{
		Debug.Log("delay 1 second!");

		if (!delayStart)
		{
			StartCoroutine("DelayForSeconds");
			delayStart = true;
		}

		if (delayFlag)
		{
			delayFlag = false;
			delayStart = false;
			yield return NodeResult.Success;
		}
		else
			yield return NodeResult.Continue;

	}

	IEnumerator DelayForSeconds()
	{
		yield return new WaitForSeconds(1);
		delayFlag = true;
	}


	//DelayRandom node------------------------------------------------------------------------
	public Action DelayRandom()
	{
		Debug.Log("delay random time!");
		
		if (!delayRandomStart)
		{
			randomDelayTime = Random.Range(2,7);
			StartCoroutine("DelayForRandomSeconds");
			delayRandomStart = true;
		}
		
		if (delayRandomFlag)
		{
			delayRandomFlag = false;
			delayRandomStart = false;
			yield return NodeResult.Success;
		}
		else
			yield return NodeResult.Continue;
		
	}
	
	IEnumerator DelayForRandomSeconds()
	{
		yield return new WaitForSeconds(randomDelayTime);

		delayRandomFlag = true;
	}


	//Go towards shelter-------------------------------------------------------------------
	public Action GoToShelter()
	{
		Debug.Log("going towards shelter!");
		
		aiController.runToDestination = true;
		
		aiController.RunTowardsShelter();
		
		if (Vector3.Distance(aiController.Shelter.transform.position, aiController.transform.position) > aiController.ShelterDistance)
		{
			yield return NodeResult.Continue;
		}
		else
		{
			aiController.runToDestination = false;
			yield return NodeResult.Success;
		}
	}


	//Select new shelter---------------------------------------------------------------------
	public Action SelectNewShelter()
	{
		Debug.Log("selecting new shelter!");

		aiController.SelectNewShelter();

		if (aiController.Shelter != null)
			yield return NodeResult.Success;
		else
			yield return NodeResult.Failure;
	}



	//Condition Nodes

	//Is player firing?------------------------------------------------------------------------------
	public bool isPlayerFiring()
	{
		Debug.Log("player firing?");

		return aiController.IsPlayerFiring();

	}


	//Is behind shelter?-----------------------------------------------------------------------------
	public bool isBehindShelter()
	{
		Debug.Log("behind shelter?");

		if (aiController.Shelter == null)
			return false;

		return Vector3.Distance(aiController.Shelter.transform.position, aiController.transform.position) <= aiController.ShelterDistance;
	}


	//Has shelter?-----------------------------------------------------------------------------------
	public bool isShelterLocated()
	{
		Debug.Log("has shelter?");

		return aiController.Shelter != null;
	}


	//Is not behind shelter?--------------------------------------------------------------------------
	public bool isNotBehindShelter()
	{
		Debug.Log("not behind shelter?");

		if (aiController.Shelter == null)
			return true;
		
		return Vector3.Distance(aiController.Shelter.transform.position, aiController.transform.position) > aiController.ShelterDistance;

	}


	public bool shouldReload()
	{
		Debug.Log("should reload?");

		return aiController.shouldReload();
	}
}
