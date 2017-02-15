using UnityEngine;
using System.Collections;

public class CollisionSound : MonoBehaviour 
{
	public AudioClip collisionSound;

	// Use this for initialization
	void Start () 
	{
		audio.PlayOneShot(collisionSound);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

}
