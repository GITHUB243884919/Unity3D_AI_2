using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TriggerSystemManager : MonoBehaviour 
{
	List<Sensor> currentSensors = new List<Sensor>();
	List<Trigger> currentTriggers = new List<Trigger>();
	List<Sensor> sensorsToRemove;
	List<Trigger> triggersToRemove;


	void Start () 
	{
		sensorsToRemove = new List<Sensor>();
		triggersToRemove = new List<Trigger>();
	}


	private void UpdateTriggers()
	{
		foreach (Trigger t in currentTriggers)
		{
			if (t.toBeRemoved)
			{
				triggersToRemove.Add(t);
			}
			else
			{
				t.Updateme();
			}
		}

		foreach (Trigger t in triggersToRemove)
			currentTriggers.Remove(t);
	}

	private void TryTriggers()
	{
		foreach (Sensor s in currentSensors)
		{
			if (s.gameObject != null)   
			{
				foreach (Trigger t in currentTriggers)
				{
					t.Try(s);
				}
			}
			else
			{
				sensorsToRemove.Add(s);
			}
		}

		foreach (Sensor s in sensorsToRemove)
			currentSensors.Remove(s);
	}
	

	void Update ()   //Tick is also OK.
	{
		UpdateTriggers();
		TryTriggers();	
	}


	public void RegisterTrigger(Trigger t)
	{
		print ("registering trigger:" + t.name);
		currentTriggers.Add(t);
	}

	public void RegisterSensor(Sensor s)
	{
		print ("registering sensor:" + s.name + s.sensorType);
		currentSensors.Add(s);
	}
}
