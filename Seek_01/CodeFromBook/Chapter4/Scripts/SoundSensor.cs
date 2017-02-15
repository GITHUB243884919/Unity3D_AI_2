using UnityEngine;
using System.Collections;

public class SoundSensor : Sensor
{
	public float hearingDistance = 30.0f;
	//private AIController1 controller;
	private Blackboard bb;
	private SenseMemory memoryScript;

	void Start () 
	{
		//controller = GetComponent<AIController1>();

		sensorType = SensorType.sound;	
		manager.RegisterSensor(this);

		bb = GameObject.FindGameObjectWithTag("Blackboard").GetComponent<Blackboard>();
		memoryScript = GetComponent<SenseMemory>();
	}
	

	void Update () 
	{

	}

	public override void Notify(Trigger t)
	{
		print ("I hear some sound at" + t.gameObject.transform.position + Time.time);

		if (memoryScript != null)
		{
			memoryScript.AddToList(t.gameObject, 0.66f);
		}

		bb.playerLastPosition = t.gameObject.transform.position;
		bb.lastSensedTime = Time.time;
		//controller.personalLastSighting = t.gameObject.transform.position;


	}

}
