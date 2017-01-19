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
    public bool isShowGizmos = false;


	void Start () {
		neighbors = new List<GameObject>();
	}
	

	void Update () {
		timer += Time.deltaTime;

		//ticked
		if (timer > checkInterval)
		{
			neighbors.Clear();
			//colliders = Physics.OverlapSphere(transform.position, detectRadius);//, layersChecked);
            colliders = Physics.OverlapSphere(transform.position, detectRadius, layersChecked);
			for (int i=0; i < colliders.Length; i++)
			{
                //Debug.Log("find neighbors " + colliders[i].name);
                if (colliders[i].GetComponent<Vehicle>())
                {
                    //Debug.Log("add neighbors");
                    neighbors.Add(colliders[i].gameObject);
                }
					
			}

			timer = 0;
		}
	}

    void OnDrawGizmos()
    {
        if (isShowGizmos)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, detectRadius);
        }

    }
}
