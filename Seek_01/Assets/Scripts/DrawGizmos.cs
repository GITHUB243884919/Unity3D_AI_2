using UnityEngine;
using System.Collections;

public class DrawGizmos : MonoBehaviour 
{
	public float evadeDistance;
	private Vector3 center;
	private Vehicle vehicleScript;
	private float LEADER_BEHIND_DIST;

	// Use this for initialization
	void Start () 
	{
		vehicleScript = GetComponent<Vehicle>();
		LEADER_BEHIND_DIST = 2.0f;
	
	}
	
	// Update is called once per frame
	void Update () {
		center = transform.position + vehicleScript.velocity.normalized * LEADER_BEHIND_DIST;
	
	}

	void OnDrawGizmos()
	{
		Gizmos.DrawWireSphere(center, evadeDistance);
	}
}
