using UnityEngine;
using System.Collections;

public class GenerateBoids : MonoBehaviour 
{
	public GameObject botPrefab;
	public int botCount;
	public float minX = 75.0f;
	public float maxX = 160.0f;
	public float minZ = -650.0f;
	public float maxZ = -600.0f;
	public float Yvalue = 4.043714f;
	
	// Use this for initialization
	void Start () 
	{
		Vector3 spawnPosition;
		GameObject bot;
		for (int i=0; i<botCount; i++)
		{
			spawnPosition = new Vector3(Random.Range(minX,maxX),Yvalue,Random.Range(minZ,maxZ));
			bot = Instantiate(botPrefab, spawnPosition,Quaternion.identity) as GameObject;			
		}	
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

