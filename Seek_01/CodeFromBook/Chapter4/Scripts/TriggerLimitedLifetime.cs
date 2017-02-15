using UnityEngine;
using System.Collections;

public class TriggerLimitedLifetime : Trigger 
{
	protected int lifetime;

	public override void Updateme()
	{
		if (--lifetime <= 0)
		{
			toBeRemoved = true;
		}
	}


	void Start () 
	{
		base.Start();
	}
	

	void Update () 
	{	
	}
}
