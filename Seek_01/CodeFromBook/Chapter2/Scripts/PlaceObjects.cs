using UnityEngine;
using System.Collections;

public class PlaceObjects : MonoBehaviour {

	public GameObject objectsToPlace;
	public int count;
	public float radius;
	public bool isPlanar;

	void Awake()
	{
		Vector3 position = new Vector3(0,0,0);
		for (int i=0; i<count; i++)
		{
			position = transform.position + Random.insideUnitSphere * radius;
			if (isPlanar)
				position.y = objectsToPlace.transform.position.y;
			Instantiate(objectsToPlace, position , Quaternion.identity);
		}
	}


	void Start () {

	}
	

	void Update () {
	
	}
}
