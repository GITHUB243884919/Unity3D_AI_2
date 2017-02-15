using UnityEngine;
using System.Collections;

public class Blackboard : MonoBehaviour 
{
	public Vector3 playerLastPosition;
	public Vector3 resetPosition;
	public float lastSensedTime = 0;
	public float resetTime = 1.0f;

	// Use this for initialization
	void Start () 
	{
		playerLastPosition = new Vector3(100,100,100);
		resetPosition = new Vector3(100,100,100);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Time.time - lastSensedTime > resetTime)
		{
			playerLastPosition = resetPosition;
		}
	}
}
