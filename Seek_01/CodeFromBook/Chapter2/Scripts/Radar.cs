using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Radar : MonoBehaviour {

	private Collider[] colliders;
	private float timer = 0;
	public List<GameObject> neighbors;

	public float checkInterval = 0.3f;
	public float detectRadius = 10f;
	public LayerMask layersChecked ;


	void Start () {
		neighbors = new List<GameObject>();
	}
	

	void Update () {
		timer += Time.deltaTime;

		//ticked
		if (timer > checkInterval)
		{
			neighbors.Clear();
			colliders = Physics.OverlapSphere(transform.position, detectRadius);//, layersChecked);
			for (int i=0; i < colliders.Length; i++)
			{
				if (colliders[i].GetComponent<Vehicle>())
					neighbors.Add(colliders[i].gameObject);
			}

			timer = 0;
		}
	}
}
