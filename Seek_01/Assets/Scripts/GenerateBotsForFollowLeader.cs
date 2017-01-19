using UnityEngine;
using System.Collections;

public class GenerateBotsForFollowLeader : MonoBehaviour 
{
	public GameObject botPrefab;
	public GameObject leader;
	public int botCount;
	//public GameObject target;
	public float minX = 88.0f;
	public float maxX = 150.0f;
	public float minZ = -640.0f;
	public float maxZ = -590.0f;
	public float Yvalue = 1.026003f;

	// Use this for initialization
	void Start () 
	{
		Vector3 spawnPosition;
		GameObject bot;
		for (int i=0; i<botCount; i++)
		{
			spawnPosition = new Vector3(Random.Range(minX,maxX),Yvalue,Random.Range(minZ,maxZ));
			bot = Instantiate(botPrefab, spawnPosition,Quaternion.identity) as GameObject;
			bot.GetComponent<SteeringForLeaderFollowing>().leader = leader;
			bot.GetComponent<SteeringForEvade>().target = leader;
			bot.GetComponent<SteeringForEvade>().enabled = false;
			bot.GetComponent<EvadeController>().leader = leader;
		}	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
