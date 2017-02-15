using UnityEngine;
using System.Collections;

public class ColliderColorChange : MonoBehaviour 
{
	void Start () {
	
	}	

	void Update () {
	
	}

	void OnTriggerEnter(Collider other)
	{
		print("collide0!");
		if (other.gameObject.GetComponent<Vehicle>()!= null)
		{
			print("collide!");
			this.renderer.material.color = Color.red;
		}
	}

	void OnTriggerExit(Collider other)
	{
		this.renderer.material.color = Color.white;
	}
}
