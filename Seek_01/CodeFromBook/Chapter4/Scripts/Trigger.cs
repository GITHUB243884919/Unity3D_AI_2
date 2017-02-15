using UnityEngine;
using System.Collections;

public class Trigger : MonoBehaviour 
{
	protected TriggerSystemManager manager;

	protected Vector3 position;
	public int radius;
	public bool toBeRemoved;

	public virtual void Try(Sensor s) 	{	}

	public virtual void Updateme()	{	}


	protected virtual bool isTouchingTrigger(Sensor sensor)
	{
		return false;
	}


	void Awake()
	{
		manager = FindObjectOfType<TriggerSystemManager>();
	}


	protected void Start ()   
	{
		toBeRemoved = false;			
	}
	

	void Update () 
	{	
	}
}
