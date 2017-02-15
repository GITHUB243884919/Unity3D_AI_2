using UnityEngine;
using System.Collections;

public class Sensor : MonoBehaviour 
{
	protected TriggerSystemManager manager;

	public enum SensorType
	{
		sight,
		sound,
		health
	} 

	public SensorType sensorType;

	void Awake()
	{
		manager = FindObjectOfType<TriggerSystemManager>();
	}

	void Start () 
	{	
	}

	void Update () 
	{	
	}

	public virtual void Notify(Trigger t)
	{
		//there may be many signals at a time, just put them in a list, then sort them to find the most important one to deal with
	}
}
