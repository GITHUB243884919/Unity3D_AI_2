using UnityEngine;
using System.Collections;

public class HealthSensor : Sensor 
{
	//private AIController1 controller;

	void Start () 
	{
		sensorType = SensorType.health;
		manager.RegisterSensor(this);
	}	

	void Update () 
	{	
	}

	public override void Notify(Trigger t)
	{
		//controller.health += 20;
		//transform.localScale *= 1.3f;
	}
}
