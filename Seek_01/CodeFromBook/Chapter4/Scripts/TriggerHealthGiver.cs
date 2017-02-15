using UnityEngine;
using System.Collections;

public class TriggerHealthGiver : TriggerRespawning 
{
	public int healthGiven = 10;

	public override void Try(Sensor sensor)
	{
		if (isTouchingTrigger(sensor) && isActive)
		{
			//Health healthScript = sensor.gameObject.GetComponent<Health>();
			AIController1 controller = sensor.GetComponent<AIController1>();
			if (controller != null)
			{
				//healthScript.health += healthGiven;
				controller.health += healthGiven;
				//print("now my health is :" + healthScript.health);
				print("now my health is :" + controller.health);
				this.renderer.material.color = Color.green;
				StartCoroutine("TurnColorBack");
				sensor.Notify(this);

			}
			else
				print ("Can't get health script!");

			Deactivate();
		}
	}

	IEnumerator TurnColorBack()
	{
		yield return new WaitForSeconds(3);
		this.renderer.material.color = Color.black;
	}


	protected override bool isTouchingTrigger(Sensor sensor)
	{
		GameObject g = sensor.gameObject;
		
		if (sensor.sensorType == Sensor.SensorType.health)
		{			
			if ((Vector3.Distance(transform.position, g.transform.position)) < radius) 
			{
				return true;
			}				
		}
		
		return false;
	}


	void Start () 
	{
		//radius = 10;
		numUpdatesBetweenRespawns = 6000;
		base.Start();
		manager.RegisterTrigger(this); 
	}
	

	void Update () 
	{	
	}

	/*
	void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(transform.position,radius);
	}*/
}
