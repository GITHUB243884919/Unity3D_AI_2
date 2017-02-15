﻿using UnityEngine;
using System.Collections;

public class FSM : MonoBehaviour 
{
	//Player Transform
	protected Transform playerTransform;

	//Next destination position of the NPC Tank
	protected Vector3 destPos;

	//List of points for patrolling
	protected GameObject[] pointList;

	//Bullet shooting rate
	protected float shootRate;
	protected float elapsedTime;

	protected virtual void Initialize() {}
	protected virtual void FSMUpdate() {}
	protected virtual void FSMFixedUpdate() {}

	//Use this for initialization
	void Start()
	{
		Initialize();
	}
		
	// Update is called once per frame
	void Update () 
	{
		FSMUpdate();	
	}

	void FixedUpdate()
	{
		FSMFixedUpdate();
	}
}
