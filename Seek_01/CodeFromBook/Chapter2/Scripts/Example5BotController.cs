using UnityEngine;
using System.Collections;

public class Example5BotController : MonoBehaviour {

	private SteeringForEvade evadeScript;
	private SteeringForSeek seekScript;

	// Use this for initialization
	IEnumerator Start () 
	{
		evadeScript = GetComponent<SteeringForEvade>();
		seekScript = GetComponent<SteeringForSeek>();

		evadeScript.enabled = false;
		seekScript.enabled = true;

		yield return new WaitForSeconds(5);

		evadeScript.enabled = true;
		seekScript.enabled = false;	
	}
	
	// Update is called once per frame
	void Update () 
	{

	}

}
