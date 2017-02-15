using UnityEngine;
using System.Collections;

public class TriggerRespawning : Trigger 
{
	protected int numUpdatesBetweenRespawns;
	protected int numUpdatesRemainingUntilRespawn;
	protected bool isActive;

	protected void SetActive()
	{
		isActive = true;
	}	
	
	protected void SetInactive()
	{
		isActive = false;
	}

	protected void Deactivate()
	{
		SetInactive();
		numUpdatesRemainingUntilRespawn = numUpdatesBetweenRespawns;
	}

	public override void Updateme()
	{
		if ((--numUpdatesRemainingUntilRespawn <= 0) && !isActive)
		{
			SetActive();
		}
	}


	protected void Start () 
	{
		isActive = true;
		base.Start();
	}


	void Update () 
	{	
	}
}
