using UnityEngine;
using System.Collections;

public class Bottle : MonoBehaviour 
{
	public GameObject collisionPrefab;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.tag != "Ground")
		{
			Debug.Log("Hit the bottle!");

			Instantiate(collisionPrefab, transform.position, Quaternion.identity);

			Destroy(this);
		}
	}
}
