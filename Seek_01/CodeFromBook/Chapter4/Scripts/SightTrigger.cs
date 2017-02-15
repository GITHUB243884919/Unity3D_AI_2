using UnityEngine;
using System.Collections;

public class SightTrigger : Trigger 
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

		if (sensor.sensorType == Sensor.SensorType.sight)
		{
			RaycastHit hit;
			Vector3 rayDirection = transform.position - g.transform.position;
			rayDirection.y = 0;
			
			if ((Vector3.Angle(rayDirection, g.transform.forward)) < (sensor as SightSensor).fieldOfView)
			{
				//print("here2");
				Debug.DrawRay(g.transform.position + new Vector3(0,1,0), rayDirection);
				if (Physics.Raycast(g.transform.position + new Vector3(0,1,0), rayDirection, out hit, (sensor as SightSensor).viewDistance))
				{
					//print("here3");
					if (hit.collider.gameObject == this.gameObject)
					{
						//print("here4");
						return true;
					}
				}
			}				
		}

		return false;
	}

	
	public override void Updateme()
	{
		position = transform.position;
	}


	void Start () 
	{
		base.Start();
		manager.RegisterTrigger(this); 
	}
	

	void Update () 
	{	
	}


}
