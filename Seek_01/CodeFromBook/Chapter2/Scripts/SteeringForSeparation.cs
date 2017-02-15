using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Radar))]
public class SteeringForSeparation : Steering {

	public float comfortDistance = 1;
	public float multiplierInsideComfortDistance = 2;

	void Start () {
	
	}

	public override Vector3 Force()
	{
		Vector3 steeringForce = new Vector3(0,0,0);

		foreach (GameObject s in GetComponent<Radar>().neighbors)
		{
			if ((s!=null)&&(s != this.gameObject))
			{
				Vector3 toNeighbor = transform.position - s.transform.position;
				float length = toNeighbor.magnitude;
				steeringForce += toNeighbor.normalized / length;
				if (length < comfortDistance)
					steeringForce *= multiplierInsideComfortDistance;
			}
		}

		return steeringForce;
	}
}
