using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour 
{
	public float speed = 100;
	public float lifeTime = 6f;

	[HideInInspector]
	public Vector3 direction;

	void Start()
	{
		Destroy(gameObject, lifeTime);
	}


	void Update () 
	{
		transform.position += direction * speed * Time.deltaTime;
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			Debug.Log("hit the player!");

			PlayerController controller;
			controller = other.GetComponent<PlayerController>();
			controller.health -= 20;

			Destroy(gameObject);
		}

		if (other.tag == "Enemy")
		{
			Debug.Log("hit the enemy!");
			
			EnemyAIController controller;
			controller = other.GetComponent<EnemyAIController>();
			controller.health -= 20;
			
			Destroy(gameObject);
		}

	}


}
