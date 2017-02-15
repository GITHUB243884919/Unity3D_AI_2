using UnityEngine;
using System.Collections;

public class GenerateBots : MonoBehaviour 
{
	public GameObject botPrefab;
	public int botCount;
	public GameObject target;
	public float minX = 75.0f;
	public float maxX = 160.0f;
	public float minZ = -650.0f;
	public float maxZ = -600.0f;
	public float Yvalue = 4.043714f;


	void Start () 
	{
		Vector3 spawnPosition;
		GameObject bot;
		for (int i=0; i<botCount; i++)
		{
			spawnPosition = new Vector3(Random.Range(minX,maxX),Yvalue,Random.Range(minZ,maxZ));
			bot = Instantiate(botPrefab, spawnPosition,Quaternion.identity) as GameObject;
			bot.GetComponent<SteeringForArrive>().target = target;

		}	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
