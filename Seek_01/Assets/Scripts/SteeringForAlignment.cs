using UnityEngine;
using System.Collections;

public class SteeringForAlignment : Steering {

	void Start () {
	
	}	

	public override Vector3 Force()
	{
		Vector3 averageDirection = new Vector3(0,0,0);
		int neighborCount = 0;

		foreach (GameObject s in GetComponent<Radar>().neighbors)
		{
			if ((s!=null)&&(s != this.gameObject))
			{
				averageDirection += s.transform.forward;
				neighborCount++;
			}
		}

		if (neighborCount > 0)
		{
			averageDirection /= (float)neighborCount;
			averageDirection -= transform.forward;
		}

		//print(gameObject.name + averageDirection);
		return averageDirection;
	}
}
