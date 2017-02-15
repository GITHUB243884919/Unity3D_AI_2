using UnityEngine;
using System.Collections;


public class SoundTrigger : TriggerLimitedLifetime 
{

	public override void Try(Sensor sensor)
	{
		if (isTouchingTrigger(sensor))
		{
			sensor.Notify(this);
		}
	}


	protected override bool isTouchingTrigger(Sensor sensor)
	{
		GameObject g = sensor.gameObject;
		
		if (sensor.sensorType == Sensor.SensorType.sound)
		{			
			if ((Vector3.Distance(transform.position, g.transform.position)) < (sensor as SoundSensor).hearingDistance)
			{
				return true;
			}				
		}

		return false;
	}


	void Start () 
	{
		//radius = 10;
		lifetime = 3;
		base.Start();
		manager.RegisterTrigger(this);   
	}
	

	void Update () 
	{	
	}

	/*
	void OnDrawGizmos()
	{
		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere(transform.position,radius);
	}*/
}
