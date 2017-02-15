using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;

public class MultiTargetsPath : MonoBehaviour {

	public Transform targetPoints;
	private CharacterController controller;
	//一个Path类的对象，表示路径；
	public Path path;
	
	//角色每秒的速度；
	public float speed = 80;
	public float curRotSpeed = 6.0f;
	
	//当角色与一个航点的距离小于这个值时，角色便可转向路径上的下一个航点；
	public float nextWaypointDistance = 3;
	
	//角色正朝其行进的航点
	private int currentWaypoint = 0;


	void Start () {
		//Find the Seeker component
		Seeker seeker = GetComponent<Seeker>();
		controller = GetComponent<CharacterController>();
		//Make sure all OnComplete calls are called to the OnPathComplete function
		seeker.pathCallback = OnPathComplete;
		//Set the target points to all children of this GameObject

		Vector3[] endPoints = new Vector3[targetPoints.childCount];
		int c = 0;
		foreach (Transform child in targetPoints) {
			endPoints[c] = child.position;
			c++;
		}

		seeker.StartMultiTargetPath (transform.position,endPoints,false);

	}

	void FixedUpdate()
	{
		if (path == null)
			return;
		
		//如果当前路点编号大于这条路径上路点的总数，那么已经到达路径的终点；
		if (currentWaypoint >= path.vectorPath.Count)
		{
			Debug.Log ("End of Path Reached");
			return;
		}
		
		//计算出去往当前路点所需的行走方向和距离，控制角色移动；
		Vector3 dir = (path.vectorPath[currentWaypoint] - transform.position).normalized;
		dir *= speed * Time.fixedDeltaTime;
		controller.SimpleMove(dir);

		Quaternion targetRotation = Quaternion.LookRotation(dir);//destPos - transform.position);
		transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime*curRotSpeed);
		
		//如果当前位置与当前路点的距离小于一个给定值，可以转向下一个路点；
		if (Vector3.Distance( transform.position, path.vectorPath[currentWaypoint]) < nextWaypointDistance)
		{
			currentWaypoint ++;
			return;
		}

	}

	public void OnPathComplete (Path p) 
	{
		//Debug.Log ("Find the path "+p.error);
		
		//如果找到了一条路径，那么保存，并把第一个路点设置为当前路点；
		if (!p.error)
		{
			path = p;
			currentWaypoint = 0;
		}
	}

}

