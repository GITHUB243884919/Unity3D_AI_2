using UnityEngine;
using System.Collections;

public class SightSensor : Sensor 
{
	public float fieldOfView = 45;
	public float viewDistance = 100.0f;

	private AIController1 controller;
	private Blackboard bb;

	private SenseMemory memoryScript;


	void Start () 
	{
		controller = GetComponent<AIController1>();

		sensorType = SensorType.sight;
		manager.RegisterSensor(this);

		bb = GameObject.FindGameObjectWithTag("Blackboard").GetComponent<Blackboard>();
		memoryScript = GetComponent<SenseMemory>();
	}	

	void Update () 
	{	
	}

	public override void Notify(Trigger t)
	{
		//print ("I see a " + t.gameObject.name + "!");
		Debug.DrawLine(transform.position, t.transform.position, Color.red);

		if (t.tag == "Player")
		{
			bb.playerLastPosition = t.gameObject.transform.position;
			bb.lastSensedTime = Time.time;
			//controller.canSeePlayer = true;
		}

		if (memoryScript != null)
		{
			memoryScript.AddToList(t.gameObject, 1.0f);
		}

	}

	/*
	void OnDrawGizmos()
	{
		Vector3 frontRayPoint = transform.position + (transform.forward * viewDistance);
		float fieldOfViewinRadians = fieldOfView*3.14f/180.0f;
		Vector3 leftRayPoint = transform.TransformPoint(new Vector3(viewDistance * Mathf.Sin(fieldOfViewinRadians),0,viewDistance * Mathf.Cos(fieldOfViewinRadians)));
		Vector3 rightRayPoint = transform.TransformPoint(new Vector3(-viewDistance * Mathf.Sin(fieldOfViewinRadians),0,viewDistance * Mathf.Cos(fieldOfViewinRadians)));
		Debug.DrawLine(transform.position+new Vector3(0,1,0), frontRayPoint+new Vector3(0,1,0), Color.green);
		Debug.DrawLine(transform.position+new Vector3(0,1,0), leftRayPoint+new Vector3(0,1,0), Color.green);
		Debug.DrawLine(transform.position+new Vector3(0,1,0), rightRayPoint+new Vector3(0,1,0), Color.green);
	}*/

}
